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
    public class FakturaoppsettsController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        // GET: Fakturaoppsetts
        public ActionResult Index()
        {
            return View(db.Fakturaoppsetts.ToList());
        }

        // GET: Fakturaoppsetts/Details/5
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

        // GET: Fakturaoppsetts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fakturaoppsetts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NavnPaKostnadssted,Tileggsinfo_kostnadssted,Fakturaformat,Fakturaadresse,Husnr,Bokstav,Postnummer,Sted,Epost,Kostnadssted")] Fakturaoppsett fakturaoppsett)
        {
            if (ModelState.IsValid)
            {
                db.Fakturaoppsetts.Add(fakturaoppsett);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fakturaoppsett);
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
