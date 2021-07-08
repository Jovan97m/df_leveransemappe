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
    public class NummersController : Controller
    {
        
        private TeliaEntities db = new TeliaEntities();

        // GET: Nummers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            //Viewbags- za sortiranja svake kolone;
            ViewBag.Telefonnummer = String.IsNullOrEmpty(sortOrder) ? "telefonnummer_desc" : "";
            ViewBag.TilegsSortParm = sortOrder == "Tilegs" ? "tilegs_desc" : "Tilegs"; // mislim da ne bi trebalo da ima ova
            ViewBag.EtternavnSortParm = sortOrder == "Etternavn" ? "etternavn_desc" : "EtternavnFormat";
            ViewBag.Bedrift_som_skal_faktureresSortParm = sortOrder == "Bedrift_som_skal_faktureres	" ? "bedrift_som_skal_faktureres_desc" : "Bedrift_som_skal_faktureres";
            ViewBag.c_o_adresse_for_SIM_leveringSortParm = sortOrder == "c_o_adresse_for_SIM_levering" ? "c_o_adresse_for_SIM_levering_desc" : "c_o_adresse_for_SIM_levering";
            ViewBag.Gateadresse_SIM_Skal_sendes_tilSortParm = sortOrder == "Gateadresse_SIM_Skal_sendes_til" ? "gateadresse_SIM_Skal_sendes_til_desc" : "Gateadresse_SIM_Skal_sendes_til";
            ViewBag.Hus_nummerSortParm = sortOrder == "Hus_nummer" ? "hus_nummer_desc" : "Hus_nummer";
            ViewBag.Hus_bokstavSortParm = sortOrder == "Hus_bokstav" ? "hus_bokstav_desc" : "Hus_bokstav";
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



            var nummers = from s in db.Nummers
                           select s;
            //pretrazivanje pre rasporedjivanja:
            if (!String.IsNullOrEmpty(searchString))
            {
                nummers = nummers.Where(s => s.Fornavn.Contains(searchString));
            }


            switch (sortOrder)
            {
                //prva kolona
                case "telefonnummer_desc":
                    nummers = nummers.OrderByDescending(s => s.Telefonnummer);
                    break;
                //druga kolona:
                case "Tilegs":
                    nummers = nummers.OrderBy(s => s.Abonnementstype);
                    break;
                case "tilegs_desc":
                    nummers = nummers.OrderByDescending(s => s.Abonnementstype);
                    break;

                //treca kolona,fakture
                case "Etternavn":
                    nummers = nummers.OrderBy(s => s.Etternavn);
                    break;
                case "etternavn_desc":
                    nummers = nummers.OrderByDescending(s => s.Etternavn);
                    break;

                //cetvrta kolona,adrese za fakture
                case "c_o_adresse_for_SIM_levering":
                    nummers =nummers.OrderBy(s => s.c_o_adresse_for_SIM_levering);
                    break;
                case "c_o_adresse_for_SIM_levering_desc":
                    nummers = nummers.OrderByDescending(s => s.c_o_adresse_for_SIM_levering);
                    break;

                //Peta kolona, Husnr
                case "Gateadresse_SIM_Skal_sendes_til":
                    nummers = nummers.OrderBy(s => s.Gateadresse_SIM_Skal_sendes_til);
                    break;
                case "husnr_desc":
                    nummers = nummers.OrderByDescending(s => s.Gateadresse_SIM_Skal_sendes_til);
                    break;

                //sesta kolona, Bokstav , mozda i ne mora
                case "Hus_nummer":
                    nummers = nummers.OrderBy(s => s.Hus_nummer);
                    break;
                case "hus_nummer_desc":
                    nummers = nummers.OrderByDescending(s => s.Hus_nummer);
                    break;

                //sedma kolona PostNummer
                case "Hus_bokstav":
                    nummers = nummers.OrderBy(s => s.Hus_bokstav);
                    break;
                case "postnummer_desc":
                    nummers = nummers.OrderByDescending(s => s.Hus_bokstav);
                    break;
                default:
                    nummers = nummers.OrderBy(s => s.Telefonnummer);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(nummers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Nummers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nummer nummer = db.Nummers.Find(id);
            if (nummer == null)
            {
                return HttpNotFound();
            }
            return View(nummer);
        }

        // GET: Nummers/Create
        public ActionResult Create()
        {
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted");
            return View();
        }

        // POST: Nummers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim")] Nummer nummer)
        {
            nummer.Abonnementstype = nummer.Abonnementstype + "GB";
            if (ModelState.IsValid)
            {
                db.Nummers.Add(nummer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            return View(nummer);
        }

        // GET: Nummers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nummer nummer = db.Nummers.Find(id);
            if (nummer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            return View(nummer);
        }

        // POST: Nummers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim")] Nummer nummer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nummer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            return View(nummer);
        }

        // GET: Nummers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nummer nummer = db.Nummers.Find(id);
            if (nummer == null)
            {
                return HttpNotFound();
            }
            return View(nummer);
        }

        // POST: Nummers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nummer nummer = db.Nummers.Find(id);
            db.Nummers.Remove(nummer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
