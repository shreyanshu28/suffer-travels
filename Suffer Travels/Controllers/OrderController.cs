using Microsoft.AspNetCore.Mvc;

namespace Suffer_Travels.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}
