using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using Suffer_Travels.Service;

namespace Suffer_Travels.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _service;
        private IHttpContextAccessor _httpContextAccessor;
        public PaymentController(ApplicationDbContext _db, ILogger<PaymentController> logger, IPaymentService service, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            _logger = logger;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetViewData()
        {
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto");
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["TotalAmount"] = HttpContext.Session.GetString("TotalAmount");
        }

        public IActionResult Index()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProcessRequestOrder(PaymentRequest _paymentRequest)
        {
            _paymentRequest.Amount = Convert.ToDecimal(HttpContext.Session.GetString("TotalAmount").ToString());
            MerchantOrder _marchantOrder = await _service.ProcessMerchantOrder(_paymentRequest);
            SetViewData();
            return View("Payment", _marchantOrder);
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrderProcess()
        {
            string PaymentMessage = await _service.CompleteOrderProcess(_httpContextAccessor);
            SetViewData();
            if (PaymentMessage == "captured")
            {
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }
        public IActionResult Success()
        {
            Order order = db.tblOrderMaster.Where(order => order.UserId == db.tblUser.FirstOrDefault(user => user.Email == HttpContext.Session.GetString("Email")).UId).OrderBy(or => or.OId).LastOrDefault();
            order.Payment = "Completed";
            db.SaveChanges();
            SetViewData();
            TempData["Success"] = "Payment is successful";
            return RedirectToAction("Home", "User");
        }

        public IActionResult Failed()
        {
            SetViewData();
            return View();
        }
    }
}
