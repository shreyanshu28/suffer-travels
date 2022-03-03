using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
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
        [ValidateAntiForgeryToken]
        public IActionResult AddOrder(Order order)
        {
            return View(order);
        }

        public IActionResult GetOrderDetails()
        {
            Order order = new Order();
            //order.UserId = db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId;
            order.TotalAdults = Convert.ToInt32(HttpContext.Session.GetString("adults"));
            order.TotalChildrens = Convert.ToInt32(HttpContext.Session.GetString("childs"));
            order.TotalInfants = Convert.ToInt32(HttpContext.Session.GetString("infants"));
            order.TotalAmount = Convert.ToInt32(HttpContext.Session.GetString("total"));
            return View(order);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetOrderDetails(Order order)
        {
            db.tblOrderMaster.Add(order);
            db.SaveChanges();
            order.UserId = db.tblUser.Where(user => user.Email == HttpContext.Session.GetString("Email")).FirstOrDefault().UId;
            order.TotalAdults = Convert.ToInt32(HttpContext.Session.GetString("adults"));
            order.TotalChildrens = Convert.ToInt32(HttpContext.Session.GetString("childs"));
            order.TotalInfants = Convert.ToInt32(HttpContext.Session.GetString("infants"));
            order.TotalAmount = Convert.ToInt32(HttpContext.Session.GetString("total"));
            return View(order);
        }*/

        [HttpPost]
        public IActionResult SaveGuestsDetails(string peoples)
        {
            HttpContext.Session.SetString("OrderPeoples", peoples);
            return View();
        }

        [HttpPost]
        public IActionResult GetOrderDetails(OrderViewModel orderViewModel)
        {
            HttpContext.Session.SetString("tourId", orderViewModel.TourView.tourDetail.TId.ToString());
            HttpContext.Session.SetString("adults", orderViewModel.order.TotalAdults.ToString());
            HttpContext.Session.SetString("childs", orderViewModel.order.TotalChildrens.ToString());
            HttpContext.Session.SetString("infants", orderViewModel.order.TotalInfants.ToString());
            HttpContext.Session.SetString("total", orderViewModel.order.TotalAmount.ToString());
            return View(orderViewModel);
        }

        public IActionResult SetOrderDetails()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetOrderDetails(OrderViewModel orderViewModel)
        {
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
