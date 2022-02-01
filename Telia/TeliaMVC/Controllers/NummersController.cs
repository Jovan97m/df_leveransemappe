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
using NPOI.SS.Formula.Functions;

namespace TeliaMVC.Controllers
{
    public class NummersController : Controller
    {
        private TeliaEntities db = new TeliaEntities();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString,int? id, int? page)
        {
            var nummers = from s in db.Nummers
                          select s;
            Client client = db.Clients.Find(id);
            //formiraj listu za odredjenog klijenta
            nummers = nummers.Where(s => s.Orgnummer.Contains(client.Id.ToString()));
            ViewBag.ID = id;
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
                case "telefonnummer_desc":
                    nummers = nummers.OrderByDescending(s => s.Telefonnummer);
                    break;

                case "Abonnementstype":
                    nummers = nummers.OrderBy(s => s.Abonnementstype);
                    break;
                case "abonnementstype_desc":
                    nummers = nummers.OrderByDescending(s => s.Abonnementstype);
                    break;

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

                case "c_o_adresse_for_SIM_levering":
                    nummers =nummers.OrderBy(s => s.c_o_adresse_for_SIM_levering);
                    break;
                case "c_o_adresse_for_SIM_levering_desc":
                    nummers = nummers.OrderByDescending(s => s.c_o_adresse_for_SIM_levering);
                    break;

                case "Gateadresse_SIM_Skal_sendes_til":
                    nummers = nummers.OrderBy(s => s.Gateadresse_SIM_Skal_sendes_til);
                    break;

                case "gateadresse_SIM_Skal_sendes_til_desc":
                    nummers = nummers.OrderByDescending(s => s.Gateadresse_SIM_Skal_sendes_til);
                    break;

                case "husnr_desc":
                    nummers = nummers.OrderByDescending(s => s.Gateadresse_SIM_Skal_sendes_til);
                    break;

                case "Hus_nummer":
                    nummers = nummers.OrderBy(s => s.Hus_nummer);
                    break;
                case "hus_nummer_desc":
                    nummers = nummers.OrderByDescending(s => s.Hus_nummer);
                    break;

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
            ViewData["FirmaNavn"] = getFirmaNavn(id);
            return View(nummers.ToPagedList(pageNumber, pageSize));
        }

        #region CREATE
        public ActionResult Create(int? id)
        {
            Client client = db.Clients.Find(id);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype,"M"); // selectbox za abonementype
            ViewBag.ID = client.Id;
            ViewBag.Mobile = client.Id_abonementype;
            ViewBag.Fixed = client.Id_abonemetypeF;
            ViewBag.Internet = client.Id_abonementypeI;
            ViewData["PostNummer"] = getPostnummers();

