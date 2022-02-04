using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;
using TeliaMVC.Controllers;

namespace TeliaMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Admin = "admin";
            ViewBag.Client = "client";
            return View();
        }
        [HttpPost]
        public ActionResult Authorise(Admin admin)

        {
            using (TeliaEntities db = new TeliaEntities())
            {
                    var userDetail = db.Clients.Where(x => x.Orgnummer == admin.UserName  && x.Password == admin.Password).FirstOrDefault();
                    if (userDetail == null)
                    {
                        admin.LoginErrorMsg = "Invalid Orgnummer or Password";
                        admin.UserName = "";
                        admin.Password = "";
                        return View("Index", admin);
                    }
                    else
                    {
                        Session["Id"] = userDetail.Id; // mora orgnummer,da bi mogao da ima njegov broj firme
                        return RedirectToAction("Details", "HomeClient",new { userDetail.Id});
                    }
            }

        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.Client = "client";
            return RedirectToAction("Index", "Login");
        }

        public ActionResult LoginAdmin()
        {
            ViewBag.Admin = "admin";
            return View();
        }

        [HttpPost]
        public ActionResult AuthoriseAdmin(Admin admin)
        {
            using (TeliaEntities db = new TeliaEntities())
            {
                var userDetail = db.Admins.Where(x => x.UserName == admin.UserName && x.Password == admin.Password).FirstOrDefault();
                if (userDetail == null)
                {
                    admin.LoginErrorMsg = "Invalid UserName or Password";
                    admin.UserName = "";
                    admin.Password = "";
                    return View("LoginAdmin", admin);
                }
                else
                {
                    Session["Id"] = admin.Id;
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}