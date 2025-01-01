using _23TH2508_Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23TH2508_Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private QuanLyQuanCafeEntities db = new QuanLyQuanCafeEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var admin = db.QuanTris.SingleOrDefault(q => q.Email == email && q.Password == password);
            if (admin != null)
            {
                Session["AdminEmail"] = admin.Email;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Invalid credentials";
            return View();
        }

        public ActionResult Logout()
        {
            Session["AdminEmail"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}