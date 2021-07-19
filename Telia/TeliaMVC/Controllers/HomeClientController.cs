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
            return View(client);
        }
        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Orgnummer,Password,FirmaNavn,GateNavn,HusNummer,HusBokStav,PostNummer,Sted,Epost,KontaktNavn,KontaktEpost,KontaktTlfnr,TekniskKontaktNavn,TekniskKontaktEpost,TekniskKontaktTlfnr")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id = client.Id});
            }
            return View(client);
        }

    }
}