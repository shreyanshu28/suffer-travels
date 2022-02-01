using Microsoft.AspNetCore.Mvc;

namespace Suffer_Travels.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
