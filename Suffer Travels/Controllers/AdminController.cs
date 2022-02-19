using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
/*        public IActionResult AddTourDetails(TourViewModel tour)
        {
            //CalculateDates(tour.repeatsEvery, tour.recurrence);

            try
            {
                db.tblTour.Add(tour.tourDetail);
                db.SaveChanges();
                TempData["Success"] = "Added Successfully";

            }
            catch (Exception e) {
                TempData["Error"] = e;
            }
            return RedirectToAction("ManageTours");
        }*/

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
            string description = tourViewModel.tourDetail.TourName;
            db.tblTour.Add(tourViewModel.tourDetail);
            
            _photos.Description = description;
            
            db.tblPhotos.Add(_photos);
            db.SaveChanges();

            TourPhotos _tourPhotos = new TourPhotos();
            _tourPhotos.TourId = db.tblTour.FirstOrDefault(t => t.TourName == tourViewModel.tourDetail.TourName).TId;
            _tourPhotos.PhotoId = db.tblPhotos.FirstOrDefault(p => p.ImagePath == _photos.ImagePath).PId;
            db.tblTourPhotos.Add(_tourPhotos);
            //_ = db.SaveChangesAsync();
            db.SaveChanges();
            TempData["Success"] = "Data added successfully";
            return RedirectToAction("ManageTours");

            /*TempData["Error"] = "Something is wrong";
            return RedirectToAction("ManageTours");*/
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTourTypeDetails(TourViewModel tourType)
        {
            db.tblTourType.Add(tourType.tourTypeDetails);
            db.SaveChanges();

            /*var files = HttpContext.Request.Form.Files;
            //var tt = db.tblTourType;
            foreach (var Image in files)
            {
                string[] Images = Image.FileName.Split(".");
                string extension = Images[Images.Length - 1].ToLower();
                if (extension != "jpg" && extension != "png")
                {
                    TempData["Error"] = "Only jpg and png image formates are supported!";
                    return RedirectToAction("ManageTours");
                }
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;

                    var uploads = Path.Combine(env.WebRootPath, "photos\\user");
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);

                            TourType tt = new TourType();
                            tt.Photo = fileName;
                            tt.TtName = tourType.tourTypeDetails.TtName;
                            db.tblTourType.Add(tt);

                            //db.tblTourType.Add(tourType.tourTypeDetails;
                            db.SaveChanges();
                        }
                    }
                }
                TempData["Success"] = "Tour Type Added Successfuly";
                return RedirectToAction("ManageTours");
            }
            TempData["Error"] = "Error in adding files";
*/            return RedirectToAction("ManageTours");
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

        public String ApproveStatus()
        {
            return "Approved";
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

        public String DeclineStatus()
        {
            return "Declined";
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
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfiePhoto"] = HttpContext.Session.GetString("ProfilePhoto");

            if (UserLoggedOut())
                return RedirectToAction("Login", "User");
            IEnumerable<User> _user = db.tblUser;
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            return View(_user);
        }
    }
}
