using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
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
        private readonly string wwwroot;
        private int otp;
        public UserController(ApplicationDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
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

        public IActionResult AdminHomePage(User user)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Login");

            if (HttpContext.Session.GetInt32("Role") == 1)
            {
                ViewData["Fname"] = HttpContext.Session.GetString("Fname");
                return View();
            }

            else
            {
                return RedirectToAction("HomePage");
            }
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
                    HttpContext.Session.SetInt32("Role", Convert.ToInt32(user.RoleId));
                }

                int? RoleId = HttpContext.Session.GetInt32("Role");

                if (RoleId == 1)
                {
                    return RedirectToAction("AdminHomePage");
                }
                else if (RoleId == 2)
                {
                    return RedirectToAction("HomePage");
                }
                else if (RoleId == 3)
                {
                    return RedirectToAction("HomePage");
                }
                else if (RoleId == 4)
                {
                    return RedirectToAction("HomePage");
                }
                else
                {
                    return NotFound();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            IEnumerable<User> _user = db.tblUser;
            if (_user.Any(u => u.ContactNo == user.ContactNo))
                ModelState.AddModelError("ContactNo", "Contact Number is already taken");
            else
            {
                TempData.Clear();
                TempData.Add("Fname", user.Fname.Trim());
                TempData.Add("Mname", user.Mname.Trim());
                TempData.Add("Lname", user.Lname.Trim());
                TempData.Add("DOB", user.DateOfBirth.ToString().Trim());
                TempData.Add("Gender", user.Gender.ToString().Trim());
                TempData.Add("ContactNo", user.ContactNo.ToString().Trim());

                return RedirectToAction("AddPassword");
            }
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
        public IActionResult AddPassword(Register register)
        {
            User user;
            if (Convert.ToInt32(register.Otp) == otp)
            {
                if (register.Password != register.RePassword)
                {
                    ModelState.AddModelError("Password", "The passwords do not match");
                }
                else
                {
                    user = new User();

                    user.Fname = TempData["Fname"].ToString();
                    user.Mname = TempData["Mname"].ToString();
                    user.Lname = TempData["Lname"].ToString();
                    user.Gender = Char.Parse(TempData["Gender"].ToString());
                    user.DateOfBirth = DateTime.Parse(TempData["Dob"].ToString());
                    user.ContactNo = Convert.ToInt64(TempData["ContactNo"].ToString());
                    user.Email = register.Email;
                    user.Password = register.Password;

                    UInt32 RoleId = TempData.ContainsKey("RoleId") ? Convert.ToUInt32(TempData["RoleId"].ToString()) : 2;
                    user.RoleId = RoleId;

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
            sendOtp(register.Email, register.Email);
            if (otp != 0)
                return Json(new { sendOtp = otp, status = 1 });
            return Json(new { sendOtp = otp, status = 0 });
        }

        public IActionResult EditProfile()
        {
            IEnumerable<User> _user = db.tblUser;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Login");

            IEnumerable<User> u = _user.Where(u => u.Email == HttpContext.Session.GetString("Email").ToString());
            //User _user = db.tblUser.Find((uint) HttpContext.Session.GetInt32("userid"));
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
            var _user = db.tblUser.FirstOrDefault(u => u.UId == user.UId);

            //if (files != null)
            //{
            //    if (files.Length > 0)
            //    {
            //        //Getting FileName
            //        var fileName = Path.GetFileName(files.FileName);
            //        //Getting file Extension
            //        var fileExtension = Path.GetExtension(fileName);
            //        // concatenating  FileName + FileExtension
            //        var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            //        string objFiless = newFileName;

            //        //var objfiles = new Files()
            //        //{
            //        //    DocumentId = 0,
            //        //    Name = newFileName,
            //        //    FileType = fileExtension,
            //        //    CreatedOn = DateTime.Now
            //        //};

            //        using (var target = new MemoryStream())
            //        {
            //            files.CopyTo(target);
            //        }


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

        public IActionResult ViewUsers()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                return RedirectToAction("Login");
            IEnumerable<User> _user = db.tblUser;
            return View(_user);
        }

        public IActionResult ApproveRole(uint? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ViewUsers");
            }
            var _user = db.tblUser.Find(id);
            _user.Status = ApproveStatus();
            _user.ChangedAt = DateTime.Today;
            db.tblUser.Update(_user);
            db.SaveChanges();
            TempData["Success"] = "Request Approved";
            sendRoleNotification(_user.Email.ToString(), HttpContext.Session.GetString("Fname"), ApproveStatus());
            return RedirectToAction("ViewUsers");
        }

        public IActionResult DeclineRole(uint? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "No matching results found";
                return RedirectToAction("ViewUsers");
            }

            var _user = db.tblUser.Find(id);
            _user.Status = DeclineStatus();
            _user.ChangedAt = DateTime.Today;
            db.tblUser.Update(_user);
            db.SaveChanges();
            TempData["Success"] = "Request Declined";
            sendRoleNotification(_user.Email.ToString(), HttpContext.Session.GetString("Fname"), DeclineStatus());
            return RedirectToAction("ViewUsers");
        }

        public String ApproveStatus()
        {
            return "Approved";
        }

        public String DeclineStatus()
        {
            return "Declined";
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

        public string tempDataToString(Object tempData)
        {
            if (tempData != null)
                return tempData.ToString();
            return "";
        }

        public int sendRoleNotification(string toEmail, string username, string status)
        {
            string email = "kushal8217@gmail.com", pass = "kushalkushal8217";
            //string email = "suffertravelco@gmail.com", pass = "tavabiryani";
            string server = "smtp.gmail.com";
            string approval_message = @"<span style='font-weight: bold; font-size: 25px; '>  Your request has been approved </span>";
            string decline_message = @"<span style='font-weight: bold; font-size: 25px; '>  Your request has been declined. You can try again. </span>";

            MailAddress from = new MailAddress(email, "Suffer Travels");
            MailAddress to = new MailAddress(toEmail, "shreyanshu vyas");
            MailMessage message = new MailMessage(from, to);

            if(status.Equals(ApproveStatus()))
            {
                message.Subject = "Congratulations! Your request has been approved!";
                message.Body = approval_message;

            }
            else if(status.Equals(DeclineStatus()))
            {
                message.Subject = "Oh no! Your request is declined";
                message.Body = decline_message;
            }

            message.IsBodyHtml = true;

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
            return 1;
        }

        public int sendOtp(string toEmail, string username)
        {

            string email = "kushal8217@gmail.com", pass = "kushalkushal8217";
            //string email = "suffertravelco@gmail.com", pass = "tavabiryani";
            string server = "smtp.gmail.com";

            MailAddress from = new MailAddress(email, "Suffer Travels");
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
                otp = 0;
                return 0;
            }
            return otp;
        }
    }
}
