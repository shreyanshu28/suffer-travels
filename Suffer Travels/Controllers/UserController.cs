﻿using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using System.Dynamic;
using System.Net;
using System.Net.Mail;

namespace Suffer_Travels.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private int otp;
        public UserController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HomePage(User user)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Login");

            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            return View();
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Register register)
        {

            IEnumerable<User> _user = db.tblUser;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Homepage");

            if (_user.Any(u => u.Email == register.Email && u.Password == register.Password))
            {
                User user = _user.FirstOrDefault(u => u.Email == register.Email);

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                {
                    HttpContext.Session.SetString("Email", register.Email.ToString().Trim());
                    HttpContext.Session.SetString("Fname", user.Fname.ToString().Trim());
                    //HttpContext.Session.SetInt32("UserId", (int)register.UId);
                }
                return RedirectToAction("HomePage");
            }

            return View();
        }

        public IActionResult Register()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPassword(User user, int? id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendOtp(Register register)
        {
            sendOtp(register.Email, "kushal");

            return Json(new { sendOtp = otp });
        }

        public IActionResult EditProfile()
        {
            IEnumerable<User> _user = db.tblUser;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
                return RedirectToAction("Login");

            IEnumerable<User> u = _user.Where(u => u.Username == HttpContext.Session.GetString("username").ToString());
            //User _user = db.tblUser.Find((uint) HttpContext.Session.GetInt32("userid"));            
            User user = u.First();
            return View(user);
        }

        public IActionResult EditRole (int? id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(User user)
        {
            var _user = db.tblUser.FirstOrDefault(u=> u.UId == user.UId);

            _user.Fname = user.Fname;
            _user.Mname = user.Mname;
            _user.Lname = user.Lname;
            _user.Gender = user.Gender;
            _user.ContactNo = user.ContactNo;
            _user.DateOfBirth = user.DateOfBirth;

            db.SaveChanges();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            IEnumerable<User> _user = db.tblUser;
            if (_user.Any(u => u.ContactNo == user.ContactNo))
                ModelState.AddModelError("ContactNo", "Contact Number is already taken");
            else
            {
                string email = user.Email;
                string fname = user.Fname;
                string lname = user.Lname;
                TempData.Clear();
                TempData.Add("tmpFname", user.Fname);
                TempData.Add("tmpMname", user.Mname);
                TempData.Add("tmpLname", user.Lname);
                TempData.Add("tmpDOB", user.DateOfBirth);
                TempData.Add("tmpGender", user.Gender.ToString());
                TempData.Add("tmpContactNo", user.ContactNo.ToString());

                return RedirectToAction("");
            }
            return View();
        }

        public IActionResult ViewUsers()
        {
            /*if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
                return RedirectToAction("Login");
            */IEnumerable<User> _user = db.tblUser;
            IEnumerable<Role> _role = db.tblRole;
            dynamic UserData = new ExpandoObject();
            UserData.User = _user;
            UserData.Role = _role;
            return View(UserData);
        }

        public string tempDataToString(Object tempData)
        {
            if (tempData != null)
                return tempData.ToString();
            return "";
        }

        public int sendOtp(string toEmail, string username)
        {
            
            string email = "kushal8217@gmail.com", pass = "kushalkushal8217";
            string server = "smtp.gmail.com";

            MailAddress from = new MailAddress("kushal8217@gmail.com", "Kushal Gaiwala");
            MailAddress to = new MailAddress(toEmail, "shreyanshu vyas");
            MailMessage message = new MailMessage(from, to);

            Random rand = new Random();
            otp = rand.Next(111111, 999999);

            string strOtp = @"<span style='font-weight: bold; font-size: 25px;'>"
                                + otp.ToString()
                            + "</span>";

            message.Subject = "Suffer Travels";
            message.IsBodyHtml = true;
            message.Body = @"<h3>Please do not share this otp to anyone</h3>
                            <p>The OTP for verification is</p>
                            <b>" + strOtp + "</b>";

            // Add a carbon copy recipient.
            //MailAddress copy = new MailAddress("Notification_List@contoso.com");
            //message.CC.Add(copy);

            SmtpClient client = new SmtpClient(server);

            // Include credentials if the server requires them.
            client.Port = 587;
            client.Credentials = new NetworkCredential(from.Address, pass);
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
                return 0;
            }
            return otp;
        }
    }
}
