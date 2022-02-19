using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using Suffer_Travels.ViewModel;

namespace Suffer_Travels.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext db;
        public OrderController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult AddOrder(int? id)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Email")))
            {

                ViewData["IsLoggedIn"] = "Not";
            }
            else
            {
                ViewData["IsLoggedin"] = "LoggedIn";
            }

            TourViewModel tourViewModel = new TourViewModel();
            tourViewModel.tourDetail = db.tblTour.FirstOrDefault(tour => tour.TId == id);
            tourViewModel.tourType = db.tblTourType.FirstOrDefault(tour => tour.);
            tourViewModel.tourDate = db.tblTourDates.FirstOrDefault(tour => tour.);
            tourViewModel.tourPhoto = db.tblTourPhotos.FirstOrDefault(tour => tour.);
            tourViewModel.photo = db.tblPhotos.FirstOrDefault(tour => tour.);

            return View(tourViewModel);
        }
    }
}
