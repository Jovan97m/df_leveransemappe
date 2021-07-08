using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;

namespace TeliaMVC.Controllers
{
    public class NummersAdminController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        // GET: NummersAdmin
        public ActionResult Index()
        {
            var nummers = db.Nummers.Include(n => n.Fakturaoppsett);
            return View(nummers.ToList());
        }

        // GET: NummersAdmin/Details/5
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

        // GET: NummersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted");
            return View();
        }

        // POST: NummersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,ID")] Nummer nummer)
        {
            if (ModelState.IsValid)
            {
                db.Nummers.Add(nummer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            return View(nummer);
        }

        // GET: NummersAdmin/Edit/5
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

        // POST: NummersAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,ID")] Nummer nummer)
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

        // GET: NummersAdmin/Delete/5
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

        // POST: NummersAdmin/Delete/5
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
