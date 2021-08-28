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
    public class AbonementypesController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        // GET: Abonementypes
        public ActionResult Index()
        {
            return View(db.Abonementypes.ToList());
        }

        // GET: Abonementypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonementype abonementype = db.Abonementypes.Find(id);
            if (abonementype == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            ViewData["Tipovi"] = VratiTipove(abonementype.Id);
            return View(abonementype);
        }

        // GET: Abonementypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Abonementypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Num_type")] Abonementype abonementype,string NumType)
        {
            abonementype.Num_type = NumType;
            if (ModelState.IsValid)
            {
                db.Abonementypes.Add(abonementype);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
                return RedirectToAction("Index");
            }

            return View(abonementype);
        }

        public ActionResult AddType(int? id)
        {
            if (id == 0)
            {
                ViewBag.id_abom = null;
                List<string> l = new List<string>();
                l.Add(ViewBag.id_abom);
                ViewBag.ids = l;
            }
            else
            { 
                 ViewBag.id_abom = id.ToString();
                 List<string> l = new List<string>();
                 l.Add(ViewBag.id_abom);
                 ViewBag.ids = l;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddType([Bind(Include = "Id,Name,Reference_code")] TeliaMVC.Models.Type type,string selected)
        {
            string prenos= selected;
            if (prenos!="")
            {
                if (ModelState.IsValid)
                {
                    db.Types.Add(type);
                    ConnectionType t = new ConnectionType();
                    t.Id_abom = Convert.ToInt32(prenos);
                    t.Id_type = type.Id;
                    db.ConnectionTypes.Add(t);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    return RedirectToAction("Index");
                }
                else
                    return View(type);
            }
            else
            {
                //DEFAULT
                if (ModelState.IsValid)
                {
                    db.Types.Add(type);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    return RedirectToAction("Index");
                }
                else
                    return View(type);
            }
        }
        // GET: Abonementypes/Edit/5


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonementype abonementype = db.Abonementypes.Find(id);
            if (abonementype == null)
            {
                return HttpNotFound();
            }

            return View(abonementype);
        }
        public ActionResult EditType(int? id, int? id_abom)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeliaMVC.Models.Type type = db.Types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id_abom;
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditType([Bind(Include = "Id,Name,Reference_code")] TeliaMVC.Models.Type tip,int? test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tip).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
                return RedirectToAction("Details",new { id=test });
            }
            return View(tip);
        }

        // POST: Abonementypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Num_type")] Abonementype abonementype,string NumType)
        {
            abonementype.Num_type = NumType;
            if (ModelState.IsValid)
            {
                db.Entry(abonementype).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(abonementype);
        }

        // GET: Abonementypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonementype abonementype = db.Abonementypes.Find(id);
            if (abonementype == null)
            {
                return HttpNotFound();
            }
            return View(abonementype);
        }
        public ActionResult DeleteType(int? id, int? id_abom)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeliaMVC.Models.Type type = db.Types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id_abom;
            return View(type);
        }

        // POST: Abonementypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Abonementype abonementype = db.Abonementypes.Find(id);
            db.Abonementypes.Remove(abonementype);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }
        // POST: Abonementypes/Delete/5
        [HttpPost, ActionName("DeleteType")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTypeConfirmed(int id,int? test)
        {
            TeliaMVC.Models.Type type = db.Types.Find(id);
            db.Types.Remove(type);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Details",new { id=test});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //pomocna:
        public List<TeliaMVC.Models.Type> VratiTipove(int id) // selectbox za abonementypes
        {
            List<TeliaMVC.Models.Type> types = new List<TeliaMVC.Models.Type>();
            List<int> ids = new List<int>();

            var veza = db.ConnectionTypes.Where(s => s.Id_abom.Equals(id));
            foreach (var item in veza)
            {
              types.Add(db.Types.Find(item.Id_type));
            }
            return types;
        }
    }
}
