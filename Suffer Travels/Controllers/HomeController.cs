using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using Suffer_Travels.ViewModel;
using System.Diagnostics;

namespace Suffer_Travels.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext _db, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = _db;
        }

        public string? SetUserCookie()
        {
            if (HttpContext.Request.Cookies.Any(ck => ck.Key == "Email"))
            {
                return HttpContext.Request.Cookies.FirstOrDefault(ck => ck.Key == "Email").Value.ToString();
            }
            return null;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Home", "User");

            string? email;
            if ((email = SetUserCookie()) != null)
            {
                HttpContext.Session.SetString("Email", email);
                return RedirectToAction("Home", "User");
            }

            TourViewModel tourViewModel = new TourViewModel();

            tourViewModel.tourDetails = db.tblTour;
            tourViewModel.tourTypes = db.tblTourType;
            tourViewModel.tourDates = db.tblTourDates;
            tourViewModel.tourPhotos = db.tblTourPhotos;
            tourViewModel.photos = db.tblPhotos;

            return View(tourViewModel);
        }

        public IActionResult Privacy()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Home", "User");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}