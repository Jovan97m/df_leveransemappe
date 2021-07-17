using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using IdentityModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using PagedList;
using TeliaMVC.Models;

namespace TeliaMVC.Controllers
{
    public class HomeClientController : Controller
    {
        private TeliaEntities db = new TeliaEntities();
        public static string orgID = "";

        public ActionResult Index(string OrgNummer)
        {
            orgID = OrgNummer;
            return View();
        }


        //Profile:
        public ActionResult Profile()
        {
            var client = db.Clients.Where(s => s.Orgnummer.Contains(orgID)); 

            return View(client);
        }
        
    }
}