            return View();
        }

        public ActionResult CreateFixed(int? id)
        {
            Client client = db.Clients.Find(id);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonemetypeF,"F"); // selectbox za abonementype
            ViewBag.ID = client.Id;
            ViewBag.Mobile = client.Id_abonementype;
            ViewBag.Fixed = client.Id_abonemetypeF;
            ViewBag.Internet = client.Id_abonementypeI;
            ViewData["PostNummer"] = getPostnummers();

            return View();
        }

        public ActionResult CreateInternet(int? id)
        {
            Client client = db.Clients.Find(id);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementypeI,"I"); // selectbox za abonementype
            ViewBag.ID = client.Id;
            ViewBag.Mobile = client.Id_abonementype;
            ViewBag.Fixed = client.Id_abonemetypeF;
            ViewBag.Internet = client.Id_abonementypeI;
            ViewData["PostNummer"] = getPostnummers();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,HovedSIM")] Nummer nummer,string selected,string kostnadsted,int? id)
        {
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            var idc = db.Clients.Find(id);
            nummer.Orgnummer = id.ToString();
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
                return RedirectToAction("Index","Nummers", new {id = idc.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype,"M");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);

            ViewBag.ORG = id;
            return View(nummer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFixed([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,HovedSIM")] Nummer nummer, string selected, string kostnadsted, int? id)
        {
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            var idc = db.Clients.Find(id);
            nummer.Orgnummer = id.ToString();
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
                return RedirectToAction("Index", "Nummers", new { id = idc.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype, "F");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.ID = id;
            return View(nummer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInternet([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,HovedSIM")] Nummer nummer, string selected, string kostnadsted,int? id)
        {
            nummer.Abonnementstype = selected;
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;
            nummer.Post_sted = VratiPostSted(nummer.post_nr_);
            var idc = db.Clients.Find(id);
            nummer.Orgnummer = id.ToString();
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
                return RedirectToAction("Index", "Nummers", new { id = idc.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype, "I");
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.ID = id;
            return View(nummer);
        }

        #endregion

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
            if (nummer.Telefonnummer.StartsWith("4") || nummer.Telefonnummer.StartsWith("9"))
                ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementype, "M");
            else if (nummer.Telefonnummer.StartsWith("58"))
                ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementypeI, "I");
            else
                ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonemetypeF, "F");
            ViewBag.ID = c.Id;
            ViewBag.Orgnummer = nummer.Orgnummer;
            return View(nummer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,HovedSIM")] Nummer nummer, string selected,string kostnadsted,int? id)
        {
            Client c = db.Clients.Find(id);

            ViewBag.ID = id;
            if (nummer.Telefonnummer.StartsWith("4") || nummer.Telefonnummer.StartsWith("9"))
                ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementype, "M");
            else if (nummer.Telefonnummer.StartsWith("58"))
                ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementypeI, "I");
            else
                ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonemetypeF, "F");
            nummer.Abonnementstype = selected;
            nummer.Pending = true;
            nummer.Date = DateTime.Today;
            nummer.Kostnadsted = kostnadsted;
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

                return RedirectToAction("Index", new { id = c.Id });
            }
            
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(c.Id);

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
            var c = db.Clients.Find(Convert.ToInt32(GetId(nummer.Orgnummer)));
            ViewBag.ID = c.Id;

            return View(nummer);
        }

        // POST: Nummers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nummer nummer = db.Nummers.Find(id);
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
            return RedirectToAction("Index", new { id = ViewBag.ID });
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
        public Dictionary<string, string> getPostnummers()
        {
            List<Postnummer> Lista = db.Postnummers.ToList();
            Dictionary<string, string> mapa = new Dictionary<string, string>();
            foreach (var item in Lista)
            {
                mapa.Add(item.PostNr, item.Poststed);
            }
            return mapa;
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
        public ActionResult Ocitaj(HttpPostedFileBase excelfile, string id_sesije)
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
                    if (System.IO.File.Exists(fileLocation))
                        System.IO.File.Delete(fileLocation);
                    excelfile.SaveAs(fileLocation);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(fileLocation);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range1 = worksheet.UsedRange;
                    Microsoft.Office.Interop.Excel.Range range = null;
                    Dictionary<int, List<string>> mapa = new Dictionary<int, List<string>>();
                    int brojac = 0;
                    for (int i = 1; i <= range1.Rows.Count; i++)
                    {
                        List<string> lista = new List<string>();
                        for (int j = 1; j <= range1.Columns.Count; j++)
                        {
                            if (j == 1)
                            {
                                if ((string)vratiRange(worksheet, j, i, range) == "" && i != 1)
                                {
                                    j = range1.Columns.Count;
                                }
                                else
                                    lista.Add((string)vratiRange(worksheet, j, i, range));
                            }
                            else
                                lista.Add((string)vratiRange(worksheet, j, i, range));
                        }
                        mapa.Add(brojac, lista);
                        brojac++;
                    }
                    ViewData["Mapa"] = mapa;
                    workbook.Close();
                    application.Quit();
                    if (System.IO.File.Exists(fileLocation))
                        System.IO.File.Delete(fileLocation);
                }
                ViewData["Naslov"] = getListMobile();
                return View();
            }
        }
        public ActionResult Ocitaj1(HttpPostedFileBase excelfile, string id_sesije)
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
                    if (System.IO.File.Exists(fileLocation))
                        System.IO.File.Delete(fileLocation);
                    excelfile.SaveAs(fileLocation);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(fileLocation);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range1 = worksheet.UsedRange;
                    Microsoft.Office.Interop.Excel.Range range = null;
                    Dictionary<int, List<string>> mapa = new Dictionary<int, List<string>>();
                    int brojac = 0;
                    for (int i = 1; i <= range1.Rows.Count; i++)
                    {
                        List<string> lista = new List<string>();
                        for (int j = 1; j <= range1.Columns.Count; j++)
                        {
                            if (j == 1)
                            {
                                if ((string)vratiRange(worksheet, j, i, range) == "" && i != 1)
                                {
                                    j = range1.Columns.Count;
                                }
                                else
                                    lista.Add((string)vratiRange(worksheet, j, i, range));
                            }
                            else
                                lista.Add((string)vratiRange(worksheet, j, i, range));
                        }
                        mapa.Add(brojac, lista);
                        brojac++;
                    }
                    ViewData["Mapa1"] = mapa;
                    workbook.Close();
                    application.Quit();
                    if (System.IO.File.Exists(fileLocation))
                        System.IO.File.Delete(fileLocation);
                }
                ViewData["Naslov1"] = getListFix();
                return View();
            }
        }
        #endregion
        public List<string> getListFix()
        {
            List<string> list = new List<string>();
            list.Add("SELECT");
            list.Add("Telefonnummer");
            list.Add("Abonnementstype");
            list.Add("Fornavn");
            list.Add("Etternavn");
            list.Add("c_o_adresse_for_SIM_levering");
            list.Add("Gateadresse_SIM_Skal_sendes_til");
            list.Add("Hus_nummer");
            list.Add("Hus_bokstav");
            list.Add("post_nr_");
            list.Add("Epost_for_sporings_informasjon");
            list.Add("Epost");
            list.Add("Kostnadsted");
            list.Add("Tilleggsinfo_ansatt_ID");
            list.Add("Ekstra_talesim_");
            list.Add("Ekstra_datasim");
            return list;
        }
        public List<string> getListMobile()
        {
            List<string> list = new List<string>();
            list.Add("SELECT");
            list.Add("Telefonnummer");
            list.Add("Abonnementstype");
            list.Add("Fornavn");
            list.Add("Etternavn");
            list.Add("Bedrift som skal faktureres");
            list.Add("c/o adresse for SIM levering");
            list.Add("Gateadresse SIM Skal sendes til");
            list.Add("husnummer");
            list.Add("bokstav");
            list.Add("post nr. ");
            list.Add("Post sted");
            list.Add("Epost for sporings informasjon");
            list.Add("Epost");
            list.Add("Tilleggsinfo/ansatt ID");
            list.Add("Ekstra talesim ");
            list.Add("Ekstra datasim");
            list.Add("Kostnadsted");
            return list;
        }

        #region provera
        public void deleteNummer(Nummer n)
        {
            Nummer nummer = db.Nummers.Where(s => s.Telefonnummer.Contains(n.Telefonnummer)).First();
            db.Nummers.Remove(nummer);
            db.SaveChanges();
        }
        public void ProveriNummer(Nummer n,ref List<Nummer> nIspravno,ref List<Nummer> nGreske,string id_sesije)
        {
            if (n.Telefonnummer != "")
            {
                string tip = null;
                bool f = false;
                tip = ProveriBroj(n.Telefonnummer, ref f);
                //ProveriFakture(n.Bedrift_som_skal_faktureres, ref f);
                Proveri_Data_sim(Convert.ToInt32(n.Ekstra_datasim), ref f);
                Proveri_Ekstra_talesim_(Convert.ToInt32(n.Ekstra_talesim_), ref f);
                Proveri_Abonnementstype(n.Abonnementstype, ref f, id_sesije, tip);
                ProveriKonstasned(ref n, ref f, id_sesije);
                ProveriMail(n);
                if (f)
                {
                    nGreske.Add(n);
                }
                else
                {
                   // AddNummer(n, id_sesije);
                    nIspravno.Add(n);
                }
            }
            
        }
        public void ProveriMail(Nummer n)
        {
            if (n.Epost == "")
            {
                n.Epost = null;
            }
            if (n.Epost_for_sporings_informasjon == "")
            {
                n.Epost_for_sporings_informasjon = null;
            }
        }
        [HttpPost]
        public void AddNummer(Nummer ispravno,string id_sesije)
        {
            ispravno.Orgnummer = id_sesije;
            ispravno.HovedSIM = 42;
            //ispravno.Kostnadsted = "Faktura";
            db.Nummers.Add(ispravno);
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

        public void ProveriKonstasned(ref Nummer broj, ref bool f,string sesija)
        {
            bool flag = true;
            int i = Convert.ToInt32(sesija);
            var fak = db.Fakturaoppsetts.Where(s => s.Id_client == i);
            foreach (var item in fak)
            {
                if (item.NavnPaKostnadssted == broj.Kostnadsted)
                    flag = false;
            }
            if (flag)
            {
                f = true;
                
                Fakturaoppsett faktura = new Fakturaoppsett();
                faktura.NavnPaKostnadssted = "samlefaktura";
                
                broj.Kostnadsted = faktura.NavnPaKostnadssted;
            }
        }
        public string getFirmaNavn(int? id)
        {
            Client klijent = db.Clients.Find(id);
            return klijent.FirmaNavn;
        }
        #endregion
        [HttpPost]
        public ActionResult Verify(List<string> mapa)
        {
            List<string> l = getListMobile(); 
            string id = mapa[0];
            int red = konvertujUBroj(mapa[1]);
            int colona = konvertujUBroj(mapa[2]);
            ViewBag.ID = id;
            List<List<string>> brojevi = new List<List<string>>();
            for (int i=colona+3; i < (red - 2) * colona; i++)
            {
                List<string> s = new List<string>();
                for (int y = 0; y < colona; y++)
                {
                    s.Add(mapa[i]);
                    i++;
                }
                i--;
                brojevi.Add(s);
            }
            List<int> raspored = new List<int>();
            for(int i =  + 3; i < colona + 3; i++)
            {
                raspored.Add(konvertujUBroj(mapa[i]));
            }
            List<Nummer> nummers = new List<Nummer>();
            for (int i = 0; i < brojevi.Count(); i++)
            {
                Nummer nummer = new Nummer();
                for (int j = 0; j < colona; j++)
                {

                    switch (raspored[j])
                    {
                        case 1: nummer.Telefonnummer = brojevi[i][j]; break;
                        case 2: nummer.Abonnementstype = brojevi[i][j]; break;
                        case 3: nummer.Fornavn = brojevi[i][j]; break;
                        case 4: nummer.Etternavn = brojevi[i][j]; break;
                        case 5: nummer.Bedrift_som_skal_faktureres = brojevi[i][j]; break;
                        case 6: nummer.c_o_adresse_for_SIM_levering = brojevi[i][j]; break;
                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = brojevi[i][j]; break;
                        case 8: nummer.Hus_nummer =konvertujUBroj( brojevi[i][j]); break;
                        case 9: nummer.Hus_bokstav = brojevi[i][j]; break;
                        case 10: nummer.post_nr_ = konvertujUBroj(brojevi[i][j]); break;
                        case 11: nummer.Post_sted = brojevi[i][j]; break;
                        case 12: nummer.Epost_for_sporings_informasjon = brojevi[i][j]; break;
                        case 13: nummer.Epost = brojevi[i][j]; break;
                        case 14: nummer.Kostnadsted = brojevi[i][j]; break;
                        case 15: nummer.Tilleggsinfo_ansatt_ID = konvertujUBroj(brojevi[i][j]); break;
                        case 16: nummer.Ekstra_talesim_ = konvertujUBroj(brojevi[i][j]); break;
                        case 17: nummer.Ekstra_datasim = konvertujUBroj(brojevi[i][j]); break;
                        default:
                            break;
                    }
                }
                nummers.Add(nummer);
            }
            List<Nummer> Ispravno = new List<Nummer>();
            List<Nummer> Neispravno = new List<Nummer>();
            foreach (var item in nummers)
            {
                ProveriNummer(item, ref Ispravno,ref Neispravno, id);
            }
            ViewData["Dobro"] = Ispravno;
            ViewData["lose"] = Neispravno;
            return View();  
        }
        [HttpPost]
        public ActionResult Verify1(List<string> mapa)
        {
            List<string> l = getListFix();
            string id = mapa[0];
            int red = konvertujUBroj(mapa[1]);
            int colona = konvertujUBroj(mapa[2]);
            ViewBag.ID = id;
            List<List<string>> brojevi = new List<List<string>>();
            for (int i = colona + 3; i < (red - 2) * colona; i++)
            {
                List<string> s = new List<string>();
                for (int y = 0; y < colona; y++)
                {
                    s.Add(mapa[i]);
                    i++;
                }
                i--;
                brojevi.Add(s);
            }
            List<int> raspored = new List<int>();
            for (int i = +3; i < colona + 3; i++)
            {
                raspored.Add(konvertujUBroj(mapa[i]));
            }
            List<Nummer> nummers = new List<Nummer>();
            for (int i = 0; i < brojevi.Count(); i++)
            {
                Nummer nummer = new Nummer();
                for (int j = 0; j < colona; j++)
                {

                    switch (raspored[j])
                    {
                        case 1: nummer.Telefonnummer = brojevi[i][j]; break;
                        case 2: nummer.Abonnementstype = brojevi[i][j]; break;
                        case 3: nummer.Fornavn = brojevi[i][j]; break;
                        case 4: nummer.Etternavn = brojevi[i][j]; break;
                        case 5: nummer.Bedrift_som_skal_faktureres = brojevi[i][j]; break;
                        case 6: nummer.c_o_adresse_for_SIM_levering = brojevi[i][j]; break;
                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = brojevi[i][j]; break;
                        case 8: nummer.Hus_nummer = konvertujUBroj(brojevi[i][j]); break;
                        case 9: nummer.Hus_bokstav = brojevi[i][j]; break;
                        case 10: nummer.post_nr_ = konvertujUBroj(brojevi[i][j]); break;
                        case 11: nummer.Post_sted = brojevi[i][j]; break;
                        case 12: nummer.Epost_for_sporings_informasjon = brojevi[i][j]; break;
                        case 13: nummer.Epost = brojevi[i][j]; break;
                        case 14: nummer.Kostnadsted = brojevi[i][j]; break;
                        case 15: nummer.Tilleggsinfo_ansatt_ID = konvertujUBroj(brojevi[i][j]); break;
                        case 16: nummer.Ekstra_talesim_ = konvertujUBroj(brojevi[i][j]); break;
                        case 17: nummer.Ekstra_datasim = konvertujUBroj(brojevi[i][j]); break;
                        default:
                            break;
                    }
                }
                nummers.Add(nummer);
            }
            List<Nummer> Ispravno = new List<Nummer>();
            List<Nummer> Neispravno = new List<Nummer>();
            foreach (var item in nummers)
            {
                ProveriNummer(item, ref Ispravno, ref Neispravno, id);
            }
            ViewData["Dobro"] = Ispravno;
            ViewData["lose"] = Neispravno;
            return View();
        }
    }
}
