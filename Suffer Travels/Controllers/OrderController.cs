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
            TourViewModel tourViewModel = new TourViewModel();

            tourViewModel.tourDetail = db.tblTour.FirstOrDefault(tour => tour.TId == id);
            tourViewModel.tourTypeDetails = db.tblTourType.FirstOrDefault(tourType => tourType.TtId == tourViewModel.tourDetail.TourTypeId);

            tourViewModel.tourDate = db.tblTourDates.FirstOrDefault(tourDate => tourDate.TourId == id);
            tourViewModel.tourDates = db.tblTourDates.Where(tourDate => tourDate.TourId == id);

            tourViewModel.tourPhoto = db.tblTourPhotos.FirstOrDefault(tourPhoto => tourPhoto.TourId == id);
            tourViewModel.photo = db.tblPhotos.FirstOrDefault(photo => photo.PId == tourViewModel.tourPhoto.PhotoId);

            orderViewModel.TourView = tourViewModel;

            return View(orderViewModel);
        }

        [HttpPost]
        public JsonResult GetTourDates(int TourId)
        {
            TourViewModel tourViewModel = new TourViewModel();

            tourViewModel.tourDates = db.tblTourDates.Where(tourDate => tourDate.TourId == TourId);

            return Json(new {
                data = tourViewModel.tourDates
            });
        }
    }
}
