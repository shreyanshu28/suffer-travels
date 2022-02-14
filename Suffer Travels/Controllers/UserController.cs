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
        private static int otp;
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

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
                return RedirectToAction("Homepage");

            if (_user.Any(u => u.Username == user.Username && u.Password == user.Password))
            {
                //BIG ERROR CHANCE
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
                {
                    HttpContext.Session.SetString("username", user.Username.ToString().Trim());
                    HttpContext.Session.SetInt32("userid", (int)user.UId);
                }
                return RedirectToAction("HomePage");
            }

            return View();
        }

        public IActionResult HomePage(User user)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
                return RedirectToAction("Login");

            ViewData["username"] = HttpContext.Session.GetString("username");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            //PARAM: Action, Controller
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(Register register)
        {
            IEnumerable<User> user = db.tblUser;
            if (user.Any(u => u.Email == register.email))
            {
                ModelState.AddModelError("email", "Username is already taken");
            }

            if (register.password != register.rePassword)
            {
                ModelState.AddModelError("password", "Password do not match");
            }
            string email = "";
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString("forgotEmail")))
                email = HttpContext.Session.GetString("forgotEmail");

            var _user = db.tblUser.FirstOrDefault(u => u.Email == email);

            _user.Password = register.password;
            
            db.SaveChanges();

            return RedirectToAction("Login");
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyEmail(User user, int? id)
        {
            IEnumerable<User> _user = db.tblUser;
            /* if (!_user.Any(u => u.Email == user.Email))
             {
                 ModelState.AddModelError("Email", "Email does not exist. Please register");
             }*/

            if (id != 1)
            {
                if (user.Email != null)
                {
                    if (!_user.Any(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "Email does not exist. Please register");
                        return View(user);
                        //   return RedirectToAction("LogIn");
                    }
                    else
                    {
                        HttpContext.Session.SetString("forgotEmail", user.Email);
                        otp = sendOtp(user.Email, "Forgot Password");
                        return RedirectToAction("VerifyUser");
                    }
                }
            }

            return View();
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

        public string tempDataToString(Object tempData)
        {
            if (tempData != null)
                return tempData.ToString();
            return "";
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyUser(OTP oneTimePass)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("forgotEmail")))
                    if (otp == Convert.ToInt32(oneTimePass.otp))
                        return RedirectToAction("ForgotPassword");
    
                if (otp == Convert.ToInt32(oneTimePass.otp))
                    return RedirectToAction("ChangePassword");
            
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(Register register)
        {
            IEnumerable<User> user = db.tblUser;
            if (user.Any(u => u.Email == register.email))
            {
                ModelState.AddModelError("Username", "Username is already taken");
            }

            if (register.password != register.rePassword)
            {
                ModelState.AddModelError("Password", "Password do not match");
            }
            if (ModelState.IsValid)
            {
                String fname = tempDataToString(TempData["tmpFname"]);
                String mname = tempDataToString(TempData["tmpMname"]);
                String lname = tempDataToString(TempData["tmpLname"]);
                String dob = tempDataToString(TempData["tmpDOB"]);
                String gender = tempDataToString(TempData["tmpGender"]);
                String contactNo = tempDataToString(TempData["tmpContactNo"]);
                String email = tempDataToString(TempData["tmpEmail"]);

                User u = new User();
                u.Fname = fname;
                u.Lname = lname;
                u.Mname = mname;
                u.Gender = Char.Parse(gender);
                u.DateOfBirth = DateTime.Parse(dob);
                u.Email = email;
                u.ContactNo = Convert.ToInt64(contactNo);
                u.Email = register.email;
                u.Password = register.password;
                db.tblUser.Add(u);
                db.SaveChanges();

                return RedirectToAction("HomePage");
            }
            return View();
        }

        public static int sendOtp(string toEmail, string username)
        {
            
            string email = "kushal8217@gmail.com", pass = "kushalkushal8217";
            string server = "smtp.gmail.com";

            MailAddress from = new MailAddress("kushal8217@gmail.com", "Kushal Gaiwala");
            MailAddress to = new MailAddress(toEmail, username);
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

        public IActionResult VerifyUser()
        {
            //if (TempData.ContainsKey("tmpFname"))
            //    fname = TempData["tmpFname"].ToString();

            //if (TempData.ContainsKey("tmpMname"))
            //    mname = TempData["tmpMname"].ToString();

            //if (TempData.ContainsKey("tmpLname"))
            //    lname = TempData["tmpLname"].ToString();

            //if (TempData.ContainsKey("tmpGender"))
            //    gender = TempData["tmpGender"].ToString();

            //if (TempData.ContainsKey("tmpDOB"))
            //    dob = TempData["tmpDOB"].ToString(); 

            //if (TempData.ContainsKey("tmpContactNo"))
            //    contactNo = TempData["tmpContactNo"].ToString(); 

            //if (TempData.ContainsKey("tmpEmail"))
            //    email = TempData["tmpEmail"].ToString();

            //TempData.Clear();
            return View();
        }
    }
}
