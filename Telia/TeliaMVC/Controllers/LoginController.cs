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
        public ActionResult Authorise(Admin admin, Client client,String usertype)

        {
            using (TeliaEntities db = new TeliaEntities())
            {
                if (usertype == "Admin")
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
                else if (usertype=="Client")
                {
                    var userDetail = db.Clients.Where(x => x.Orgnummer == admin.UserName  && x.Password == admin.Password).FirstOrDefault();
                    if (userDetail == null)
                    {
                        admin.LoginErrorMsg = "Invalid UserName or Password";
                        return View("Index", client);
                    }
                    else
                    {
                        Session["Id"] = client.Id;
                        return RedirectToAction("Index", "HomeClient");
                    }
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