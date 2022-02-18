﻿using Microsoft.AspNetCore.Mvc;
using Suffer_Travels.Data;

namespace Suffer_Travels.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Home()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (IsAdminUser())
            {
                ViewData["Fname"] = HttpContext.Session.GetString("Fname");
                ViewData["ProfiePhoto"] = HttpContext.Session.GetString("ProfilePhoto");
                return View();
            }

            else
            {
                return RedirectToAction("HomePage", "User");
            }
        }

        public bool IsAdminUser()
        {
            return HttpContext.Session.GetInt32("RoleId") == 1;
        }

        public bool UserLoggedOut()
        {

            return string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        public IActionResult RegisterHotel()
        {
            if (UserLoggedOut())
                return RedirectToAction("Login", "User");

            if (IsAdminUser())
                return View();

            return RedirectToAction("HomePage", "User");
        }
        


    }
}