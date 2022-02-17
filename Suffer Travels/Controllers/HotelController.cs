using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;

namespace Suffer_Travels.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HotelController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public bool userLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        public bool isHotelUser()
        {
            return HttpContext.Session.GetInt32("RoleId") == 3;
        }

        public IActionResult RegisterHotel()
        {
            if (userLoggedOut())
                return RedirectToAction("Login", "User");

            if (isHotelUser())
                return View();
                        
            return RedirectToAction("HomePage", "User");
        }
    }
}
