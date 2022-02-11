using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;
using Suffer_Travels.Models;

namespace Suffer_Travels.Controllers
{
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext db;
        public ChartController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            IEnumerable<User> user = db.tblUser;
            
            ViewData["User"] = user;
            return View(user);
        }
    }
}
