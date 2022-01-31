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
        public ActionResult Authorise(Admin admin, Client client,String usertype)

        {
            using (TeliaEntities db = new TeliaEntities())
            {
                if (usertype == "admin")
                {
                    var userDetail = db.Admins.Where(x => x.UserName == admin.UserName && x.Password == admin.Password).FirstOrDefault();
                    if (userDetail == null)
                    {
                        admin.LoginErrorMsg = "Invalid UserName or Password";
                        admin.UserName = "";
                        admin.Password = "";
                        return View("Index", admin);
                    }
                    else
                    {
                        Session["Id"] = admin.Id;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (usertype=="client")
                {
                    var userDetail = db.Clients.Where(x => x.Orgnummer == admin.UserName  && x.Password == admin.Password).FirstOrDefault();
                    if (userDetail == null)
                    {
                        admin.LoginErrorMsg = "Invalid UserName or Password";
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
                else { return View(); }
            }

        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.Admin = "admin";
            ViewBag.Client = "client";
            return RedirectToAction("Index", "Login");
        }
    }
}