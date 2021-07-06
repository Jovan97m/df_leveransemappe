using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;

namespace TeliaMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorise(Admin admin)
        {
            using (TeliaEntities db = new TeliaEntities())
            {
                var userDetail = db.Admins.Where(x => x.UserName == admin.UserName && x.Password == admin.Password).FirstOrDefault();
                if (userDetail == null)
                {
                    admin.LoginErrorMsg = "Invalid UserName or Password";
                    return View("Index", admin);
                }
                else
                {
                    Session["Id"] = admin.Id;
                    return RedirectToAction("Index", "Home");
                }
            }

        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}