using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using Suffer_Travels.ViewModel;
using System.Net;
using System.Net.Mail;

namespace Suffer_Travels.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public AdminController(ApplicationDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        public IActionResult AddCities()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsAdminUser())
                return RedirectToAction("Home", "User");

            TourViewModel tourViewModel = new TourViewModel();

            tourViewModel.cities = db.tblCity;
            tourViewModel.states = db.tblState;
            tourViewModel.countries = db.tblCountry;

            SetViewData();

            return View(tourViewModel);
        }

        public IActionResult AddCityDetails(TourViewModel tourViewModel)
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsAdminUser())
                return RedirectToAction("Home", "User");

            if(tourViewModel.city.StateId == 0)
            {
                if(tourViewModel.country.CId == 0)
                {
                    //db.tblTourType.FirstOrDefault(t => t.TtName == tourViewModel.tourTypeDetails.TtName).TtId;
                    db.tblCountry.Add(tourViewModel.country);
                    db.SaveChanges();
                    tourViewModel.state.CountryId = db.tblCountry.FirstOrDefault(c => c.Cname == tourViewModel.country.Cname).CId;
                }

                else
                {
                    db.tblState.Add(tourViewModel.state);
                    db.SaveChanges();
                    tourViewModel.city.StateId = db.tblState.FirstOrDefault(s => s.Sname == tourViewModel.state.Sname).SId;
                }
            }

            db.tblCity.Add(tourViewModel.city);
            db.SaveChanges();

            TempData["success"] = "Cities Added Successfully";

            return RedirectToAction("AddCities");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTourDetailsAsync(TourViewModel tourViewModel)
        {
            var files = HttpContext.Request.Form.Files;

            Photo _photos = new Photo();

            foreach (var Image in files)
            {
                string[] Images = Image.FileName.Split(".");
                string extension = Images[Images.Length - 1].ToLower();
                if (extension != "jpg" && extension != "png")
                {
                    TempData["Error"] = "Only jpg and png files are allowed";
                    return RedirectToAction("ManageTours");
                }
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;
                    var uploads = Path.Combine(env.WebRootPath, "photos\\tour");
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            _photos.ImagePath = fileName;
                        }
                    }
                }
            }

            //db.tblTourType.Add(tourViewModel.tourTypeDetails);

            string description = tourViewModel.tourDetail.TourName;

            if (tourViewModel.tourDetail.TourTypeId == 0)
            {
                db.tblTourType.Add(tourViewModel.tourTypeDetails);
                db.SaveChanges();
                tourViewModel.tourDetail.TourTypeId = db.tblTourType.FirstOrDefault(t => t.TtName == tourViewModel.tourTypeDetails.TtName).TtId;
            }
            db.tblTour.Add(tourViewModel.tourDetail);

            _photos.Description = description;

            db.tblPhotos.Add(_photos);
            db.SaveChanges();

            TourPhotos _tourPhotos = new TourPhotos();
            _tourPhotos.TourId = db.tblTour.FirstOrDefault(t => t.TourName == tourViewModel.tourDetail.TourName).TId;
            _tourPhotos.PhotoId = db.tblPhotos.FirstOrDefault(p => p.ImagePath == _photos.ImagePath).PId;
            db.tblTourPhotos.Add(_tourPhotos);

            TourDates tourDates = new TourDates();
            DateTime sixMonths = tourViewModel.tourDate.Date.AddMonths(6);
            List<TourDates> td = new List<TourDates>();
            switch (tourViewModel.recurrence)
            {
                case ("Day"):
                    while (tourViewModel.tourDate.Date < sixMonths)
                    {
                        td.Add(new TourDates { Date = tourViewModel.tourDate.Date, Time = DateTime.Now, TourId = _tourPhotos.TourId });

                        tourViewModel.tourDate.Date = tourViewModel.tourDate.Date.AddDays(tourViewModel.repeatsEvery);
                    }
                    break;

                case ("Week"):
                    while (tourViewModel.tourDate.Date < sixMonths)
                    {
                        td.Add(new TourDates { Date = tourViewModel.tourDate.Date, Time = DateTime.Now, TourId = _tourPhotos.TourId });

                        tourViewModel.tourDate.Date = tourViewModel.tourDate.Date.AddDays(tourViewModel.repeatsEvery * 7);
                    }
                    break;

                case ("Month"):
                    while (tourViewModel.tourDate.Date < sixMonths)
                    {
                        td.Add(new TourDates { Date = tourViewModel.tourDate.Date, Time = DateTime.Now, TourId = _tourPhotos.TourId });

                        tourViewModel.tourDate.Date = tourViewModel.tourDate.Date.AddMonths((int)tourViewModel.repeatsEvery);
                    }
                    break;

                case ("Year"):
                    while (tourViewModel.tourDate.Date < sixMonths.AddYears(6))
                    {
                        td.Add(new TourDates { Date = tourViewModel.tourDate.Date, Time = DateTime.Now, TourId = _tourPhotos.TourId });

                        tourViewModel.tourDate.Date = tourViewModel.tourDate.Date.AddYears((int)tourViewModel.repeatsEvery);
                    }
                    break;

                default:
                    break;
            }

            db.tblTourDates.AddRange(td);
            db.SaveChanges();

            TempData["Success"] = "Data added successfully";

            return RedirectToAction("ManageTours");
        }

        public IActionResult AddTourItineary(uint? id)
        {
            TourViewModel tourViewModel;
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsAdminUser())
                return RedirectToAction("Home", "User");

            SetViewData();

            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ViewUsers");
            }

            tourViewModel = new TourViewModel();
            tourViewModel.tourDetail = db.tblTour.First(t => t.TId == id);
            tourViewModel.NoOfDays = Convert.ToInt32(tourViewModel.tourDetail.NoOfDays);
            tourViewModel.TourId = id;

            tourViewModel.cities = db.tblCity;
            tourViewModel.countries = db.tblCountry;
            tourViewModel.states = db.tblState;
            tourViewModel.landmarks = db.tblLandMark;

            //tourViewModel.tourDetail = db.tblTour.Find(id);

            return View(tourViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTourItineary([FromRoute]uint? id, TourViewModel tourViewModel)
        {
            List<TourItinerary> tiList = new List<TourItinerary>();
            IEnumerable<TourItinerary> tourItineraries = tourViewModel.tiList;
            
            if(id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("AddTourItineary");
            }
            int day = 1;
            foreach(var item in tourItineraries)
            {
                tiList.Add(new TourItinerary
                {
                    TourId = Convert.ToUInt32(id),
                    Day = Convert.ToUInt32(day),
                    CityId = Convert.ToUInt32(item.CityId),
                    Description = item.Description
                });
                day++;
            }

            db.tblTourItinerary.AddRange(tiList);
            db.SaveChanges();

            return RedirectToAction("ManageTours");
        }

        public IActionResult ApproveRole(uint? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ViewUsers");
            }
            var _user = db.tblUser.Find(id);
            _user.Status = ApproveStatus();
            _user.ChangedAt = DateTime.Today;
            db.tblUser.Update(_user);
            db.SaveChanges();
            TempData["Success"] = "Request Approved";
            SendRoleNotification(_user.Email.ToString(), HttpContext.Session.GetString("Fname"), ApproveStatus());
            return RedirectToAction("ViewUsers");
        }

        [NonAction]
        public String ApproveStatus()
        {
            return "Approved";
        }        

        public IActionResult ActivateTour(uint? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ManageTours");
            }

            var _tour = db.tblTour.Find(id);
            _tour.IsActive = true;
            db.tblTour.Update(_tour);
            db.SaveChanges();
            TempData["Success"] = "Tour is now active";

            return RedirectToAction("ManageTours");
        }

        public IActionResult DeactivateTour(uint? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ManageTours");
            }

            var _tour = db.tblTour.Find(id);
            _tour.IsActive = false;
            db.tblTour.Update(_tour);
            db.SaveChanges();
            TempData["Success"] = "Tour is deactivated";

            return RedirectToAction("ManageTours");
        }

        public IActionResult DeclineRole(uint? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ViewUsers");
            }

            var _user = db.tblUser.Find(id);
            _user.Status = DeclineStatus();
            _user.ChangedAt = DateTime.Today;
            db.tblUser.Update(_user);
            db.SaveChanges();
            TempData["Success"] = "Request Declined";
            SendRoleNotification(_user.Email.ToString(), HttpContext.Session.GetString("Fname"), DeclineStatus());
            return RedirectToAction("ViewUsers");
        }
        
        [NonAction]
        public String DeclineStatus()
        {
            return "Declined";
        }

        public IActionResult EditTour(uint? id)
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsAdminUser())
                return RedirectToAction("Home", "User");

            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ViewUsers");
            }

            SetViewData();
            /*TourViewModel tourViewModel = new TourViewModel();
            tourViewModel.tourDetails
            Tour _tour = db.tblTour.Find(id);*/

            TourViewModel tourViewModel = new TourViewModel();
            tourViewModel.tourDetail = db.tblTour.Find(id);
            tourViewModel.tourTypes = db.tblTourType;

            return View(tourViewModel);
        }

        public IActionResult EditTourDetails(TourViewModel tourViewModel)
        {
            var tourDetails = db.tblTour.Find(tourViewModel.tourDetail.TId);
            //var tourDetails = db.tblTour.Find(tourViewModel.tourDetail.TId);
            tourDetails.TourName = tourViewModel.tourDetail.TourName;
            tourDetails.TotalSeats = tourViewModel.tourDetail.TotalSeats;
            tourDetails.Description = tourViewModel.tourDetail.Description;
            tourDetails.NoOfDays = tourViewModel.tourDetail.NoOfDays;
            tourDetails.Price = tourViewModel.tourDetail.Price;
            tourDetails.PriceChildren = tourViewModel.tourDetail.PriceChildren;
            tourDetails.PriceInfant = tourViewModel.tourDetail.PriceInfant;
            db.tblTour.Update(tourDetails);
            db.SaveChanges();

            TempData["Success"] = "Tour succesfully edited!";

            return RedirectToAction("ManageTours");
        }

        public IActionResult Home()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsAdminUser())
                return RedirectToAction("Home", "User");

            SetViewData();
            return View();
        }

        public bool IsAdminUser()
        {
            return HttpContext.Session.GetInt32("RoleId") == 1;
        }

        public IActionResult ManageTours()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsAdminUser())
                return RedirectToAction("Home", "User");

            TourViewModel tourViewModel = new TourViewModel();

            tourViewModel.tourDetails = db.tblTour;
            tourViewModel.tourTypes = db.tblTourType;
            SetViewData();
            return View(tourViewModel);
        }

        [HttpPost]
        public IActionResult SaveIteneraryDetails(string TourItineary)
        {
            HttpContext.Session.SetString("TourItineary", TourItineary);
            return View();
        }

        
        public IActionResult SetIteneraryDetails()
        {
            TourItinerary tourItinerary = new TourItinerary();
            List<TourItinerary> tourItineraries = new List<TourItinerary>();
            //dynamic TourItineary = JsonConvert.DeserializeObject(HttpContext.Session.GetString("TourItineary"));
            dynamic TourItineary = JsonConvert.DeserializeObject(HttpContext.Session.GetString("TourItineary"));
            //int tiid = 1; 
            foreach ( var item in TourItineary)
            {
                tourItineraries.Add(new TourItinerary
                {
                    TourId = (UInt32)item["TourId"],
                    Day = (UInt32)item["Day"],
                    Description = item["Description"],
                    CityId = (UInt32) item["CityId"],
                });
            }
            db.tblTourItinerary.AddRange(TourItineary);
            //db.tblTourItinerary.Add(TourItineary);
            db.SaveChanges();

            TempData["Success"] = "Itenerary Added Successfully";
            return RedirectToAction("ManageTours");
        }

        public void SetViewData()
        {
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto");
        }

        public int SendRoleNotification(string toEmail, string username, string status)
        {
            string email = "suffertravelco@gmail.com", pass = "tavabiryani";
            string server = "smtp.gmail.com";
            string approval_message = @"<span style='font-weight: bold; font-size: 25px; '>  Your request has been approved </span>";
            string decline_message = @"<span style='font-weight: bold; font-size: 25px; '>  Your request has been declined. You can try again. </span>";

            MailAddress from = new MailAddress(email, "Suffer Travels");
            MailAddress to = new MailAddress(toEmail, "shreyanshu vyas");
            MailMessage message = new MailMessage(from, to);

            if (status.Equals(ApproveStatus()))
            {
                message.Subject = "Congratulations! Your request has been approved!";
                message.Body = approval_message;

            }
            else if (status.Equals(DeclineStatus()))
            {
                message.Subject = "Oh no! Your request is declined";
                message.Body = decline_message;
            }

            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(server);

            // Include credentials if the server requires them.
            client.Port = 587;
            client.Credentials = new NetworkCredential(from.Address, pass);
            client.EnableSsl = true;

            Console.WriteLine("Sending an email message to {0} by using the SMTP host {1}.",
                to.Address, client.Host);

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateCopyMessage(): {0}",
                    ex.ToString());
                return 0;
            }
            return 1;
        }

        public bool UserLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        public IActionResult ViewUsers()
        {
            SetViewData();

            if (UserLoggedOut())
                return RedirectToAction("Login", "User");
            IEnumerable<User> _user = db.tblUser;
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            return View(_user);
        }
    }
}
