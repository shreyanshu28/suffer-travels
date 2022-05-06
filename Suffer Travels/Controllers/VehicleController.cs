using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;

namespace Suffer_Travels.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public VehicleController(ApplicationDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        public bool UserLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        public bool IsVehicleUser()
        {
            return HttpContext.Session.GetInt32("RoleId") == 4;
        }

        public IActionResult Home()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (!IsVehicleUser())
                return RedirectToAction("Home", "User");

            SetViewData();
            return View();
        }

        public void SetViewData()
        {
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto");
        }
    }
}
