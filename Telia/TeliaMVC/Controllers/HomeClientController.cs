using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ActionResult Index(int? id)
        {
            ViewBag.id_sesije = id;
            return View();
        }


        //Profile:
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = client.Id;
            ViewBag.Mobile =  getAbonementype(client.Id_abonementype);
            ViewBag.Fixed = getAbonementype(client.Id_abonemetypeF);
            ViewBag.Internet = getAbonementype(client.Id_abonementypeI);
            ViewBag.numberOfFaktures = getNumberOFFaktures(client.Id); 
            return View(client);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_abonemetype = client.Id_abonementype;
            ViewBag.Id_abonemetypeI = client.Id_abonementypeI;
            ViewBag.Id_abonemetypeF = client.Id_abonemetypeF;
            return View(client);
        }
        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Orgnummer,Password,FirmaNavn,GateNavn,HusNummer,HusBokStav,PostNummer,Sted,Epost,KontaktNavn,KontaktEpost,KontaktTlfnr,TekniskKontaktNavn,TekniskKontaktEpost,TekniskKontaktTlfnr,Id_abonementype,Id_abonementypeI,Id_abonemetypeF")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                try { db.SaveChanges(); }
                catch (Exception) { throw; }
                return RedirectToAction("Details",new { id = client.Id});
            }
            return View(client);
        }

        public string getAbonementype(int id)
        {
            return db.Abonementypes.Find(id).Name;
        }

        public int getNumberOFFaktures(int id)
        {
            var faktures = db.Fakturaoppsetts.Where(c=> c.Id_client == id);
            return faktures.Count();
        }

    }
}