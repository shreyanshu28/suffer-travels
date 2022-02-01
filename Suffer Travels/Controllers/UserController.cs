using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;

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
            //Console.WriteLine(user.Fname);
            //ViewData["user"] = user;

            return View();
        }


    }
}
