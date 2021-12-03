using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;
using System.Data.Entity.Validation;
using PagedList;
using System.Diagnostics;

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

            int pageSize = 10;
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
            ViewBag.TypeM = FillSelectBoxClients("M");
            ViewBag.TypeI = FillSelectBoxClients("I");
            ViewBag.TypeF = FillSelectBoxClients("F");
            return View();
        }

        public ActionResult Edit(string orgnummer)
        {
            if (orgnummer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = db.Clients.Where(a => a.Orgnummer == orgnummer).FirstOrDefault();
            Client c = db.Clients.Find(client.Id);
            if (c == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeM = FillSelectBoxClients("M");
            ViewBag.TypeI = FillSelectBoxClients("I");
            ViewBag.TypeF = FillSelectBoxClients("F");
            ViewBag.ID = c.Id;
            return View(c);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Orgnummer ,Password,Id_admin,Id_abonementype,FirmaNavn,GateNavn,HusNummer,PostNummer,Sted,Epost,KontaktNavn,KontaktEpost,KontaktTlfnr,TekniskKontaktNavn,TekniskKontaktEpost,TekniskKontaktTlfnr")] Client client, string selectedM, string selectedI,string selectedF)
        {
            int id = GetAbonementypeId(selectedM);
            int idI = GetAbonementypeId(selectedI);
            int idF = GetAbonementypeId(selectedF);
            if (id == 0)
            {
                return View(client);
            }
            else
            {
                client.Id_abonementype = id;
                client.Id_abonementypeI = idI;
                client.Id_abonemetypeF = idF;
                if (ModelState.IsValid)
                {
                    db.Entry(client).State = EntityState.Modified;
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
                    return RedirectToAction("Index"); // refresh stranicu opet
                }
            }
            ViewBag.TypeM = FillSelectBoxClients("M");
            ViewBag.TypeI = FillSelectBoxClients("I");
            ViewBag.TypeF = FillSelectBoxClients("F");
            return View(client);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Orgnummer ,Password,Id_admin,Id_abonementype")] Client client,string selectedM,string selectedI,string selectedF)
        {
            int id = GetAbonementypeId(selectedM);
            int idI = GetAbonementypeId(selectedI);
            int idF = GetAbonementypeId(selectedF);
            if (id == 0)
            {
                return View(client);
            }
            else
            {
                client.Id_abonementype = id;
                client.Id_abonementypeI= idI;
                client.Id_abonemetypeF= idF;
                if (ModelState.IsValid)
                {
                    db.Clients.Add(client);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    return RedirectToAction("Index"); // refresh stranicu opet
                }
            }
            ViewBag.TypeM = FillSelectBoxClients("M");
            ViewBag.TypeI = FillSelectBoxClients("I");
            ViewBag.TypeF = FillSelectBoxClients("F");
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
        //novo
        public ActionResult Delete(string orgnummer)
        {
            if (orgnummer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = db.Clients.Where(a => a.Orgnummer == orgnummer).FirstOrDefault();
            Client c = db.Clients.Find(client.Id);
            if (c == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = c.Id;
            return View(c);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string orgnummer)
        {
            var client = db.Clients.Where(a => a.Orgnummer == orgnummer).FirstOrDefault();
            db.Clients.Remove(client);
            try { db.SaveChanges(); }
            catch (Exception) { throw; }
            return RedirectToAction("Index");
        }

        #region pomocne
        public List<String> FillSelectBoxClients(string c) // selectbox za abonementypes
        {
            List<String> names = new List<String>();
            foreach (var item in db.Abonementypes.ToList())
            {
                if (item.Num_type == c)
                {
                    names.Add(item.Name);
                }
                
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