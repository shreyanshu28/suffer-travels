using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;

namespace Suffer_Travels.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VehicleController(ApplicationDbContext db)
        {
            _db = db;
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
            return View();
        }
    }
}
