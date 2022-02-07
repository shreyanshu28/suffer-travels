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
            TempData.Add("tmpFname", user.Fname);
            TempData.Add("tmpMname", user.Mname);
            TempData.Add("tmpLname", user.Lname);
            TempData.Add("tmpDOB", user.DateOfBirth);
            TempData.Add("tmpContactNo", user.ContactNo.ToString());
            TempData.Add("tmpEmail", user.Email);

            return RedirectToAction("VerifyUser");
        }

        public string tempDataToString(Object tempData)
        {
            if(tempData != null)
                return tempData.ToString();
            return "";
        }

        public IActionResult VerifyUser()
        {
            int otp = 0;

            string fname = "", mname = "", lname = "", gender = "", dob = "", contactNo = "", email = "";
            
            fname = tempDataToString(TempData["tmpFname"]);
            mname = tempDataToString(TempData["tmpMname"]);
            lname = tempDataToString(TempData["tmpLname"]);
            gender = tempDataToString(TempData["tmpGender"]);
            dob = tempDataToString(TempData["tmpDOB"]);
            contactNo = tempDataToString(TempData["tmpContactNo"]);
            email = tempDataToString(TempData["tmpEmail"]);

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

            otp = sendOtp(email);

            TempData.Clear();
            return View();
        }

        public static int sendOtp( string toEmail)
        {
            int otp = 0;
            string email = "kushal8217@gmail.com", pass = "kushalkushal8217";
            string server = "smtp.gmail.com";

            MailAddress from = new MailAddress("kushal8217@gmail.com", "Kushal Gaiwala");
            MailAddress to = new MailAddress(toEmail, "Shreyanshu Vyas");
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
