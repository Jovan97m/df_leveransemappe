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
using System.Web;
using System.Data.Entity.Validation;
using System.Diagnostics;

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
            nummers = nummers.Where(s => s.Orgnummer.Contains(client.Id.ToString()));
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

            int pageSize = 10;
            int pageNumber = (page ?? 1);
             return View(nummers.ToPagedList(pageNumber, pageSize));
        }

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


        #region CREATE
        
        public ActionResult Create(int? sesija, string id_sesije)
        {
            Client client = db.Clients.Find(sesija);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype,"M"); // selectbox za abonementype
            ViewBag.ORG = client.Id.ToString();
            return View();
        }

        public ActionResult CreateFixed(int? sesija)
        {
            Client client = db.Clients.Find(sesija);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonemetypeF,"F"); // selectbox za abonementype
            ViewBag.ORG = client.Id.ToString();
            return View();
        }

        public ActionResult CreateInternet(int? sesija)
        {
            Client client = db.Clients.Find(sesija);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementypeI,"I"); // selectbox za abonementype
            ViewBag.ORG = client.Id.ToString();
            return View();
        }

        // POST: Nummers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer,string selected,string kostnadsted)
        {
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            nummer.Bedrift_som_skal_faktureres = fakturaoppsett.Fakturaformat;
            var idc = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));
            if (ModelState.IsValid)
             {
                var errors2 = ModelState.Values.SelectMany(v => v.Errors);
                db.Nummers.Add(nummer);
                try
                {
                    db.SaveChanges();
                }
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
                return RedirectToAction("Index","Nummers", new {id_sesije = idc.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype,"M");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.ORG =idc.Id.ToString();
            return View(nummer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFixed([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer, string selected, string kostnadsted)
        {
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            nummer.Bedrift_som_skal_faktureres = fakturaoppsett.Fakturaformat;
            var idc = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));
            if (ModelState.IsValid)
            {
                var errors2 = ModelState.Values.SelectMany(v => v.Errors);
                db.Nummers.Add(nummer);
                try
                {
                    db.SaveChanges();
                }
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
                return RedirectToAction("Index", "Nummers", new { id_sesije = idc.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype, "F");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.ORG = idc.Id.ToString();
            return View(nummer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInternet([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer, string selected, string kostnadsted)
        {
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            nummer.Bedrift_som_skal_faktureres = fakturaoppsett.Fakturaformat;
            var idc = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));
            if (ModelState.IsValid)
            {
                var errors2 = ModelState.Values.SelectMany(v => v.Errors);
                db.Nummers.Add(nummer);
                try
                {
                    db.SaveChanges();
                }
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
                return RedirectToAction("Index", "Nummers", new { id_sesije = idc.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype, "I");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.ORG = idc.Id.ToString();
            return View(nummer);
        }

        #endregion


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


            var c = db.Clients.Find(Convert.ToInt32(GetId(nummer.Orgnummer)));
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(c.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementype,"M"); // selectbox za abonementype
            ViewBag.ID = GetId(nummer.Orgnummer);
            ViewBag.Orgnummer = nummer.Orgnummer;
            return View(nummer);
        }

        // POST: Nummers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer, string selected,string kostnadsted)
        {
            Client c = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));

            ViewBag.ID = nummer.Orgnummer ;
            nummer.Abonnementstype = selected;
            nummer.Pending = true;
            nummer.Date = DateTime.Today;
            nummer.Kostnadsted = kostnadsted;
            nummer.Bedrift_som_skal_faktureres= db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).First().Fakturaformat;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            if (ModelState.IsValid)
            {
                db.Entry(nummer).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
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

                return RedirectToAction("Index", new { id_sesije = c.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementype,"M");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(c.Id);
            //ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
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
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
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
            var c = db.Clients.Find(Convert.ToInt32(orgNummer));
            if (c == null)
            {
                return "";
            }
            else
                return c.Id.ToString();
        }
        public List<String> FillKostnadstedSelectBox(int id)
        {
            List<String> povratna = new List<String>();
            var test = db.Fakturaoppsetts.Where(s => s.Id_client == id) ;
            foreach (var item in test)
            {
                povratna.Add(item.NavnPaKostnadssted);
            }
            return povratna;
        }
        public List<String> FillAbonementtypeSelectBox(int id,string type) // selectbox za abonementypes
        {
            List<String> types = new List<String>();
            List<int> ids = new List<int>();


            var veza = db.ConnectionTypes.Where(s => s.Id_abom.Equals(id));
            foreach (var item in veza)
            {
                types.Add(db.Types.Where(s => s.Id.Equals(item.Id_type)).First().Name.ToString());
            }
            return types;
        }

        public string VratiPostSted(int? numm)
        {
            try
            {
                if (numm == null)
                {
                    return "";
                }
                else
                {
                    string number = FormProperPostNummer(numm);
                   // return db.Postnummers.Where(s => s.PostNr.Contains(FormProperPostNummer(numm))).First().Poststed;
                    var PostSted = db.Postnummers.Where(s => s.PostNr.Contains(number)).FirstOrDefault();
                    if (PostSted == null)
                        return "Feil postnummer";
                    return PostSted.Poststed;
                }
            }
            catch
            {
                return "";
            }
        }
        public string FormProperPostNummer(int? numm)
        {
            string finalString = numm.ToString();
            if (finalString.Length == 1)
            {
                finalString = "000" + finalString;
            }
            else if(finalString.Length == 2)
            {
                finalString = "00" + finalString;
            }
            else if(finalString.Length == 3)
            {
                finalString = "0" + finalString;
            }
            return finalString;
                 
        }

        #region excel

        public ActionResult Excel(HttpPostedFileBase excelfile, string id_sesije)
        {
            if (excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Du har ikke valgt noen filer";
                return View();

            }
            else
            {
                string fileExtension = System.IO.Path.GetExtension(excelfile.FileName);
                if (fileExtension.EndsWith(".xls") || fileExtension.EndsWith(".xlsx"))
                {
                    string fileLocation = Server.MapPath("~/Content/" + excelfile.FileName);
                    // if (System.IO.File.Exists(fileLocation)) 
                    //  System.IO.File.Delete(fileLocation);

                    excelfile.SaveAs(fileLocation);
                    ViewBag.Name = excelfile.FileName;
                    string m = (string)ViewBag.Name;
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(fileLocation);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range1 = worksheet.UsedRange;
                    Microsoft.Office.Interop.Excel.Range range = null;

                    List<string> NN = new List<string>();
                    for (int j = 1; j <= range1.Columns.Count; j++)
                    {
                        range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, j];
                        if (range == null)
                            NN.Add("");
                        else
                        {
                            if (range.Value != null)
                            {
                                string r = (string)range.Value.ToString();
                                NN.Add(r);
                            }
                        }
                            
                    }
                    workbook.Close();
                    application.Quit();
                    ViewData["Kolone"] = NN;
                }
            }
            return View();
        }

        
        public int konvertujUBroj(string i)
        {
            if (i == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(i);
            }
        }
        public Object vratiRange(Excel.Worksheet worksheet, int i, int j, Excel.Range range)
        {
            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[j, i];
            if (range.Value == null)
            {
                return "" ;
            }
            else
            {
                return range.Value.ToString();
            }
        }
        public Nummer vratiBroj(string n)
        {
            
            if (n == null)
            {
                return null;
            }
            else
            {
                return db.Nummers.Where(s => s.Telefonnummer.Contains(n)).First();
            }
        }

        public ActionResult Export()
        {
            try
            {
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;


                worksheet.Cells[1, 1] = "Telefonnummer";
                worksheet.Cells[1, 2] = "Abonnementstype";
                worksheet.Cells[1, 3] = "Fornavn";
                worksheet.Cells[1, 4] = "Etternavn";
                worksheet.Cells[1, 5] = "Bedrift_som_skal_faktureres";
                worksheet.Cells[1, 6] = "c_o_adresse_for_SIM_levering";
                worksheet.Cells[1, 7] = "Gateadresse_SIM_Skal_sendes_til";
                worksheet.Cells[1, 8] = "Hus_nummer";
                worksheet.Cells[1, 9] = "Hus_bokstav";
                worksheet.Cells[1, 10] = "post_nr_";
                worksheet.Cells[1, 11] = "Post_sted";
                worksheet.Cells[1, 12] = "Epost_for_sporings_informasjon";
                worksheet.Cells[1, 13] = "Epost";
                worksheet.Cells[1, 14] = "Tilleggsinfo_ansatt_ID";
                worksheet.Cells[1, 15] = "Ekstra_talesim_";
                worksheet.Cells[1, 16] = "Ekstra_datasim";
                worksheet.Cells[1, 17] = "Kostnadsted";

                worksheet.get_Range("A1", "Q1").EntireColumn.AutoFit();
                var range_heading = worksheet.get_Range("A1", "Q1");

                range_heading.Font.Bold = true;
                range_heading.Interior.Color = System.Drawing.ColorTranslator.FromHtml("#c3f7bc");
                worksheet.get_Range("O1,P1").Interior.Color = System.Drawing.ColorTranslator.FromHtml("#f9b3a7");
                workbook.SaveAs("c:\\Users\\Public\\Downloads\\ExcelFile.xlsx");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);

                application.Quit();
                Marshal.FinalReleaseComObject(application);
                ViewBag.Result = "Done";
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Action(string name, string id_sesije)
        {
            List<string> kolone = new List<string>();
            string c = Request.Form["uc18"];
            if (c == "false")
            {

                string Telefonnummer = Request.Form["uc1"];
                string Telefonnummer1 = Request.Form["u1"];
                if (Telefonnummer == "true,false") kolone.Add(Telefonnummer1);
                else kolone.Add("");

                string Abonnementstype = Request.Form["uc2"];
                string Abonnementstype1 = Request.Form["u2"];
                if (Abonnementstype == "true,false") kolone.Add(Abonnementstype1);
                else kolone.Add("");

                string Fornavn = Request.Form["uc3"];
                string Fornavn1 = Request.Form["u3"];
                if (Fornavn == "true,false") kolone.Add(Fornavn1);
                else kolone.Add("");

                string Etternavn = Request.Form["uc4"];
                string Etternavn1 = Request.Form["u4"];
                if (Etternavn == "true,false") kolone.Add(Etternavn1);
                else kolone.Add("");

                string Bedrift_som_skal_faktu = Request.Form["uc5"];
                string Bedrift_som_skal_faktu1 = Request.Form["u5"];
                if (Bedrift_som_skal_faktu == "true,false") kolone.Add(Bedrift_som_skal_faktu1);
                else kolone.Add("");

                string c_o_adresse_for_SIM_leve = Request.Form["uc6"];
                string c_o_adresse_for_SIM_leve1 = Request.Form["u6"];
                if (c_o_adresse_for_SIM_leve == "true,false") kolone.Add(c_o_adresse_for_SIM_leve1);
                else kolone.Add("");

                string Gateadresse_SIM_Skal = Request.Form["uc7"];
                string Gateadresse_SIM_Skal1 = Request.Form["u7"];
                if (Gateadresse_SIM_Skal == "true,false") kolone.Add(Gateadresse_SIM_Skal1);
                else kolone.Add("");

                string Hus_nummer = Request.Form["uc8"];
                string Hus_nummer1 = Request.Form["u8"];
                if (Hus_nummer == "true,false") kolone.Add(Hus_nummer1);
                else kolone.Add("");

                string Hus_bokstav = Request.Form["uc9"];
                string Hus_bokstav1 = Request.Form["u9"];
                if (Hus_bokstav == "true,false") kolone.Add(Hus_bokstav1);
                else kolone.Add("");

                string postnr = Request.Form["uc10"];
                string postnr1 = Request.Form["u10"];
                if (postnr == "true,false") kolone.Add(postnr1);
                else kolone.Add("");

                string Poststed = Request.Form["uc11"];
                string Poststed1 = Request.Form["u11"];
                if (Poststed == "true,false") kolone.Add(Poststed1);
                else kolone.Add("");

                string Epostforsporingsinfo = Request.Form["uc12"];
                string Epostforsporingsinfo1 = Request.Form["u12"];
                if (Epostforsporingsinfo == "true,false") kolone.Add(Epostforsporingsinfo1);
                else kolone.Add("");

                string Epost = Request.Form["uc13"];
                string Epost1 = Request.Form["u13"];
                if (Epost == "true,false") kolone.Add(Epost1);
                else kolone.Add("");

                string TilleggsinfoansattID = Request.Form["uc14"];
                string TilleggsinfoansattID1 = Request.Form["u14"];
                if (TilleggsinfoansattID == "true,false") kolone.Add(TilleggsinfoansattID1);
                else kolone.Add("");

                string Ekstratalesim = Request.Form["uc15"];
                string Ekstratalesim1 = Request.Form["u15"];
                if (Ekstratalesim == "true,false") kolone.Add(Ekstratalesim1);
                else kolone.Add("");

                string Ekstradatasim = Request.Form["uc16"];
                string Ekstradatasim1 = Request.Form["u16"];
                if (Ekstradatasim == "true,false") kolone.Add(Ekstradatasim1);
                else kolone.Add("");

                string Kostnadsted = Request.Form["uc17"];
                string Kostnadsted1 = Request.Form["u17"];
                if (Kostnadsted == "true,false") kolone.Add(Kostnadsted1);
                else kolone.Add("");


                try
                {


                    if (name.Length == 0)
                    {
                        ViewBag.Error = "Du har ikke valgt noen filer";
                        return View();

                    }
                    else
                    {

                        if (name.EndsWith(".xls") || name.EndsWith(".xlsx"))
                        {
                            string fileLocation = Server.MapPath("~/Content/" + name);
                            Excel.Application application = new Excel.Application();
                            Excel.Workbook workbook = application.Workbooks.Open(fileLocation);
                            Excel.Worksheet worksheet = workbook.ActiveSheet;
                            //Excel.Range range = worksheet.UsedRange;
                            List<Nummer> nIspravno = new List<Nummer>();
                            List<Nummer> nGreske = new List<Nummer>();
                            bool tacno = true;
                            List<int> indexi = new List<int>();
                            Excel.Range range1 = worksheet.UsedRange;
                            Microsoft.Office.Interop.Excel.Range range = null;
                            for (int i = 1; i <= range1.Columns.Count; i++)
                            {
                                string r = (string)vratiRange(worksheet, i, 1, range);
                                for (int j = 0; j < kolone.Count(); j++)
                                {
                                    if (kolone[j] == r && r != "")
                                    {
                                        indexi.Add(j + 1);
                                    }
                                }
                            }

                            for (int i = 2; i <= range1.Rows.Count; i++)
                            {
                                Nummer nummer = new Nummer();
                                for (int j = 0; j < range1.Columns.Count; j++)
                                {
                                    switch (indexi[j])
                                    {

                                        case 1: nummer.Telefonnummer = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 2: nummer.Abonnementstype = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 3: nummer.Fornavn = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 4: nummer.Etternavn = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 5:break;
                                        case 6: nummer.c_o_adresse_for_SIM_levering = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 8: nummer.Hus_nummer = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 9: nummer.Hus_bokstav = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 10: nummer.post_nr_ = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 11:break;
                                        case 12: nummer.Epost_for_sporings_informasjon = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 13: nummer.Epost = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 14: nummer.Tilleggsinfo_ansatt_ID = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 15: nummer.Ekstra_talesim_ = (konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range))); break;
                                        case 16: nummer.Ekstra_datasim = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 17: nummer.Kostnadsted = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        default:
                                            break;
                                    }
                                    
                                }
                                //brojac za greske koliko se pojavile 
                                nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_);
                                nummer.Bedrift_som_skal_faktureres = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(nummer.Kostnadsted)).First().Fakturaformat;
                                ProveriNummer(nummer, ref nIspravno, ref nGreske, id_sesije);
                            }
                            ViewData["Ispravno"] = nIspravno;
                            ViewData["Neispravno"] = nGreske;

                            workbook.Close();
                            application.Quit();
                            if (System.IO.File.Exists(fileLocation))
                                System.IO.File.Delete(fileLocation);
                            return View();
                        }
                        else
                        {
                            ViewBag.Error = "Du har valgt feil fil";
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = ex;
                }
                return View();
            }
            else
            {
                try
                {


                    if (name.Length == 0)
                    {
                        ViewBag.Error = "Du har ikke valgt noen filer";
                        return View();

                    }
                    else
                    {

                        if (name.EndsWith(".xls") || name.EndsWith(".xlsx"))
                        {
                            string fileLocation = Server.MapPath("~/Content/" + name);
                            Excel.Application application = new Excel.Application();
                            Excel.Workbook workbook = application.Workbooks.Open(fileLocation);
                            Excel.Worksheet worksheet = workbook.ActiveSheet;
                            //Excel.Range range = worksheet.UsedRange;
                            List<Nummer> nIspravno = new List<Nummer>();
                            List<Nummer> nGreske = new List<Nummer>();
                            bool tacno = true;
                            List<int> indexi = new List<int>();
                            Excel.Range range1 = worksheet.UsedRange;
                            Microsoft.Office.Interop.Excel.Range range = null;
                            for (int i = 2; i <= range1.Rows.Count; i++)
                            {
                                Nummer nummer = new Nummer();
                                for (int j = 1; j <= range1.Columns.Count; j++)
                                {
                                    switch (j)
                                    {

                                        case 1: nummer.Telefonnummer = (string)vratiRange(worksheet, j , i, range); break;
                                        case 2: nummer.Abonnementstype = (string)vratiRange(worksheet, j, i, range); break;
                                        case 3: nummer.Fornavn = (string)vratiRange(worksheet, j, i, range); break;
                                        case 4: nummer.Etternavn = (string)vratiRange(worksheet, j, i, range); break;
                                        case 5: break;
                                        case 6: nummer.c_o_adresse_for_SIM_levering = (string)vratiRange(worksheet, j, i, range); break;
                                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = (string)vratiRange(worksheet, j , i, range); break;
                                        case 8: nummer.Hus_nummer = konvertujUBroj((string)vratiRange(worksheet, j , i, range)); break;
                                        case 9: nummer.Hus_bokstav = (string)vratiRange(worksheet, j , i, range); break;
                                        case 10: nummer.post_nr_ = konvertujUBroj((string)vratiRange(worksheet, j , i, range)); break;
                                        case 11: break;
                                        case 12: nummer.Epost_for_sporings_informasjon = (string)vratiRange(worksheet, j , i, range); break;
                                        case 13: nummer.Epost = (string)vratiRange(worksheet, j , i, range); break;
                                        case 14: nummer.Tilleggsinfo_ansatt_ID = konvertujUBroj((string)vratiRange(worksheet, j , i, range)); break;
                                        case 15: nummer.Ekstra_talesim_ = (konvertujUBroj((string)vratiRange(worksheet, j , i, range))); break;
                                        case 16: nummer.Ekstra_datasim = konvertujUBroj((string)vratiRange(worksheet, j , i, range)); break;
                                        case 17: nummer.Kostnadsted = (string)vratiRange(worksheet, j , i, range); break;
                                        default:
                                            break;
                                    }
                                   // nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_);
                                   // nummer.Bedrift_som_skal_faktureres = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(nummer.Kostnadsted)).First().Fakturaformat;
                                }
                                //brojac za greske koliko se pojavile 
                                nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_);
                                nummer.Bedrift_som_skal_faktureres = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(nummer.Kostnadsted)).First().Fakturaformat;
                                ProveriNummer(nummer, ref nIspravno, ref nGreske, id_sesije);
                            }
                            ViewData["Ispravno"] = nIspravno;
                            ViewData["Neispravno"] = nGreske;

                            workbook.Close();
                            application.Quit();
                            if (System.IO.File.Exists(fileLocation))
                                System.IO.File.Delete(fileLocation);
                            return View();
                        }
                        else
                        {
                            ViewBag.Error = "Du har valgt feil fil";
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = ex;
                }
                return View();
            }
        }

        [HttpPost]
        public ActionResult Update(string name, string id_sesije)
        {
            List<string> kolone = new List<string>();
            int indbroj=0;
            string Telefonnummer = Request.Form["c1"];
            string Telefonnummer1 = Request.Form["1"];
            if (Telefonnummer == "true,false") kolone.Add(Telefonnummer1);
            else kolone.Add("");

            string Abonnementstype = Request.Form["c2"];
            string Abonnementstype1 = Request.Form["2"];
            if (Abonnementstype == "true,false") kolone.Add(Abonnementstype1);
            else kolone.Add("");

            string Fornavn = Request.Form["c3"];
            string Fornavn1 = Request.Form["3"];
            if (Fornavn == "true,false") kolone.Add(Fornavn1);
            else kolone.Add("");

            string Etternavn = Request.Form["c4"];
            string Etternavn1 = Request.Form["4"];
            if (Etternavn == "true,false") kolone.Add(Etternavn1);
            else kolone.Add("");

            string Bedrift_som_skal_faktu = Request.Form["c5"];
            string Bedrift_som_skal_faktu1 = Request.Form["5"];
            if (Bedrift_som_skal_faktu == "true,false") kolone.Add(Bedrift_som_skal_faktu1);
            else kolone.Add("");

            string c_o_adresse_for_SIM_leve = Request.Form["c6"];
            string c_o_adresse_for_SIM_leve1 = Request.Form["6"];
            if (c_o_adresse_for_SIM_leve == "true,false") kolone.Add(c_o_adresse_for_SIM_leve1);
            else kolone.Add("");

            string Gateadresse_SIM_Skal = Request.Form["c7"];
            string Gateadresse_SIM_Skal1 = Request.Form["7"];
            if (Gateadresse_SIM_Skal == "true,false") kolone.Add(Gateadresse_SIM_Skal1);
            else kolone.Add("");

            string Hus_nummer = Request.Form["c8"];
            string Hus_nummer1 = Request.Form["8"];
            if (Hus_nummer == "true,false") kolone.Add(Hus_nummer1);
            else kolone.Add("");

            string Hus_bokstav = Request.Form["c9"];
            string Hus_bokstav1 = Request.Form["9"];
            if (Hus_bokstav == "true,false") kolone.Add(Hus_bokstav1);
            else kolone.Add("");

            string postnr = Request.Form["c10"];
            string postnr1 = Request.Form["10"];
            if (postnr == "true,false") kolone.Add(postnr1);
            else kolone.Add("");

            string Poststed = Request.Form["c11"];
            string Poststed1 = Request.Form["11"];
            if (Poststed == "true,false") kolone.Add(Poststed1);
            else kolone.Add("");

            string Epostforsporingsinfo = Request.Form["c12"];
            string Epostforsporingsinfo1 = Request.Form["12"];
            if (Epostforsporingsinfo == "true,false") kolone.Add(Epostforsporingsinfo1);
            else kolone.Add("");

            string Epost = Request.Form["c13"];
            string Epost1 = Request.Form["13"];
            if (Epost == "true,false") kolone.Add(Epost1);
            else kolone.Add("");

            string TilleggsinfoansattID = Request.Form["c14"];
            string TilleggsinfoansattID1 = Request.Form["14"];
            if (TilleggsinfoansattID == "true,false") kolone.Add(TilleggsinfoansattID1);
            else kolone.Add("");

            string Ekstratalesim = Request.Form["c15"];
            string Ekstratalesim1 = Request.Form["15"];
            if (Ekstratalesim == "true,false") kolone.Add(Ekstratalesim1);
            else kolone.Add("");

            string Ekstradatasim = Request.Form["c16"];
            string Ekstradatasim1 = Request.Form["16"];
            if (Ekstradatasim == "true,false") kolone.Add(Ekstradatasim1);
            else kolone.Add("");

            string Kostnadsted = Request.Form["c17"];
            string Kostnadsted1 = Request.Form["17"];
            if (Kostnadsted == "true,false") kolone.Add(Kostnadsted1);
            else kolone.Add("");

            
            try
            {


                if (name.Length == 0)
                {
                    ViewBag.Error = "Du har ikke valgt noen filer";
                    return View();

                }
                else
                {

                    if (name.EndsWith(".xls") || name.EndsWith(".xlsx"))
                    {
                        string fileLocation = Server.MapPath("~/Content/" + name);
                        Excel.Application application = new Excel.Application();
                        Excel.Workbook workbook = application.Workbooks.Open(fileLocation);
                        Excel.Worksheet worksheet = workbook.ActiveSheet;
                        //Excel.Range range = worksheet.UsedRange;
                        List<Nummer> nIspravno = new List<Nummer>();
                        List<Nummer> nGreske = new List<Nummer>();
                        bool tacno = true;
                        List<int> indexi = new List<int>();
                        Excel.Range range1 = worksheet.UsedRange;
                        
                        Microsoft.Office.Interop.Excel.Range range = null;
                        string telNummer = (string)vratiRange(worksheet, 1, 1, range);
                        for (int i = 1; i <= range1.Columns.Count; i++)
                        {
                            string r = (string)vratiRange(worksheet, i, 1, range);
                            for (int j = 0; j < kolone.Count(); j++)
                            {
                                if (kolone[j] == telNummer)
                                {
                                    indbroj = j + 1;
                                }
                                if (kolone[j] == r&&r!="")
                                {
                                    indexi.Add(j + 1);
                                }                                    
                            }
                        }

                        for (int i = 2; i <= range1.Rows.Count; i++)
                        {

                            Nummer nummer = new Nummer();
                                for (int j = 0; j < range1.Columns.Count; j++)
                                {

                                    switch (indexi[j])
                                    {
                                        case 1:nummer.Telefonnummer = (string)vratiRange(worksheet, j + 1, i, range);break;
                                        case 2: nummer.Abonnementstype = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 3: nummer.Fornavn = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 4: nummer.Etternavn = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 5: nummer.Bedrift_som_skal_faktureres = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 6: nummer.c_o_adresse_for_SIM_levering = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 8: nummer.Hus_nummer = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 9: nummer.Hus_bokstav = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 10: nummer.post_nr_ = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 11: nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_); break;
                                        case 12: nummer.Epost_for_sporings_informasjon = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 13: nummer.Epost = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 14: nummer.Tilleggsinfo_ansatt_ID = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 15: nummer.Ekstra_talesim_ = (konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range))); break;
                                        case 16: nummer.Ekstra_datasim = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 17: nummer.Kostnadsted = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        default:
                                            break;
                                    }
                                }

                            //brojac za greske koliko se pojavile 
                            if (indbroj != 0)
                            {

                                Nummer n = vratiBroj(nummer.Telefonnummer);

                                for (int p = 0; p < range1.Columns.Count; p++)
                                {
                                    switch (indexi[p])
                                    {
                                        case 1: n.Telefonnummer = nummer.Telefonnummer; break;
                                        case 2: n.Abonnementstype = nummer.Abonnementstype; break;
                                        case 3: n.Fornavn = nummer.Fornavn ; break;
                                        case 4: n.Etternavn = nummer.Etternavn; break;
                                        case 5: n.Bedrift_som_skal_faktureres = nummer.Bedrift_som_skal_faktureres; break;
                                        case 6: n.c_o_adresse_for_SIM_levering = nummer.c_o_adresse_for_SIM_levering; break;
                                        case 7: n.Gateadresse_SIM_Skal_sendes_til = nummer.Gateadresse_SIM_Skal_sendes_til; break;
                                        case 8: n.Hus_nummer = nummer.Hus_nummer; break;
                                        case 9: n.Hus_bokstav = nummer.Hus_bokstav; break;
                                        case 10: n.post_nr_ = nummer.post_nr_; break;
                                        case 11: n.Post_sted = nummer.Post_sted; break;
                                        case 12: n.Epost_for_sporings_informasjon = nummer.Epost_for_sporings_informasjon; break;
                                        case 13: n.Epost = nummer.Epost; break;
                                        case 14: n.Tilleggsinfo_ansatt_ID = nummer.Tilleggsinfo_ansatt_ID; break;
                                        case 15: n.Ekstra_talesim_ = nummer.Ekstra_talesim_; break;
                                        case 16: n.Ekstra_datasim = nummer.Ekstra_datasim; break;
                                        case 17: n.Kostnadsted = nummer.Kostnadsted; break;
                                        default:
                                            break;
                                    }
                                }
                                deleteNummer(n);
                                ProveriNummer(n, ref nIspravno, ref nGreske, id_sesije);
                            }
                        }
                        ViewData["Ispravno"] = nIspravno;
                        ViewData["Neispravno"] = nGreske;

                        workbook.Close();
                        application.Quit();
                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Du har valgt feil fil";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex;
            }
            return View();
        }
        #endregion

        #region provera
        public void deleteNummer(Nummer n)
        {
            Nummer nummer = db.Nummers.Where(s => s.Telefonnummer.Contains(n.Telefonnummer)).First();
            db.Nummers.Remove(nummer);
            db.SaveChanges();
        }
        public void ProveriNummer(Nummer n,ref List<Nummer> nIspravno,ref List<Nummer> nGreske,string id_sesije)
        {
                string tip = null;
                bool f = false;
                tip=ProveriBroj(n.Telefonnummer, ref f);
                ProveriFakture(n.Bedrift_som_skal_faktureres, ref f);
                Proveri_Data_sim(Convert.ToInt32(n.Ekstra_datasim), ref f);
                Proveri_Ekstra_talesim_(Convert.ToInt32(n.Ekstra_talesim_), ref f);
                Proveri_Abonnementstype(n.Abonnementstype, ref f,id_sesije,tip);
                ProveriKonstasned(n.Kostnadsted, ref f,id_sesije);

                if(f)
                {
                    nGreske.Add(n);
                }
                else
                {
                    AddNummer(n, id_sesije);
                    nIspravno.Add(n);
                }
            
        }

        [HttpPost]
        public void AddNummer(Nummer ispravno,string id_sesije)
        {
            ispravno.Orgnummer = id_sesije;
            ispravno.HovedSIM = 42;
            //ispravno.Kostnadsted = "Faktura";
            db.Nummers.Add(ispravno);
            db.SaveChanges();
        }

        public void ProveriFakture(string broj, ref bool f)
        {
            if (broj != null)
                if (broj == "EHF" || broj == "Epost" || broj == "Papirfaktura") ;
                else
                {
                    f = true;
                }
            else
            {
                broj = "";
            }
        }

        public void Proveri_Abonnementstype(string broj, ref bool f,string id_sesije, string tip)
        {
            int i = Convert.ToInt32(id_sesije);
            var c = db.Clients.Where(s => s.Id == i).First();
            bool flag = true;
            if (tip != null)
            {
                var a = db.Abonementypes.Where(s => s.Id == c.Id_abonementype && s.Num_type == tip);
                foreach (var item3 in a)
                {
                    var con = db.ConnectionTypes.Where(s => s.Id_abom == item3.Id);
                    foreach (var item in con)
                    {
                        var t = db.Types.Where(s => s.Id == item.Id_type);
                        foreach (var item1 in t)
                        {
                            if (item1.Name == broj)
                            {
                                flag = false;
                            }
                        }

                    }
                }
                if (flag) f = true;
            }
            else f = true;
        }

        public string ProveriBroj(string broj,ref bool f)
        {
            string t = null;
            bool flag = true;
            var b = db.Nummers.Where(s => s.Telefonnummer.Contains(broj));


            if (broj.Length == 8)
            {
                flag = false;
                if (broj.Substring(0, 1) == "4" || broj.Substring(0, 1) == "9")
                {
                    t = "M";
                }
                else
                {
                    t = "F";
                }
            }

            else if (broj.Length == 5 && broj.Substring(0, 1) == "5") { flag = false; t = "F"; }
            else if (broj.Length == 12)
                if (broj.Substring(0, 1) == "5" && broj.Substring(1, 1) == "8" && broj.Substring(2, 1) == "0")
                { flag = false; t = "I"; }
            
            if (b.Count()!=0||flag == true) f = true;
            return t;
        }

        public void Proveri_Ekstra_talesim_(int talesim,ref bool f)
        {
            if (talesim>=0 && talesim<=2);
            else
            {
                f = true;
            }
        }

        public void Proveri_Data_sim(int broj, ref bool f)
        {
            if (broj >= 0 && broj <= 5) ;
            else
            {
                f = true;
            }
        }

        public void ProveriKonstasned(string broj, ref bool f,string sesija)
        {
            bool flag = true;
            int i = Convert.ToInt32(sesija);
            var fak = db.Fakturaoppsetts.Where(s => s.Id_client == i);
            foreach (var item in fak)
            {
                if (item.NavnPaKostnadssted == broj)
                    flag = false;
            }
            if (flag) f = true;
        }
        #endregion
        
    }
}
