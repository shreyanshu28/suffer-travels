using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;
using System.Linq;

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

        public IActionResult Login(User _user)
        {
            IEnumerable<User> user = db.tblUser;

            if(user.Any(u => u.Username == _user.Username && u.Password == _user.Password))
            {
                return View(user);
            }

            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }
    }
}
