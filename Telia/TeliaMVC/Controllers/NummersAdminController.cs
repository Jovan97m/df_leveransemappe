﻿using System;
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
    public class NummersAdminController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        // GET: NummersAdmin
        public ActionResult Index(string sortOrder, string currentFilter, string searchString,string SearchParameter,string selected, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Telefonnummer = String.IsNullOrEmpty(sortOrder) ? "telefonnummer_desc" : "";
            ViewBag.Abonnementstype = sortOrder == "Abonnementstype" ? "abonnementstype_desc" : "Abonnementstype"; // mislim da ne bi trebalo da ima ova
            ViewBag.EtternavnSortParm = sortOrder == "Etternavn" ? "etternavn_desc" : "Etternavn";
            ViewBag.FornavnSortParm = sortOrder == "Fornavn" ? "fornavn_desc" : "Fornavn";
            ViewBag.Bedrift_som_skal_faktureresSortParm = sortOrder == "Bedrift_som_skal_faktureres	" ? "bedrift_som_skal_faktureres_desc" : "Bedrift_som_skal_faktureres";
            ViewBag.c_o_adresse_for_SIM_leveringSortParm = sortOrder == "c_o_adresse_for_SIM_levering" ? "c_o_adresse_for_SIM_levering_desc" : "c_o_adresse_for_SIM_levering";
            ViewBag.Gateadresse_SIM_Skal_sendes_tilSortParm = sortOrder == "Gateadresse_SIM_Skal_sendes_til" ? "gateadresse_SIM_Skal_sendes_til_desc" : "Gateadresse_SIM_Skal_sendes_til";
            ViewBag.Hus_nummerSortParm = sortOrder == "Hus_nummer" ? "hus_nummer_desc" : "Hus_nummer";
            ViewBag.Hus_bokstavSortParm = sortOrder == "Hus_bokstav" ? "hus_bokstav_desc" : "Hus_bokstav";
            ViewBag.post_nr_SortParm = sortOrder == "post_nr_" ? "post_nr_desc" : "post_nr_";
            ViewBag.Post_stedSortParm = sortOrder == "Post_sted" ? "post_sted_desc" : "Post_sted";
            ViewBag.Ekstra_talesimSortParm = sortOrder == "Ekstra_talesim_" ? "ekstra_talesim_desc" : "Ekstra_talesim_";
            ViewBag.Ekstra_datasimSortParm = sortOrder == "Ekstra_datasim_" ? "ekstra_datasim_desc" : "Ekstra_datasim_";
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

            //Deo za selectBox,da se ucita uvek u dropdown menu.
            List<Client> clients = db.Clients.ToList();
            List<String> orgNummers = new List<String>();

            orgNummers.Add("All");
            foreach (var item in clients)
            {
                //kad se doda u bazu
                //string final = item.Orgnummer + "-" + item.FirmaNavn;
                orgNummers.Add(item.Orgnummer);
            }
            ViewBag.nummers = orgNummers;

            //Ovde izmeni brojeve koji treba da se prikazu na osnovu selektovanog  
            if (selected != null)
            {
                if (selected != "All")
                {
                    nummers = nummers.Where(s => s.Orgnummer.Contains(selected));
                }
            }


            //pretrazivanje pre rasporedjivanja:
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (SearchParameter)
                {
                    case "Telefonnummer":
                        nummers = nummers.Where(s => s.Telefonnummer.Contains(searchString));
                        break;
                    case "Binding":
                        nummers = nummers.Where(s=> s.Binding.Contains(searchString));
                        break;
                    case "Fornavn":
                        nummers = nummers.Where(s=> s.Fornavn.Contains(searchString));
                        break;
                    default:
                        break;
                }
            }
            switch (sortOrder)
            {
                //prva kolona
                case "telefonnummer_desc":
                    nummers = nummers.OrderByDescending(s => s.Telefonnummer);
                    break;
                //druga kolona:
                case "Abonnementstype":
                    nummers = nummers.OrderBy(s => s.Abonnementstype);
                    break;
                case "abonnementstype_desc":
                    nummers = nummers.OrderByDescending(s => s.Abonnementstype);
                    break;

                //treca kolona,fakture
                case "Etternavn":
                    nummers = nummers.OrderBy(s => s.Etternavn);
                    break;
                case "etternavn_desc":
                    nummers = nummers.OrderByDescending(s => s.Etternavn);
                    break;
                case "Fornavn":
                    nummers = nummers.OrderBy(s => s.Fornavn);
                    break;
                case "fornavn_desc":
                    nummers = nummers.OrderByDescending(s => s.Fornavn);
                    break;
                case "Bedrift_som_skal_faktureres":
                    nummers = nummers.OrderBy(s => s.Bedrift_som_skal_faktureres);
                    break;
                case "bedrift_som_skal_faktureres_desc":
                    nummers = nummers.OrderByDescending(s => s.Bedrift_som_skal_faktureres);
                    break;


                //cetvrta kolona,adrese za fakture
                case "c_o_adresse_for_SIM_levering":
                    nummers = nummers.OrderBy(s => s.c_o_adresse_for_SIM_levering);
                    break;
                case "c_o_adresse_for_SIM_levering_desc":
                    nummers = nummers.OrderByDescending(s => s.c_o_adresse_for_SIM_levering);
                    break;

                //Peta kolona, Husnr
                case "Gateadresse_SIM_Skal_sendes_til":
                    nummers = nummers.OrderBy(s => s.Gateadresse_SIM_Skal_sendes_til);
                    break;

                case "gateadresse_SIM_Skal_sendes_til_desc":
                    nummers = nummers.OrderByDescending(s => s.Gateadresse_SIM_Skal_sendes_til);
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
                case "hus_bokstav_desc":
                    nummers = nummers.OrderByDescending(s => s.Hus_bokstav);
                    break;

                case "post_nr_":
                    nummers = nummers.OrderBy(s => s.post_nr_);
                    break;
                case "post_nr_desc":
                    nummers = nummers.OrderByDescending(s => s.post_nr_);
                    break;

                case "Post_sted":
                    nummers = nummers.OrderBy(s => s.Post_sted);
                    break;
                case "post_sted_desc":
                    nummers = nummers.OrderByDescending(s => s.Post_sted);
                    break;

                case "Ekstra_datasim_":
                    nummers = nummers.OrderBy(s => s.Ekstra_datasim);
                    break;
                case "ekstra_datasim_desc":
                    nummers = nummers.OrderByDescending(s => s.Ekstra_datasim);
                    break;
                case "Ekstra_talesim_":
                    nummers = nummers.OrderBy(s => s.Ekstra_talesim_);
                    break;
                case "ekstra_talesim_desc":
                    nummers = nummers.OrderByDescending(s => s.Ekstra_talesim_);
                    break;

                default:
                    nummers = nummers.OrderBy(s => s.Telefonnummer);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(nummers.ToPagedList(pageNumber, pageSize));
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
            //sa 4 opcija
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Opcija 1", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Opcija 2", Value = "1" });
            items.Add(new SelectListItem { Text = "Opcija 3", Value = "2" });
            items.Add(new SelectListItem { Text = "Opcija 4", Value = "3" });

            ViewBag.Katalogoppforing = items;
            return View();
        }

        // POST: NummersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,ID,Pending,Katalogoppforing,Porteringsdatoog_tid,Binding,Postnummer,Antall_TrillingSIM,allDataSIM,Manuell_Top_up,Sperre_Top_up,Norden,Tale_og_SMS_til_EU,TBN,HovedSIM,TrillingSIM1,TrillingSIM2,DataSIM1,DataSIM2,DataSIM3,DataSIM4,DataSIM5,DeliveryStreetName,DeliveryStreetNumber,DeliveryStreetSuffix,DeliveryCity,DeliveryZIP,DeliveryContractEmail,DeliveryContractCountyCode,DeliveryContractLocalNumber,DeliveryIndividualFirstName,DeliveryIndividualLastName")] Nummer nummer)
        {
            if (ModelState.IsValid)
            {
                nummer.Pending = false; // oznaci da je admin obradio ovu informaciju
                nummer.DeliveryMethodCode = "LETTER";
                nummer.DeliveryCountryCode = "47";


                db.Nummers.Add(nummer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            ViewBag.Clients = new SelectList(db.Clients, "Clients", "Orgnummer", nummer.Orgnummer);

            

            return View(nummer);
        }

        // GET: NummersAdmin/Edit/5
        public ActionResult Edit()
        {
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted");
            
            return View();
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








    }
}