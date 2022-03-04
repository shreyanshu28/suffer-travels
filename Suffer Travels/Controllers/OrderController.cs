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
            order.TotalAdults = Convert.ToInt32(HttpContext.Session.GetString("TotalAdults"));
            order.TotalChildrens = Convert.ToInt32(HttpContext.Session.GetString("TotalChildren"));
            order.TotalInfants = Convert.ToInt32(HttpContext.Session.GetString("TotalInfants"));
            order.TotalAmount = Convert.ToInt32(HttpContext.Session.GetString("TotalAmount"));
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
            HttpContext.Session.SetString("TourId", orderViewModel.TourView.tourDetail.TId.ToString());
            HttpContext.Session.SetString("TotalAdults", orderViewModel.order.TotalAdults.ToString());
            HttpContext.Session.SetString("TotalChildren", orderViewModel.order.TotalChildrens.ToString());
            HttpContext.Session.SetString("TotalInfants", orderViewModel.order.TotalInfants.ToString());
            HttpContext.Session.SetString("TotalAmount", orderViewModel.order.TotalAmount.ToString());
            HttpContext.Session.SetString("Date", orderViewModel.order.Date.ToString());
            return View(orderViewModel);
        }

        public IActionResult SetOrderDetails()
        {
            Order order = new Order();
            order.TotalAdults = Convert.ToInt32(HttpContext.Session.GetString("TotalAdults"));
            order.TotalChildrens = Convert.ToInt32(HttpContext.Session.GetInt32("TotalChildren"));
            order.TotalInfants = Convert.ToInt32(HttpContext.Session.GetInt32("TotalInfants"));
            order.TotalAmount = Convert.ToDecimal(HttpContext.Session.GetString("TotalAmount"));
            order.Date = Convert.ToDateTime(HttpContext.Session.GetString("Date"));
            order.UserId = Convert.ToUInt32(db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId);

            db.tblOrderMaster.Add(order);
            db.SaveChanges();
            order = db.tblOrderMaster.FirstOrDefault(order => order.UserId == db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId);

            OrderTour orderTour = new OrderTour();
            orderTour.OrderId = order.OId;
            orderTour.TourId = Convert.ToUInt32(HttpContext.Session.GetInt32("TourId"));
            orderTour.Price = Convert.ToDecimal(HttpContext.Session.GetString("TotalAmount"));
            db.SaveChanges();

            OrderPeople orderPeople = new OrderPeople();
            List<OrderPeople> op = new List<OrderPeople>();
            dynamic peoples = JsonConvert.DeserializeObject(HttpContext.Session.GetString("OrderPeoples"));
            foreach (var item in peoples)
            {
                op.Add(new OrderPeople { 
                    Fname = item["Fname"],
                    Lname = item["Lname"],
                    Proof = item["Proof"],
                    ProofId = item["ProofId"],
                    OrderId = order.OId,
                });
            }
            db.tblOrderPeople.AddRange(op);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetOrderDetails(Order _order)
        {
            Order order = new Order();
            order.TotalAdults = Convert.ToInt32(HttpContext.Session.GetString("TotalAdults"));
            order.TotalChildrens = Convert.ToInt32(HttpContext.Session.GetInt32("TotalChildren"));
            order.TotalInfants = Convert.ToInt32(HttpContext.Session.GetInt32("TotalInfants"));
            order.TotalAmount = Convert.ToInt32(HttpContext.Session.GetInt32("TotalAmount"));
            order.UserId = Convert.ToUInt32(db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId);

            db.tblOrderMaster.Add(order);
            order = db.tblOrderMaster.FirstOrDefault(order => order.UserId == db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId);

            OrderPeople orderPeople = new OrderPeople();
            foreach (dynamic item in HttpContext.Session.GetString("OrderPeoples"))
            {
                orderPeople.Fname = item["Fname"];
                orderPeople.Lname = item["Lname"];
                orderPeople.Proof = item["Proof"];
                orderPeople.ProofId = item["ProofId"];
                orderPeople.OrderId = order.OId;
                db.tblOrderPeople.Add(orderPeople);
            }
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
