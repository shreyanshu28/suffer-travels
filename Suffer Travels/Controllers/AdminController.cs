using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using System.Net;
using System.Net.Mail;

namespace Suffer_Travels.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        public AdminController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult AddTours()
        {
            if(UserLoggedOut())
            {
                return RedirectToAction("Login", "User");
            }
            
            if(IsAdminUser())
            {
                return View();
            }

            return RedirectToAction("Home", "User");
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
            SendRoleNotification(_user.Email.ToString(), HttpContext.Session.GetString("Fname"), ApproveStatus());
            return RedirectToAction("ViewUsers");
        }

        public String ApproveStatus()
        {
            return "Approved";
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
            SendRoleNotification(_user.Email.ToString(), HttpContext.Session.GetString("Fname"), DeclineStatus());
            return RedirectToAction("ViewUsers");
        }

        public String DeclineStatus()
        {
            return "Declined";
        }

        public IActionResult Home()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (IsAdminUser())
            {
                ViewData["Fname"] = HttpContext.Session.GetString("Fname");
                ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto");
                return View();
            }
            else
            {
                return RedirectToAction("Home", "User");
            }
        }

        public bool IsAdminUser()
        {
            return HttpContext.Session.GetInt32("RoleId") == 1;
        }

        public int SendRoleNotification(string toEmail, string username, string status)
        {
            string email = "suffertravelco@gmail.com", pass = "tavabiryani";
            string server = "smtp.gmail.com";
            string approval_message = @"<span style='font-weight: bold; font-size: 25px; '>  Your request has been approved </span>";
            string decline_message = @"<span style='font-weight: bold; font-size: 25px; '>  Your request has been declined. You can try again. </span>";

            MailAddress from = new MailAddress(email, "Suffer Travels");
            MailAddress to = new MailAddress(toEmail, "shreyanshu vyas");
            MailMessage message = new MailMessage(from, to);

            if (status.Equals(ApproveStatus()))
            {
                message.Subject = "Congratulations! Your request has been approved!";
                message.Body = approval_message;

            }
            else if (status.Equals(DeclineStatus()))
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
        public bool UserLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        public IActionResult ViewUsers()
        {
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            ViewData["ProfiePhoto"] = HttpContext.Session.GetString("ProfilePhoto");

            if (UserLoggedOut())
                return RedirectToAction("Login", "User");
            IEnumerable<User> _user = db.tblUser;
            ViewData["Fname"] = HttpContext.Session.GetString("Fname");
            return View(_user);
        }
    }
}
