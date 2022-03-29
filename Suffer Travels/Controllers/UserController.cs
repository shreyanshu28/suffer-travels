﻿using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using Suffer_Travels.ViewModel;
using System.Collections;
using System.Dynamic;
using System.Net;
using System.Net.Mail;

namespace Suffer_Travels.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        private static int otp;
        public UserController(ApplicationDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        public IActionResult Index()
        {
            string email;
            if ((email = UserCookieSet()) != null)
            {
                HttpContext.Session.SetString("Email", email);
            }
            return View();
        }

        public void SetSessionValues(string _email)
        {
            User user;
            int roleId;
            string email = _email, fname, profilePhoto;

            user = db.tblUser.First(user => user.Email == email);

            roleId = Convert.ToInt32(user.RoleId);
            fname = user.Fname;
            profilePhoto = user.ProfilePhoto;

            HttpContext.Session.SetInt32("RoleId", roleId);
            HttpContext.Session.SetString("Email", email);
            HttpContext.Session.SetString("Fname", fname);
            HttpContext.Session.SetString("ProfilePhoto", profilePhoto);
        }

        public bool UserLoggedOut()
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        public bool ValidRole(int roleId)
        {
            return HttpContext.Session.GetInt32("RoleId") == roleId;
        }

        public Int32? GetRole()
        {
            return Convert.ToBoolean(HttpContext.Session.GetInt32("RoleId")) ? 
                HttpContext.Session.GetInt32("RoleId") : 0;
        }

        public string UserCookieSet()
        {
            if(HttpContext.Request.Cookies.Any(ck => ck.Key == "Email"))
            {
                return HttpContext.Request.Cookies.FirstOrDefault(ck => ck.Key == "Email").Value.ToString();
            }
            return null;

        }

        public IActionResult Home()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login");

            SetSessionValues(HttpContext.Session.GetString("Email").ToString());

            if (GetRole() == 2)
            {
                TourViewModel tourViewModel = new TourViewModel();
                
                ViewData["Fname"] = HttpContext.Session.GetString("Fname");
                ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto");

                tourViewModel.tourDetails = db.tblTour;
                tourViewModel.tourTypes = db.tblTourType;
                tourViewModel.tourDates = db.tblTourDates;
                tourViewModel.tourPhotos = db.tblTourPhotos;
                tourViewModel.photos = db.tblPhotos;

                return View(tourViewModel);
            }

            return ShowCustomHomePage(GetRole());
        }
        
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Register register)
        {
            IEnumerable<User> _user = db.tblUser;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Home");

            if (_user.Any(u => u.Email == register.Email && u.Password == register.Password))
            {
                User? user = _user.FirstOrDefault(u => u.Email == register.Email);

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                {
                    // Setting Sessions
                    SetSessionValues(register.Email.ToString().Trim());

                    //HttpContext.Session.SetInt32("RoleId", Convert.ToInt32(user.RoleId));
                    //HttpContext.Session.SetString("Email", register.Email.ToString().Trim());
                    //HttpContext.Session.SetString("Fname", user.Fname.ToString().Trim());
                    //HttpContext.Session.SetString("ProfilePhoto", user.ProfilePhoto.ToString().Trim());
                }

                if (register.RememberMe)
                {
                    CookieOptions cookieOption = new CookieOptions();
                    cookieOption.Expires = new DateTimeOffset(DateTime.Now.AddDays(2));
                    HttpContext.Response.Cookies.Append(
                        "Email",
                        register.Email.ToString(),
                        cookieOption
                    );
                }

                return ShowCustomHomePage(HttpContext.Session.GetInt32("RoleId"));
            }

            return View(register);
        }

        public IActionResult ShowCustomHomePage(int? RoleId)
        {
            switch (RoleId)
            {
                case 1:
                    return RedirectToAction("Home", "Admin");

                case 2:
                    return RedirectToAction("Home");

                case 3:
                    return RedirectToAction("Home", "Hotel");

                case 4:
                    return RedirectToAction("Home", "Vehicle");

                default:
                    return RedirectToAction("Login");

            }
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Home");
            return View();
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Home");

            IEnumerable<User> _user = db.tblUser;
            if (_user.Any(u => u.ContactNo == user.ContactNo))
                ModelState.AddModelError("ContactNo", "Contact Number is already taken");

            ModelState.Remove("Email");
            ModelState.Remove("Password");

            if(ModelState.IsValid)
            {
                TempData.Clear();
                TempData.Add("Fname", user.Fname.Trim());
                TempData.Add("Mname", user.Mname.Trim());
                TempData.Add("Lname", user.Lname.Trim());
                TempData.Add("DOB", user.DateOfBirth.ToString().Trim());
                TempData.Add("Gender", user.Gender.ToString().Trim());
                TempData.Add("ContactNo", user.ContactNo.ToString().Trim());

                return RedirectToAction("AddPassword", new
                {
                    id = 3,
                });
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Email");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddPassword(int? id)
        {
            // From forgot password
            if (id == 1)
            {
                HttpContext.Session.SetString("AddPasswordFlag", "1");
            }
            // From change password
            else if (id == 2)
            {
                HttpContext.Session.SetString("AddPasswordFlag", "2");

            }
            // From registration form
            else if (id == 3)
            {
                HttpContext.Session.SetString("AddPasswordFlag", "3");
            }
            // From nowhere
            else
            {
                HttpContext.Session.SetString("AddPasswordFlag", "0");
                return RedirectToAction("Register");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPassword(Register register)
        {
            User user;
            int passFlag = Convert.ToInt32(HttpContext.Session.GetString("AddPasswordFlag"));
            // From forgot password
            if(passFlag == 1)
            {
                if (!db.tblUser.Any(user => user.Email == HttpContext.Session.GetString("Email")))
                {
                    ModelState.AddModelError("Email", "The user associated with this email address doesn't exists");
                }
            }
            // From change password
            else if (passFlag == 2)
            {
                ModelState.Remove("Email");
            }
            // From registration
            else if (passFlag == 3)
            {
                if(db.tblUser.Any(user => user.Email == register.Email))
                {
                    ModelState.AddModelError("Emai", "User associated with this email address alreasy exists");
                }
            }
            else
            {
                return RedirectToAction("Registration");
            }

            if (Convert.ToInt32(register.Otp) == otp)
            {
                if (register.Password != register.RePassword)
                {
                    ModelState.AddModelError("Password", "The passwords do not match");
                }

                if(ModelState.IsValid)
                {
                    UInt32 RoleId;
                    user = new User();

                    user.Fname = TempData["Fname"].ToString();
                    user.Mname = TempData["Mname"].ToString();
                    user.Lname = TempData["Lname"].ToString();
                    user.Gender = Char.Parse(TempData["Gender"].ToString());
                    user.DateOfBirth = DateTime.Parse(TempData["Dob"].ToString());
                    user.ContactNo = Convert.ToInt64(TempData["ContactNo"].ToString());
                    user.Email = register.Email;
                    user.Password = register.Password;

                    RoleId = TempData.ContainsKey("RoleId") ? Convert.ToUInt32(TempData["RoleId"].ToString()) : 2;
                    user.RoleId = RoleId;

                    user.Status = RoleId != 2 ? "" : "Approved";

                    db.tblUser.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult SendOtp(Register register)
        {
            string message = "Otp is successfully sent";
            if(HttpContext.Session.GetString("AddPasswordFlag") == "1")
            {
                if (db.tblUser.Any(user => user.Email == register.Email))
                    sendOtp(register.Email, register.Email);
                else
                    message = "The email address you entered does not exists";
            }
            else if (db.tblUser.Any(user => user.Email == register.Email))
            {
                message = "The email address you entered already exists";
                otp = 0;
            }
            else if (register.Email == null)
            {
                message = "Please enter a email address";
                otp = 0;
            }
            else
            {
                sendOtp(register.Email, register.Email);
            }

            if (otp != 0)
                return Json(new
                {
                    sendOtp = otp,
                    status = 1,
                    message = message
                });
            
            return Json(new { 
                sendOtp = otp, 
                status = 0, 
                message = message 
            });
        }

        public IActionResult EditProfile()
        {
            IEnumerable<User> _user = db.tblUser;

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Login");

            IEnumerable<User> u = _user.Where(u => u.Email == HttpContext.Session.GetString("Email").ToString());
            //User _user = db.tblUser.Find((uint) HttpContext.Session.GetInt32("userid"));
            int? roleId = HttpContext.Session.GetInt32("Role");
            User user = u.First();
            return View(user);
        }

        public IActionResult EditRole(int? id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfileAsync(User user)
        {
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfiePhoto"] = HttpContext.Session.GetString("ProfilePhoto");

            var _user = db.tblUser.FirstOrDefault(u => u.UId == user.UId);

            var files = HttpContext.Request.Form.Files;

            foreach (var Image in files)
            {
                string[] Images = Image.FileName.Split(".");
                string extension = Images[Images.Length - 1].ToLower();
                if (extension != "jpg" && extension != "png")
                {
                    ModelState.AddModelError("ProfilePhoto", "Only jpg and png image formates are supported!");
                    return View(_user);
                }
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;

                    var uploads = Path.Combine(env.WebRootPath, "photos\\user");
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            _user.ProfilePhoto = fileName;
                        }
                    }
                }
            }

            _user.Fname = user.Fname;
            _user.Mname = user.Mname;
            _user.Lname = user.Lname;
            _user.Gender = user.Gender;
            _user.ContactNo = user.ContactNo;
            _user.DateOfBirth = user.DateOfBirth;

            db.SaveChanges();

            return View(_user);
        }

        public IActionResult RegisterPartner()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterPartner(User user)
        {
            IEnumerable<User> _user = db.tblUser;
            if (_user.Any(u => u.ContactNo == user.ContactNo))
            {
                ModelState.AddModelError("ContactNo", "Contact Number is already taken");
            }

            ModelState.Remove("Email");
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                TempData.Clear();
                TempData.Add("Fname", user.Fname.Trim());
                TempData.Add("Mname", user.Mname.Trim());
                TempData.Add("Lname", user.Lname.Trim());
                TempData.Add("DOB", user.DateOfBirth.ToString().Trim());
                TempData.Add("Gender", user.Gender.ToString().Trim());
                TempData.Add("ContactNo", user.ContactNo.ToString().Trim());
                TempData.Add("RoleId", user.RoleId.ToString().Trim());

                return RedirectToAction("AddPassword");
            }

            return View();
        }
        
        

        public int sendOtp(string toEmail, string username)
        {
            string email = "suffertravelco@gmail.com", pass = "tavabiryani";
            string server = "smtp.gmail.com";

            MailAddress from = new MailAddress(email, "Suffer Travels");
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
                otp = 0;
                return 0;
            }
            return otp;
        }
    }
}
