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

        public IActionResult OrderDetail(int? id)
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
        public IActionResult OrderDetail(OrderViewModel orderViewModel)
        {
            DateTime startDate = orderViewModel.order.Date;
            int TId = Convert.ToInt32(orderViewModel.TourView.tourDetail.TId);

            if (startDate < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Start date is not available");

                TourViewModel tourViewModel = new TourViewModel();

                tourViewModel.tourDetail = db.tblTour.FirstOrDefault(tour => tour.TId == TId);
                tourViewModel.tourTypeDetails = db.tblTourType.FirstOrDefault(tourType => tourType.TtId == tourViewModel.tourDetail.TourTypeId);

                tourViewModel.tourDate = db.tblTourDates.FirstOrDefault(tourDate => tourDate.TourId == TId);
                tourViewModel.tourDates = db.tblTourDates.Where(tourDate => tourDate.TourId == TId);

                tourViewModel.tourPhoto = db.tblTourPhotos.FirstOrDefault(tourPhoto => tourPhoto.TourId == TId);
                tourViewModel.photo = db.tblPhotos.FirstOrDefault(photo => photo.PId == tourViewModel.tourPhoto.PhotoId);

                orderViewModel.TourView = tourViewModel;

                return View(orderViewModel);
            }

            if (db.tblTourDates.Any(date => date.Date == startDate))
            {
                HttpContext.Session.SetString("TourId", orderViewModel.TourView.tourDetail.TId.ToString());
                HttpContext.Session.SetString("TotalAdults", orderViewModel.order.TotalAdults.ToString());
                HttpContext.Session.SetString("TotalChildren", orderViewModel.order.TotalChildrens.ToString());
                HttpContext.Session.SetString("TotalInfants", orderViewModel.order.TotalInfants.ToString());
                HttpContext.Session.SetString("TotalAmount", orderViewModel.order.Total.ToString());
                HttpContext.Session.SetString("Date", orderViewModel.order.Date.ToString());
                HttpContext.Session.SetString("EndDate", orderViewModel.order.EndDate.ToString());
                return View("GetGuestsDetails", orderViewModel);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetGuestsDetails(OrderViewModel orderViewModel)
        {
            DateTime startDate = orderViewModel.order.Date;
            int TId = Convert.ToInt32(orderViewModel.TourView.tourDetail.TId);

            if (startDate < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Start date is not available");

                TourViewModel tourViewModel = new TourViewModel();

                tourViewModel.tourDetail = db.tblTour.FirstOrDefault(tour => tour.TId == TId);
                tourViewModel.tourTypeDetails = db.tblTourType.FirstOrDefault(tourType => tourType.TtId == tourViewModel.tourDetail.TourTypeId);

                tourViewModel.tourDate = db.tblTourDates.FirstOrDefault(tourDate => tourDate.TourId == TId);
                tourViewModel.tourDates = db.tblTourDates.Where(tourDate => tourDate.TourId == TId);

                tourViewModel.tourPhoto = db.tblTourPhotos.FirstOrDefault(tourPhoto => tourPhoto.TourId == TId);
                tourViewModel.photo = db.tblPhotos.FirstOrDefault(photo => photo.PId == tourViewModel.tourPhoto.PhotoId);

                orderViewModel.TourView = tourViewModel;

                return View(orderViewModel);
            }

            if (db.tblTourDates.Any(date => date.Date == startDate))
            {
                HttpContext.Session.SetString("TourId", orderViewModel.TourView.tourDetail.TId.ToString());
                HttpContext.Session.SetString("TotalAdults", orderViewModel.order.TotalAdults.ToString());
                HttpContext.Session.SetString("TotalChildren", orderViewModel.order.TotalChildrens.ToString());
                HttpContext.Session.SetString("TotalInfants", orderViewModel.order.TotalInfants.ToString());
                HttpContext.Session.SetString("TotalAmount", orderViewModel.order.Total.ToString());
                HttpContext.Session.SetString("Date", orderViewModel.order.Date.ToString());
                HttpContext.Session.SetString("EndDate", orderViewModel.order.EndDate.ToString());
                return View("GetGuestsDetails", orderViewModel);
            }
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetOrderDetails(OrderViewModel orderViewModel)
        {
            //Order order = new Order();
            //order.TotalAdults = Convert.ToInt32(HttpContext.Session.GetString("TotalAdults"));
            //order.TotalChildrens = Convert.ToInt32(HttpContext.Session.GetString("TotalChildren"));
            //order.TotalInfants = Convert.ToInt32(HttpContext.Session.GetString("TotalInfants"));
            //order.Total = Convert.ToDecimal(HttpContext.Session.GetString("TotalAmount"));
            //order.Date = Convert.ToDateTime(HttpContext.Session.GetString("Date"));
            orderViewModel.order.UserId = Convert.ToUInt32(db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId);
            var dbTrans = db.Database.BeginTransaction();
            db.tblOrderMaster.Add(orderViewModel.order);
            db.SaveChanges();
            Order order = db.tblOrderMaster.Where(order => order.UserId == db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId).OrderBy(or => or.OId).LastOrDefault();

            OrderTour orderTour = new OrderTour();
            orderTour.OrderId = order.OId;
            orderTour.TourId = orderViewModel.TourView.tourDetail.TId;
            //orderTour.Price = Convert.ToDecimal(HttpContext.Session.GetString("TotalAmount"));
            orderTour.Price = orderViewModel.order.Total;
            db.tblOrderTour.Add(orderTour);
            db.SaveChanges();

            OrderPeople orderPeople = new OrderPeople();
            List<OrderPeople> op = new List<OrderPeople>();
            IEnumerable<OrderPeople> peoples = orderViewModel.orderPeoples;
            foreach (var item in peoples)
            {
                op.Add(new OrderPeople { 
                    Fname = item.Fname,
                    Lname = item.Lname,
                    Proof = item.Proof,
                    ProofId = item.ProofId,
                    OrderId = order.OId,
                });
            }
            db.tblOrderPeople.AddRange(op);
            db.SaveChanges();
            HttpContext.Session.SetString("TotalAmount", orderViewModel.order.Total.ToString());
            return RedirectToAction("Index", "Payment");
        }

        public IActionResult Payment()
        {
            ViewData["Amount"] = HttpContext.Session.GetString("TotalAmount");
            return View();
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
