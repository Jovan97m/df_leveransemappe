using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;

namespace TeliaMVC.Controllers
{
    public class FakturaoppsettsAdminController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        // GET: Fakturaoppsetts

        //SearchParameter -je atribut koji se prosledjuje iz selektovanog radio-button-a
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,string selected,int? Orgnummer, string SearchParameter)
        {
            ViewBag.CurrentSort = sortOrder;
            //Viewbags- za sortiranja svake kolone;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TilegsSortParm = sortOrder == "Tilegs" ? "tilegs_desc" : "Tilegs"; // mislim da ne bi trebalo da ima ova
            ViewBag.FakturaFormatSortParm = sortOrder == "FakturaFormat" ? "fakturaformat_desc" : "FakturaFormat";
            ViewBag.FakturaadresseSortParm = sortOrder == "Fakturaadresse" ? "fakturaadresse_desc" : "Fakturaadresse";
            ViewBag.HusnrSortParm = sortOrder == "Husnr" ? "husnr_desc" : "Husnr";
            ViewBag.BokstavSortParm = sortOrder == "Bokstav" ? "bokstav_desc" : "Bokstav";
            ViewBag.PostnummerSortParm = sortOrder == "Postnummer" ? "postnummer_desc" : "Postnummer";
            ViewBag.StedSortParm = sortOrder == "Sted" ? "sted_desc" : "Sted";
            //provera za search
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;



            var faktures = from s in db.Fakturaoppsetts
                           select s;

            

            

            //pretrazivanje pre rasporedjivanja:
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (SearchParameter)
                {
                    case "Kostnadsted":
                        faktures = faktures.Where(s => s.NavnPaKostnadssted.Contains(searchString));
                        break;
                    case "Husnr":
                        faktures = faktures.Where(s => s.Husnr.ToString().Contains(searchString));
                        break;
                    case "FakturaFormat":
                        faktures = faktures.Where(s => s.Fakturaformat.Contains(searchString));
                        break;
                }
            }





            switch (sortOrder)
            {
                //prva kolona
                case "name_desc":
                    faktures = faktures.OrderByDescending(s => s.NavnPaKostnadssted);
                    break;
                //druga kolona:
                case "Tilegs":
                    faktures = faktures.OrderBy(s => s.Tileggsinfo_kostnadssted);
                    break;
                case "tilegs_desc":
                    faktures = faktures.OrderByDescending(s => s.Tileggsinfo_kostnadssted);
                    break;

                //treca kolona,fakture
                case "FakturaFormat":
                    faktures = faktures.OrderBy(s => s.Fakturaformat);
                    break;
                case "fakturaformat_desc":
                    faktures = faktures.OrderByDescending(s => s.Fakturaformat);
                    break;

                //cetvrta kolona,adrese za fakture
                case "Fakturaadresse":
                    faktures = faktures.OrderBy(s => s.Fakturaadresse);
                    break;
                case "fakturaadresse_desc":
                    faktures = faktures.OrderByDescending(s => s.Fakturaadresse);
                    break;

                //Peta kolona, Husnr
                case "Husnr":
                    faktures = faktures.OrderBy(s => s.Husnr);
                    break;
                case "husnr_desc":
                    faktures = faktures.OrderByDescending(s => s.Husnr);
                    break;

                //sesta kolona, Bokstav , mozda i ne mora
                case "Bokstav":
                    faktures = faktures.OrderBy(s => s.Bokstav);
                    break;
                case "bokstav_desc":
                    faktures = faktures.OrderByDescending(s => s.Bokstav);
                    break;

                //sedma kolona PostNummer
                case "Postnummer":
                    faktures = faktures.OrderBy(s => s.Postnummer);
                    break;
                case "postnummer_desc":
                    faktures = faktures.OrderByDescending(s => s.Postnummer);
                    break;

                //osma kolona Sted
                case "Sted":
                    faktures = faktures.OrderBy(s => s.Sted);
                    break;
                case "sted_desc":
                    faktures = faktures.OrderByDescending(s => s.Sted);
                    break;

                default:  // Name ascending 
                    faktures = faktures.OrderBy(s => s.NavnPaKostnadssted);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(faktures.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //
        #region CRUD operacije
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fakturaoppsetts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NavnPaKostnadssted,Tileggsinfo_kostnadssted,Fakturaformat,Fakturaadresse,Husnr,Bokstav,Postnummer,Sted,Epost,Kostnadssted")] Fakturaoppsett fakturaoppset)
        {
            if (ModelState.IsValid)
            {
                db.Fakturaoppsetts.Add(fakturaoppset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fakturaoppset);
        }

        // GET: Fakturaoppsetts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Find(id);
            if (fakturaoppsett == null)
            {
                return HttpNotFound();
            }
            return View(fakturaoppsett);
        }

        // POST: Fakturaoppsetts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NavnPaKostnadssted,Tileggsinfo_kostnadssted,Fakturaformat,Fakturaadresse,Husnr,Bokstav,Postnummer,Sted,Epost,Kostnadssted")] Fakturaoppsett fakturaoppsett)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fakturaoppsett).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fakturaoppsett);
        }

        // GET: Fakturaoppsetts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Find(id);
            if (fakturaoppsett == null)
            {
                return HttpNotFound();
            }
            return View(fakturaoppsett);
        }

        public ActionResult Load(int? page)
        {
            //load sve podatke
            var nummers = from s in db.Nummers
                select s;
            //load u selectList
            List<Client> clients = db.Clients.ToList();
            List<String> orgNummers = new List<String>();
            foreach (var item in clients)
            {
                //kad se doda u bazu
                //string final = item.Orgnummer + "-" + item.FirmaNavn;
                orgNummers.Add(item.Orgnummer);
            }
            ViewBag.nummers = orgNummers;

            //orderby da bi radio Paging
            nummers = nummers.OrderBy(s => s.Telefonnummer);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(nummers.ToPagedList(pageNumber, pageSize));
        }

        // POST: Fakturaoppsetts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Find(id);
            db.Fakturaoppsetts.Remove(fakturaoppsett);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Find(id);
            if (fakturaoppsett == null)
            {
                return HttpNotFound();
            }
            return View(fakturaoppsett);
        }
        #endregion

    }
}