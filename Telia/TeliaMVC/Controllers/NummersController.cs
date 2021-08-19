using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TeliaMVC.Models;
using PagedList;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace TeliaMVC.Controllers
{
    public class NummersController : Controller
    {
        
        private TeliaEntities db = new TeliaEntities();

        // GET: Nummers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString,string id_sesije, int? page)
        {
            var nummers = from s in db.Nummers
                          select s;
            Client client = db.Clients.Find(Convert.ToInt32(id_sesije));
            //formiraj listu za odredjenog klijenta
            nummers = nummers.Where(s => s.Orgnummer.Contains(client.Orgnummer));


            ViewBag.ID = Convert.ToInt32(id_sesije);
            ViewBag.CurrentSort = sortOrder; // za paging,da ostane sortirano kad se radi stranicenje
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
                        nummers = nummers.OrderBy(s=>s.Fornavn);
                    break;
                case "fornavn_desc":
                        nummers = nummers.OrderByDescending(s=>s.Fornavn);
                    break;
                case "Bedrift_som_skal_faktureres":
                    nummers = nummers.OrderBy(s=>s.Bedrift_som_skal_faktureres);
                    break;
                case "bedrift_som_skal_faktureres_desc":
                    nummers = nummers.OrderByDescending(s=>s.Bedrift_som_skal_faktureres);
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
                    nummers = nummers.OrderBy(s=> s.post_nr_);
                    break;
                case "post_nr_desc":
                    nummers = nummers.OrderByDescending(s=> s.post_nr_);
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
                    nummers = nummers.OrderBy(s=>s.Ekstra_talesim_);
                    break;
                case "ekstra_talesim_desc":
                    nummers = nummers.OrderByDescending(s=>s.Ekstra_talesim_);
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
        public ActionResult Create(int? sesija)
        {
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted");
            
            Client client = db.Clients.Find(sesija);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype); // selectbox za abonementype
            ViewBag.ORG  = client.Orgnummer;
            return View();
        }

        // POST: Nummers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer,string selected)
        {
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
             if (ModelState.IsValid)
             {
                int  id = db.Clients.Where(s => s.Orgnummer.Contains(nummer.Orgnummer)).FirstOrDefault().Id;
                db.Nummers.Add(nummer);
                db.SaveChanges();
                return RedirectToAction("Index","Nummers", new {id_sesije = id });
            }

           ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            ViewBag.ORG = nummer.Orgnummer;
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
            ViewBag.ID = GetId(nummer.Orgnummer);
            return View(nummer);
        }

        // POST: Nummers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer)
        {
            Client c = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));
          //  nummer.Orgnummer = c.Orgnummer;
            ViewBag.ID = nummer.Orgnummer;
            nummer.Pending = true;
            nummer.Date = null;
            if (ModelState.IsValid)
            {
                db.Entry(nummer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id_sesije = c.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
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
            ViewBag.ID = GetId(nummer.Orgnummer);
            return View(nummer);
        }

        // POST: Nummers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nummer nummer = db.Nummers.Find(id);
           //Client c = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));
            //nummer.Orgnummer = c.Orgnummer;
            ViewBag.ID = GetId(nummer.Orgnummer);
            db.Nummers.Remove(nummer);
            db.SaveChanges();
            return RedirectToAction("Index", new { id_sesije = ViewBag.ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //da na osnovu orgNUmmer vrati ID klijenta
        public string GetId(string orgNummer)
        {
            var c = db.Clients.Where(s => s.Orgnummer.Contains(orgNummer));
            if (c == null)
            {
                return "";
            }
            else
                return c.FirstOrDefault().Id.ToString() ;
        }
        public List<String> FillAbonementtypeSelectBox(int id) // selectbox za abonementypes
        {
            List<String> types = new List<String>();
            List<int> ids = new List<int>();

             var veza=   db.ConnectionTypes.Where(s => s.Id_abom.Equals(id));
            foreach (var item in veza)
            {
                types.Add(db.Types.Where(s => s.Id.Equals(item.Id_type)).First().Name.ToString());
            }

            veza = db.ConnectionTypes;
            List<int> id_types = new List<int>();
            foreach (var item in veza)
            {
                id_types.Add(item.Id_type);
            }

            List<int> id_return = new List<int>();
            foreach (var item in db.Types)
            {
                if (!id_types.Contains(item.Id))
                {
                    id_return.Add(item.Id);
                }
            }

            foreach (var item in db.Types)
            {
                    if (id_return.Contains(item.Id))
                    {
                        types.Add(item.Name);
                    }
            }

            return types;
        }


        public ActionResult Export()
        {
            try
            {
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;


                worksheet.Cells[1, 1] = "Telefonnummer";
                worksheet.Cells[1, 2] = "Fornavn";
                worksheet.Cells[1, 3] = "Etternavn";
                worksheet.Cells[1, 4] = "E-postadresse";
                worksheet.Cells[1, 5] = "Tilleggsinfo, bruker";
                worksheet.Cells[1, 6] = "Gatenavn";
                worksheet.Cells[1, 7] = "Husnummer";
                worksheet.Cells[1, 8] = "Husbokstav";
                worksheet.Cells[1, 9] = "Postnummer";
                worksheet.Cells[1, 10] = "Katalogoppføring";
                worksheet.Cells[1, 11] = "Kostnadssted (BAN)";
                worksheet.Cells[1, 12] = "Porteringsdato og tid";
                worksheet.Cells[1, 13] = "Nåværende eier, Navn";
                worksheet.Cells[1, 14] = "Nåværende Eier ID (Org.nr/f.dato)";
                worksheet.Cells[1, 15] = "Abonnementstype";
                worksheet.Cells[1, 16] = "Binding";
                worksheet.Cells[1, 17] = "Antall TrillingSIM (maks 2)";
                worksheet.Cells[1, 18] = "Antall DataSIM (maks 5)";
                worksheet.Cells[1, 19] = "Manuell Top-up";
                worksheet.Cells[1, 20] = "Sperre Top-up";
                worksheet.Cells[1, 21] = "Norden";
                worksheet.Cells[1, 22] = "Tale og SMS til EU";
                worksheet.Cells[1, 23] = "TBN";
                worksheet.Cells[1, 24] = "HovedSIM";
                worksheet.Cells[1, 25] = "TrillingSIM1";
                worksheet.Cells[1, 26] = "TrillingSIM2";
                worksheet.Cells[1, 27] = "DataSIM1";
                worksheet.Cells[1, 28] = "DataSIM2";
                worksheet.Cells[1, 29] = "DataSIM3";
                worksheet.Cells[1, 30] = "DataSIM4";
                worksheet.Cells[1, 31] = "DataSIM5";
                worksheet.Cells[1, 32] = "DeliveryMethodCode";
                worksheet.Cells[1, 33] = "DeliveryStreetName";
                worksheet.Cells[1, 34] = "DeliveryStreetNumber";
                worksheet.Cells[1, 35] = "DeliveryStreetSuffix";
                worksheet.Cells[1, 36] = "DeliveryCity";
                worksheet.Cells[1, 37] = "DeliveryZip";
                worksheet.Cells[1, 38] = "DeliveryCountryCode";
                worksheet.Cells[1, 39] = "DeliveryContactEmail";
                worksheet.Cells[1, 40] = "DeliveryContactCountryCode";
                worksheet.Cells[1, 41] = "DeliveryContactLocalNumber";
                worksheet.Cells[1, 42] = "DeliveryIndividualFirstName";
                worksheet.Cells[1, 43] = "DeliveryIndividualLastName";
                worksheet.get_Range("A1,AS1").EntireColumn.AutoFit();
                workbook.SaveAs("e:\\MyDemo.xlsx");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);

                application.Quit();
                Marshal.FinalReleaseComObject(application);
                ViewBag.Result = "Done";
            }
            catch(Exception ex)
            {
                ViewBag.Result = ex.Message;
            }
            return View();
        }
    }
}
