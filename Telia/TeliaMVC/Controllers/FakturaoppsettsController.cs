using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;
using PagedList; // dodato za prikaz podataka po stranicama
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace TeliaMVC.Controllers
{
    public class FakturaoppsettsController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        // GET: Fakturaoppsetts
        //SearchParameter -je atribut koji se prosledjuje iz selektovanog radio-button-a
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,string SearchParameter,int? id) // id_sesije za prenos kada se vrsi search, a int? id za prenos id direktno
        {
            ViewBag.ID = id;
            var faktures = from s in db.Fakturaoppsetts
                           select s;
            faktures = faktures.Where(s => s.Id_client == id);
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
            

            //pretrazivanje pre rasporedjivanja:
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (SearchParameter)
                {
                    case "Kostnadsted":
                        faktures = faktures.Where(s => s.NavnPaKostnadssted.Contains(searchString));
                        break;
                    case "Fakturaadresse":
                        faktures = faktures.Where(s => s.Fakturaadresse.Contains(searchString));
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
        #region CRUD operacije
        public ActionResult Create(int? id)
        {
            ViewBag.id_sesije = id;
            return View();
        }

        // POST: Fakturaoppsetts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NavnPaKostnadssted,Tileggsinfo_kostnadssted,Fakturaformat,Fakturaadresse,Husnr,Bokstav,Postnummer,Sted,Epost,Kostnadssted,Id_client")] Fakturaoppsett fakturaoppset,string selected)
        {
            fakturaoppset.Kostnadssted = fakturaoppset.NavnPaKostnadssted;
            fakturaoppset.Fakturaformat = selected;
            fakturaoppset.Sted = vratiSted(fakturaoppset.Postnummer);
            if (ModelState.IsValid)
            {
                db.Fakturaoppsetts.Add(fakturaoppset);
                try { db.SaveChanges(); }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation(
                                  "Class: {0}, Property: {1}, Error: {2}",
                                  validationErrors.Entry.Entity.GetType().FullName,
                                  validationError.PropertyName,
                                  validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index",new { id = fakturaoppset.Id_client});
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
            ViewBag.id_sesije = fakturaoppsett.Id_client;
            return View(fakturaoppsett);
        }

        // POST: Fakturaoppsetts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NavnPaKostnadssted,Tileggsinfo_kostnadssted,Fakturaformat,Fakturaadresse,Husnr,Bokstav,Postnummer,Sted,Epost,Kostnadssted,Id_client")] Fakturaoppsett fakturaoppsett,string selected)
        {
            fakturaoppsett.Fakturaformat = selected;
            fakturaoppsett.Kostnadssted = fakturaoppsett.NavnPaKostnadssted;
            fakturaoppsett.Sted = vratiSted(fakturaoppsett.Postnummer);
            if (ModelState.IsValid)
            {
                db.Entry(fakturaoppsett).State = EntityState.Modified;
                try { db.SaveChanges(); }
                catch (Exception) { throw; }
                return RedirectToAction("Index", "Fakturaoppsetts", new { id = fakturaoppsett.Id_client});
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
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
            ViewBag.id_sesije = fakturaoppsett.Id_client;
            return View(fakturaoppsett);
        }

        // POST: Fakturaoppsetts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Find(id);

            db.Fakturaoppsetts.Remove(fakturaoppsett);
            try { db.SaveChanges(); }
            catch (Exception) { throw; }
            return RedirectToAction("Index", "Fakturaoppsetts",new { id = fakturaoppsett.Id_client });
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
        public string vratiSted(int? numm)
        {
            try
            {
                if (numm == null)
                {
                    return "";
                }
                else
                {
                    return db.Postnummers.Where(s => s.PostNr.Contains(numm.ToString())).First().Poststed;

                }
            }
            catch
            {
                return "";
            }
        }
    }
}
