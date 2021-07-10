using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TeliaMVC.Models;

namespace TeliaMVC.Controllers
{
    public class HomeClientController : Controller
    {
        private TeliaEntities db = new TeliaEntities();
        public ActionResult Index(int? page)
        {
            var clients = from s in db.Clients
                select s; // vrati sve klijente u jednu listu
            //obavezno order
            clients = clients.OrderBy(s => s.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(clients.ToPagedList(pageNumber,pageSize));
        }
    }
}