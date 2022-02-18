using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;

namespace Suffer_Travels.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext db;
        public HotelController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Index()
        {
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
                return View();
                        
            return RedirectToAction("HomePage", "User");
        }

        [NonAction]
        public bool UserLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }
    }
}
