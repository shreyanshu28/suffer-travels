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

            OrderViewModel orderViewModel = new OrderViewModel();

            orderViewModel.tourDetail = db.tblTour.FirstOrDefault(tour => tour.TId == id);
            orderViewModel.tourTypeDetails = db.tblTourType.FirstOrDefault(tourType => tourType.TtId == orderViewModel.tourDetail.TourTypeId);

            orderViewModel.tourDate = db.tblTourDates.FirstOrDefault(tourDate => tourDate.TourId == id);

            orderViewModel.tourPhoto = db.tblTourPhotos.FirstOrDefault(tourPhoto => tourPhoto.TourId == id);
            orderViewModel.photo = db.tblPhotos.FirstOrDefault(photo => photo.PId == orderViewModel.tourPhoto.PhotoId);

            return View(orderViewModel);
        }
    }
}
