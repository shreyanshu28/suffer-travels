using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using System.Net;
using System.Net.Mail;

namespace Suffer_Travels.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        public UserController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            IEnumerable<User> _user = db.tblUser;

            if (_user.Any(u => u.Username == user.Username && u.Password == user.Password))
            {
                return RedirectToAction("HomePage");
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            //db.Add(user);
            //db.SaveChanges();
            TempData.Add("fname", user.Fname);

            return RedirectToAction("VerifyUser");
        }

        public IActionResult VerifyUser()
        {
            ViewData["fname"] = TempData["fname"];
            CreateCopyMessage("smtp.gmail.com");
            //Console.WriteLine(user.Fname);
            //ViewData["user"] = user;

            return View();
        }

        public static void CreateCopyMessage(string server)
        {
            string email = "kushal8217@gmail.com", pass = "kushalkushal8217";
            MailAddress from = new MailAddress("kushal8217@gmail.com", "Kushal Gaiwala");
            MailAddress to = new MailAddress("19bmiit112@gmail.com", "Shreyanshu Vyas");
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Using the SmtpClient class.";
            message.Body = @"Using this feature, you can send an email message from an application very easily.";
            
            // Add a carbon copy recipient.
            //MailAddress copy = new MailAddress("Notification_List@contoso.com");
            //message.CC.Add(copy);

            SmtpClient client = new SmtpClient(server);

            // Include credentials if the server requires them.
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(from.Address, pass);
            client.EnableSsl = true;

            Console.WriteLine("Sending an email message to {0} by using the SMTP host {1}.",
                 to.Address, client.Host);

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateCopyMessage(): {0}",
                    ex.ToString());
            }
        }
    }
}
