using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using Suffer_Travels.ViewModel;

namespace Suffer_Travels.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public HotelController(ApplicationDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        public IActionResult Home()
        {
            /*String email = HttpContext.Session.GetString("Email").ToString();
            ViewData["ApprovalFlag"] = db.tblUser.First(user => user.Email == email).Status;*/
            SetViewData();
            return View();
        }

        [NonAction]
        public bool IsHotelUser()
        {
            return HttpContext.Session.GetInt32("RoleId") == 3;
        }        

        
        public IActionResult RegisterHotel()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (IsHotelUser())
            {
                HotelViewModel hotelViewModel = new HotelViewModel();
                hotelViewModel.cities = db.tblCity;
                SetViewData();
                return View(hotelViewModel);                
            }

            return RedirectToAction("Home", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterHotelAsync(HotelViewModel hotelViewModel)
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsHotelUser())
                return RedirectToAction("Home", "User");

            SetViewData();

            string filen = "";

            Hotel hotel = new Hotel();
            hotel.HName = hotelViewModel.hotel.HName;
            hotel.HotelType = hotelViewModel.hotel.HotelType;
            hotel.ContactNo = hotelViewModel.hotel.ContactNo;
            hotel.AreaId = hotelViewModel.hotelAddress.CityId;

            HotelAddress hotelAddress = new HotelAddress();
            hotelAddress.AddressLine1 = hotelViewModel.hotelAddress.AddressLine1;
            hotelAddress.AddressLine2 = hotelViewModel.hotelAddress.AddressLine2;
            hotelAddress.CityId = hotelViewModel.hotelAddress.CityId;            

            var dbTrans = db.Database.BeginTransaction();
            try
            {
                db.tblHotelMaster.Add(hotel);
                db.SaveChanges();
                db.tblHotelAddress.Add(hotelAddress);
                db.SaveChanges();

                Photo photo = new Photo();

                var files = HttpContext.Request.Form.Files;

                foreach (var Image in files)
                {
                    string[] Images = Image.FileName.Split(".");
                    string extension = Images[Images.Length - 1].ToLower();
                    if (extension != "jpg" && extension != "png")
                    {
                        ModelState.AddModelError("photo.ImagePath", "Only jpg and png image formates are supported!");
                        return View(hotelViewModel);
                    }
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;

                        var uploads = Path.Combine(env.WebRootPath, "photos\\hotel");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                photo.ImagePath = fileName;
                                photo.Description = hotel.HName;
                                filen = fileName;
                                db.tblPhotos.Add(photo);
                                db.SaveChanges();

                            }
                        }
                    }
                }

                HotelPhotos hotelPhotos = new HotelPhotos();
                hotelPhotos.HId = db.tblHotelMaster.FirstOrDefault(h => h.HName == hotelViewModel.hotel.HName).HId;
                hotelPhotos.PID = db.tblPhotos.FirstOrDefault(p => p.ImagePath == filen).PId;
                db.tblHotelPhotos.Add(hotelPhotos);
                db.SaveChanges();

                /*HotelRooms hotelRooms = new HotelRooms();
                hotelRooms.TotalRooms = 100;
                hotelRooms.HId = db.tblHotelMaster.FirstOrDefault(h => h.HName == hotelViewModel.hotel.HName).HId;*/
                for(int i=0; i<5; ++i)
                {   
                    if(hotelViewModel.hrList[i].TotalRooms == 0)
                    {
                        continue;
                    }

                    HotelRooms hotelRooms = new HotelRooms();
                    hotelRooms.TotalRooms = hotelViewModel.hrList[i].TotalRooms;
                    hotelRooms.HId = db.tblHotelMaster.FirstOrDefault(h => h.HName == hotelViewModel.hotel.HName).HId;
                    hotelRooms.Price = hotelViewModel.hrList[i].Price;
                    hotelRooms.Capacity = hotelViewModel.hrList[i].Capacity;
                    db.tblHotelRooms.Add(hotelRooms);
                    db.SaveChanges();

                }
                //db.SaveChanges();
                dbTrans.Commit();

                TempData["success"] = "Details added successfully";
                return RedirectToAction("Home");
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                TempData["Error"] = "Error";
                return RedirectToAction("Home");
            }

        }

        [NonAction]
        public void SetViewData()
        {
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto");
            String email = HttpContext.Session.GetString("Email").ToString();
            ViewData["ApprovalFlag"] = db.tblUser.First(user => user.Email == email).Status;
        }

        [NonAction]
        public bool UserLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }
    }
}
