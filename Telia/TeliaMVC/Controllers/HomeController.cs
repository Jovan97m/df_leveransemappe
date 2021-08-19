using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;
using PagedList;

namespace TeliaMVC.Controllers
{
    public class HomeController : Controller
    {
        private TeliaEntities db = new TeliaEntities(); // treba pristup bazi
        public ActionResult Index(int? page)
        {
            var clients = from s in db.Clients
                           select s; // vrati sve klijente u jednu listu
            //obavezno order
            clients= clients.OrderBy(s => s.Id);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(clients.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //DODAVANJE NOVOG KLIJENTA:
        public ActionResult Create()
        {
            ViewBag.Abonementypes = FillSelectBoxClients();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Orgnummer ,Password,Id_admin,Id_abonementype")] Client client,string selected)
        {
            int id = GetAbonementypeId(selected);
            if (id == 0)
            {
                return View(client);
            }
            else
            {
                client.Id_abonementype = id;
                if (ModelState.IsValid)
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index"); // refresh stranicu opet
                }
            }
            return View(client);
        }

        //mislim da mora
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #region pomocne
        public List<String> FillSelectBoxClients() // selectbox za abonementypes
        {
            List<String> names = new List<String>();
            foreach (var item in db.Abonementypes.ToList())
            {
                names.Add(item.Name);
            }
            return names;
        }

        public int GetAbonementypeId(string name)
        {
            var c = db.Abonementypes.Where(s => s.Name.Contains(name));
            if (c == null)
            {
                return 0;
            }
            else
                return c.FirstOrDefault().Id;
        }
        #endregion
    }
}