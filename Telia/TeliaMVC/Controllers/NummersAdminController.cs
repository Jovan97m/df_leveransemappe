using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;
using PagedList;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace TeliaMVC.Controllers
{
    public class NummersAdminController : Controller
    {
        private TeliaEntities db = new TeliaEntities();
        public ActionResult Index(string sortOrder, string currentFilter, string currentSelected, string searchString,string SearchParameter,string selected,string CopyColumn, int? page)
        {
            var nummers = from s in db.Nummers
                          select s;
            //SelectBox
            ViewBag.nummers = FillSelectBoxClients();
            ViewBag.nummerAdd = FillSelectBoxClientsBezAll();
            if (currentSelected!= null  && selected==null)
            {
                selected = currentSelected;
            }
            //Ovde izmeni brojeve koji treba da se prikazu na osnovu selektovanog  
            if (selected != null)
            {
                if (selected != "All")
                {
                    int id = GetId(selected);
                    nummers = nummers.Where(s => s.Orgnummer.Contains(id.ToString()));
                }
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CopyColumn = CopyColumn; // naziv kolone koja se kopira
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
            ViewBag.CurrentSelected = selected;

            //pretrazivanje pre rasporedjivanja:
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (SearchParameter)
                {
                    case "Telefonnummer":
                        nummers = nummers.Where(s => s.Telefonnummer.Contains(searchString));
                        break;
                    case "Fornavn":
                        nummers = nummers.Where(s=> s.Fornavn.Contains(searchString));
                        break;
                    case "Etternavn":
                        nummers = nummers.Where(s => s.Etternavn.Contains(searchString));
                        break;
                    default:
                        break;
                }
            }
            //OrderBy
            nummers = SortList(nummers, sortOrder);

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(nummers.ToPagedList(pageNumber, pageSize));
        }



        #region CREATE
        //Glavni
        [HttpGet]
        public ActionResult Creating(string selected,string Type)
        {
            if (selected == "Choose OrgNummer" || selected == "All" || Type == null)
            {
                ViewBag.nummers = FillSelectBoxClientsBezAll();
                try
                {
                    return RedirectToAction("Index", "NummersAdmin");
                }
                catch (Exception)
                {

                    throw;
                }

            }

            else 
            {
                
                if (Type == "F")
                {
                    //selected nije potreban 
                    Client client = db.Clients.Find(GetId(selected));
                    ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
                    ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonemetypeF, Type); // selectbox za abonementype
                    ViewBag.ORG = client.Id.ToString();
                    ViewBag.tip = Type;
                    ViewData["FirmaNavn"] = getFirmaNavn(selected);
                    //  return RedirectToAction("CreateFixed", "NummersAdmin" , new { sesija = client.Id});
                    return View("CreateFixed");
                }
                else if(Type=="M")
                {
                    Client client = db.Clients.Find(GetId(selected));
                    ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
                    ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype, Type); // selectbox za abonementype
                    ViewBag.ORG = client.Id.ToString();
                    ViewBag.tip = Type;
                    ViewData["FirmaNavn"] = getFirmaNavn(selected);
                    return View("Create");
                }
                else
                {
                    Client client = db.Clients.Find(GetId(selected));
                    ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
                    ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementypeI, Type); // selectbox za abonementype
                    ViewBag.ORG = client.Id.ToString();
                    ViewBag.tip = Type;
                    ViewData["FirmaNavn"] = getFirmaNavn(selected);
                    return View("CreateInternet");
                }
            }
        }
        public ActionResult Create(string selected, string Type)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,ID,Pending,Katalogoppforing,Porteringsdatoog_tid,Binding,Postnummer,Antall_TrillingSIM,allDataSIM,Manuell_Top_up,Sperre_Top_up,Norden,Tale_og_SMS_til_EU,TBN,HovedSIM,TrillingSIM1,TrillingSIM2,DataSIM1,DataSIM2,DataSIM3,DataSIM4,DataSIM5,DeliveryStreetName,DeliveryStreetNumber,DeliveryStreetSuffix,DeliveryCity,DeliveryZIP,DeliveryContractEmail,DeliveryContractCountyCode,DeliveryContractLocalNumber,DeliveryIndividualFirstName,DeliveryIndividualLastName")] Nummer nummer)
        {
            //ovde da proveri broj


            if (ModelState.IsValid)
            {
                nummer.Pending = false; // oznaci da je admin obradio ovu informaciju
                nummer.DeliveryMethodCode = "LETTER";
                nummer.DeliveryCountryCode = "47";

                db.Nummers.Add(nummer);
                try { db.SaveChanges(); }
                catch (Exception) { throw; }
                return RedirectToAction("Index");
            }

            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            ViewBag.Clients = new SelectList(db.Clients, "Clients", "Orgnummer", nummer.Orgnummer);

            

            return View(nummer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creating([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM,Katalogoppforing,Postnummer,Binding,Porteringsdatoog_tid,Antall_TrillingSIM,allDataSIM,Manuell_Top_up,Sperre_Top_up,Norden,Tale_og_SMS_til_EU,TBN,TrillingSIM1,TrillingSIM2,DataSIM1,DataSIM2,DataSIM3,DataSIM4,DataSIM5,DeliveryMethodCode,DeliveryStreetName,DeliveryStreetSuffix,DeliveryCity,DeliveryZIP,DeliveryCountryCode,DeliveryContractEmail,DeliveryContractCountryCode,DeliveryContractLocalNumber,DeliveryIndividualFirstName,DeliveryIndividualLastName")] Nummer nummer, string selected, string kostnadsted,string tip)
        {
            nummer.DeliveryMethodCode = "LETTER";
            nummer.DeliveryCountryCode = "47";
            nummer.Abonnementstype = selected; // ucitaj selektovani
            nummer.Pending = true;
            Fakturaoppsett fakturaoppsett = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(kostnadsted)).FirstOrDefault();
            nummer.Kostnadsted = fakturaoppsett.Kostnadssted;


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
                return RedirectToAction("Index", "NummersAdmin");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype, tip);
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(client.Id);
            ViewBag.ORG = idc.Id.ToString();
            return View(nummer);
        }



        #endregion

        #region EDIT
        public ActionResult Edit(int? id,string selected)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nummer nummer = db.Nummers.Find(id); // nadje broj koji se edituje
            if (nummer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = nummer.Orgnummer;
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(Convert.ToInt32(nummer.Orgnummer));
            var a = db.Abonementypes.Find(getAbonement(nummer.Abonnementstype, Convert.ToInt32(nummer.Orgnummer)));
            ViewBag.Types = FillAbonementtypeSelectBox(getAbonement(nummer.Abonnementstype, Convert.ToInt32(nummer.Orgnummer)), a.Num_type);
            ViewBag.tip = a.Num_type;
            ViewBag.CurrentSelected = selected;
            return View(nummer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,ID,HovedSIM,Orgnummer,Katalogoppforing,Postnummer,Binding,Porteringsdatoog_tid,Antall_TrillingSIM,allDataSIM,Manuell_Top_up,Sperre_Top_up,Norden,Tale_og_SMS_til_EU,TBN,TrillingSIM1,TrillingSIM2,DataSIM1,DataSIM2,DataSIM3,DataSIM4,DataSIM5,DeliveryMethodCode,DeliveryStreetName,DeliveryStreetSuffix,DeliveryCity,DeliveryZIP,DeliveryCountryCode,DeliveryContractEmail,DeliveryContractCountryCode,DeliveryContractLocalNumber,DeliveryIndividualFirstName,DeliveryIndividualLastName")] Nummer nummer, string selected, string kostnadsted, string tip)
        {
            ViewBag.ID = nummer.Orgnummer;
            nummer.Abonnementstype = selected;
            nummer.Kostnadsted = kostnadsted;
            nummer.Pending = true;
            nummer.Date = null;
            string selected2 = nummer.DeliveryCountryCode;
            nummer.DeliveryCountryCode = "47";
            if (ModelState.IsValid)
            {
                db.Entry(nummer).State = EntityState.Modified;
                try { db.SaveChanges(); }
                catch (Exception) { throw; }
                return RedirectToAction("Index","NummersAdmin",new { currentSelected = selected2});
            }
            ViewBag.Kostnadsted = FillKostnadstedSelectBox(Convert.ToInt32(nummer.Orgnummer));
            var a = db.Abonementypes.Find(getAbonement(nummer.Abonnementstype, Convert.ToInt32(nummer.Orgnummer)));
            ViewBag.Types = FillAbonementtypeSelectBox(getAbonement(nummer.Abonnementstype, Convert.ToInt32(nummer.Orgnummer)), a.Num_type);
            return View(nummer);
        }


        public string UpdateColumn(string sortOrder,string currentFilter,string CopyColumn,string currentSelected,int? a)
        {
            var nummers = from s in db.Nummers
                          select s;
            if (currentSelected != "" && currentSelected != null)
            {
                if (currentSelected != "All")
                {
                    int id = GetId(currentSelected);
                    nummers = nummers.Where(s => s.Orgnummer.Contains(id.ToString()));
                }
            }
            //sortiraj
            nummers = SortList(nummers, sortOrder);

            switch (CopyColumn)
            {
                case "Hus_bokstav": ViewBag.Value = nummers.FirstOrDefault().Hus_bokstav; break;
                case "post_nr_": ViewBag.Value = nummers.FirstOrDefault().Postnummer; break;
                case "Hus_nummer": ViewBag.Value = nummers.FirstOrDefault().Hus_nummer; break;
                case "date": ViewBag.Value = nummers.FirstOrDefault().Porteringsdatoog_tid;break;
                case "Fornavn": ViewBag.Value = nummers.FirstOrDefault().Fornavn; break;
                case "Etternavn": ViewBag.Value = nummers.FirstOrDefault().Etternavn; break;
                case "Katalop": ViewBag.Value = nummers.FirstOrDefault().Katalogoppforing; break;
                default:
                    break;
            }
            ViewBag.sort = sortOrder;
            ViewBag.filter = currentFilter;
            ViewBag.Copy = CopyColumn;
            ViewBag.selected = currentSelected;
            return ViewBag.value;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateColumn(string sortOrder, string currentFilter, string CopyColumn, string currentSelected)
        {
            var nummers = from s in db.Nummers
                          select s;
            if (currentSelected != "" || currentSelected!= null)
            {
                if (currentSelected != "All")
                {
                    int id = GetId(currentSelected);
                    nummers = nummers.Where(s => s.Orgnummer.Contains(id.ToString()));
                }
            }
            //sortiraj
            nummers = SortList(nummers, sortOrder);
            foreach (var item in nummers)
            {
                switch (CopyColumn)
                {
                    case "Hus_bokstav": item.Hus_bokstav = nummers.FirstOrDefault().Hus_bokstav;break;
                    case "post_nr_": item.Postnummer = nummers.FirstOrDefault().Postnummer; break;
                    case "Hus_nummer": item.Hus_nummer = nummers.FirstOrDefault().Hus_nummer; break;
                    case "date": item.Porteringsdatoog_tid = nummers.FirstOrDefault().Porteringsdatoog_tid; break;
                    case "Fornavn": item.Fornavn = nummers.FirstOrDefault().Fornavn; break;
                    case "Etternavn": item.Etternavn = nummers.FirstOrDefault().Etternavn; break;
                    case "Katalop": item.Katalogoppforing = nummers.FirstOrDefault().Katalogoppforing; break;
                    default:
                        break;
                }
                db.Entry(item).State = EntityState.Modified;   
            }
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

            return RedirectToAction("Index","NummersAdmin", new { sortOrder = sortOrder,currentFilter = currentFilter , currentSelected = currentSelected });
        }
        #endregion

        #region DELETE + Details
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nummer nummer = db.Nummers.Find(id);
            db.Nummers.Remove(nummer);
            try { db.SaveChanges(); }
            catch (Exception) { throw; }
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

        #endregion

        #region excel

        [HttpPost]
        public ActionResult Excel(HttpPostedFileBase excelfile,string id_sesije)
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
            if (id_sesije == "Choose OrgNummer" || id_sesije == "All" || id_sesije.Length != 9)
            {
                ViewBag.nummers = FillSelectBoxClientsBezAll();
                try
                {
                    return RedirectToAction("Index", "NummersAdmin");
                }
                catch (Exception)
                {

                    throw;
                }

            }

            else
            {

                var client = db.Clients.Where(s => s.Orgnummer.Contains(id_sesije)).First();
                ViewBag.Clijent = client.Id.ToString();
            }
            return View();
        }
        
        public Object vratiRange(Excel.Worksheet worksheet, int i, int j, Excel.Range range)
        {
            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[j, i];
            return range.Value.ToString();
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
                                        case 5: break;
                                        case 6: nummer.c_o_adresse_for_SIM_levering = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 8: nummer.Hus_nummer = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 9: nummer.Hus_bokstav = (string)vratiRange(worksheet, j + 1, i, range); break;
                                        case 10: nummer.post_nr_ = konvertujUBroj((string)vratiRange(worksheet, j + 1, i, range)); break;
                                        case 11: break;
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
                                string id = db.Clients.Where(s => s.Orgnummer.Contains(id_sesije)).First().Id.ToString();
                                //brojac za greske koliko se pojavile 
                                nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_);
                                nummer.Bedrift_som_skal_faktureres = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(nummer.Kostnadsted)).First().Fakturaformat;
                                ProveriNummer(nummer, ref nIspravno, ref nGreske, id);
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

                                        case 1: nummer.Telefonnummer = (string)vratiRange(worksheet, j, i, range); break;
                                        case 2: nummer.Abonnementstype = (string)vratiRange(worksheet, j, i, range); break;
                                        case 3: nummer.Fornavn = (string)vratiRange(worksheet, j, i, range); break;
                                        case 4: nummer.Etternavn = (string)vratiRange(worksheet, j, i, range); break;
                                        case 5: break;
                                        case 6: nummer.c_o_adresse_for_SIM_levering = (string)vratiRange(worksheet, j, i, range); break;
                                        case 7: nummer.Gateadresse_SIM_Skal_sendes_til = (string)vratiRange(worksheet, j, i, range); break;
                                        case 8: nummer.Hus_nummer = konvertujUBroj((string)vratiRange(worksheet, j, i, range)); break;
                                        case 9: nummer.Hus_bokstav = (string)vratiRange(worksheet, j, i, range); break;
                                        case 10: nummer.post_nr_ = konvertujUBroj((string)vratiRange(worksheet, j, i, range)); break;
                                        case 11: break;
                                        case 12: nummer.Epost_for_sporings_informasjon = (string)vratiRange(worksheet, j, i, range); break;
                                        case 13: nummer.Epost = (string)vratiRange(worksheet, j, i, range); break;
                                        case 14: nummer.Tilleggsinfo_ansatt_ID = konvertujUBroj((string)vratiRange(worksheet, j, i, range)); break;
                                        case 15: nummer.Ekstra_talesim_ = (konvertujUBroj((string)vratiRange(worksheet, j, i, range))); break;
                                        case 16: nummer.Ekstra_datasim = konvertujUBroj((string)vratiRange(worksheet, j, i, range)); break;
                                        case 17: nummer.Kostnadsted = (string)vratiRange(worksheet, j, i, range); break;
                                        default:
                                            break;
                                    }
                                    // nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_);
                                    // nummer.Bedrift_som_skal_faktureres = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(nummer.Kostnadsted)).First().Fakturaformat;
                                }
                                //brojac za greske koliko se pojavile//
                                string id = db.Clients.Where(s => s.Orgnummer.Contains(id_sesije)).First().Id.ToString();
                                //brojac za greske koliko se pojavile 
                                nummer.Post_sted = (string)VratiPostSted(nummer.post_nr_);
                                nummer.Bedrift_som_skal_faktureres = db.Fakturaoppsetts.Where(s => s.NavnPaKostnadssted.Contains(nummer.Kostnadsted)).First().Fakturaformat;
                                ProveriNummer(nummer, ref nIspravno, ref nGreske, id);
                            }
                            

                            workbook.Close();
                            application.Quit();
                            if (System.IO.File.Exists(fileLocation))
                                System.IO.File.Delete(fileLocation);
                            ViewData["Ispravno"] = nIspravno;
                            ViewData["Neispravno"] = nGreske;
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
            int indbroj = 0;
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
                                        case 3: n.Fornavn = nummer.Fornavn; break;
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
        public void ProveriNummer(Nummer n, ref List<Nummer> nIspravno, ref List<Nummer> nGreske, string id_sesije)
        {
            string tip = null;
            bool f = false;
            tip = ProveriBroj(n.Telefonnummer, ref f);
            ProveriFakture(n.Bedrift_som_skal_faktureres, ref f);
            Proveri_Data_sim(Convert.ToInt32(n.Ekstra_datasim), ref f);
            Proveri_Ekstra_talesim_(Convert.ToInt32(n.Ekstra_talesim_), ref f);
            Proveri_Abonnementstype(n.Abonnementstype, ref f, id_sesije, tip);
            ProveriKonstasned(n.Kostnadsted, ref f, id_sesije);

            if (f)
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
        public void AddNummer(Nummer ispravno, string id_sesije)
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
                    return db.Postnummers.Where(s => s.PostNr.Contains(numm.ToString())).First().Poststed;

                }
            }
            catch
            {
                return "";
            }
        }
        public void Proveri_Abonnementstype(string broj, ref bool f, string id_sesije, string tip)
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

        public string ProveriBroj(string broj, ref bool f)
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

            if (b.Count() != 0 || flag == true) f = true;
            return t;
        }

        public void Proveri_Ekstra_talesim_(int talesim, ref bool f)
        {
            if (talesim >= 0 && talesim <= 2) ;
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

        public void ProveriKonstasned(string broj, ref bool f, string sesija)
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
        #endregion
        #region pomocne funkcije
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
        public int getAbonement(string abonement,int idClient)
        {
            int id = getIdType(abonement);
            return (int)db.ConnectionTypes.Where(s => s.Id_type == id).FirstOrDefault().Id_abom;
        }
        public int getIdType(string test)
        {
            var c = db.Types.Where(s => s.Name.Contains(test));
            if (c == null)
            {
                return 0;
            }
            else
                return c.FirstOrDefault().Id;
        }
        public List<String> FillAbonementtypeSelectBox(int id, string type) // selectbox za abonementypes
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
        public List<String> FillKostnadstedSelectBox(int id)
        {
            List<String> povratna = new List<String>();
            var test = db.Fakturaoppsetts.Where(s => s.Id_client == id);
            foreach (var item in test)
            {
                povratna.Add(item.NavnPaKostnadssted);
            }
            return povratna;
        }
        public System.Linq.IQueryable<TeliaMVC.Models.Nummer> SortList(System.Linq.IQueryable<TeliaMVC.Models.Nummer> list, string sortOrder)
        {
            var nummers = list;
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
                case "c_o_adresse_for_SIM_levering":
                    nummers = nummers.OrderBy(s => s.c_o_adresse_for_SIM_levering);
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
            return nummers;
        }

        //funkcija za popunjavanje selectBox-a sa svim klijentima
        public List<String> FillSelectBoxClients()
        {
            List<String> orgNummers = new List<String>();
            orgNummers.Add("All");
            foreach (var item in db.Clients.ToList())
            {
                //kad se doda u bazu
                //string final = item.Orgnummer + "-" + item.FirmaNavn;
                orgNummers.Add(item.Orgnummer);
            }
            return orgNummers;
        }
        public List<String> FillSelectBoxClientsBezAll()
        {
            List<String> orgNummers = new List<String>();
            foreach (var item in db.Clients.ToList())
            {
                //kad se doda u bazu
                //string final = item.Orgnummer + "-" + item.FirmaNavn;
                orgNummers.Add(item.Orgnummer);
            }
            return orgNummers;
        }

        public int GetId(string orgNummer)
        {
            var c = db.Clients.Where(s => s.Orgnummer.Contains(orgNummer));
            if (c == null)
            {
                return 0;
            }
            else
                return c.FirstOrDefault().Id;
        }

        public string getFirmaNavn(string selected)
        {
            
            Client klijent = db.Clients.Find(GetId(selected));
            return klijent.FirmaNavn;
        }
        #endregion
    }
}
