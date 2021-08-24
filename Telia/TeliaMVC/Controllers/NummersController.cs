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
            ViewBag.ORG = client.Id.ToString();
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
            var idc = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));
            if (ModelState.IsValid)
             {
               
                db.Nummers.Add(nummer);
                db.SaveChanges();
                return RedirectToAction("Index","Nummers", new {id_sesije = idc.Id });
            }
            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }
                
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            Client client = db.Clients.Find(idc.Id);
            ViewBag.Types = FillAbonementtypeSelectBox(client.Id_abonementype);
            ViewBag.ORG =idc.Id.ToString();
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


            var c = db.Clients.Find(Convert.ToInt32(GetId(nummer.Orgnummer)));
            ViewBag.Kostnadsted = new SelectList(db.Fakturaoppsetts, "Kostnadssted", "NavnPaKostnadssted", nummer.Kostnadsted);
            ViewBag.Types = FillAbonementtypeSelectBox(c.Id_abonementype); // selectbox za abonementype
            ViewBag.ID = GetId(nummer.Orgnummer);
            ViewBag.Orgnummer = nummer.Orgnummer;
            return View(nummer);
        }

        // POST: Nummers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Telefonnummer,Abonnementstype,Fornavn,Etternavn,Bedrift_som_skal_faktureres,c_o_adresse_for_SIM_levering,Gateadresse_SIM_Skal_sendes_til,Hus_nummer,Hus_bokstav,post_nr_,Post_sted,Epost_for_sporings_informasjon,Epost,Kostnadsted,Tilleggsinfo_ansatt_ID,Ekstra_talesim_,Ekstra_datasim,Orgnummer,HovedSIM")] Nummer nummer, string selected)
        {
            Client c = db.Clients.Find(Convert.ToInt32(nummer.Orgnummer));

            ViewBag.ID = nummer.Orgnummer ;
            nummer.Abonnementstype = selected;
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
            var c = db.Clients.Find(Convert.ToInt32(orgNummer));
            if (c == null)
            {
                return "";
            }
            else
                return c.Id.ToString();
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


      
        #region excel

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase excelfile)
        {
            if (excelfile.ContentLength == 0)
            {
                ViewBag.Error = "ijfhguihriughwie";
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
                    //Excel.Range range = worksheet.UsedRange;
                    List<Nummer> nIpspravno= new List<Nummer>();
                    List<Nummer> nGreske = new List<Nummer>();

                    Excel.Range range1 = worksheet.UsedRange;
                    for (int i = 2; i <= range1.Rows.Count; i++)
                    {
                        Nummer nummer = new Nummer();
                        int greske = 0;
                        bool flag = false;
                        for (int j = 1; j <= range1.Columns.Count; j++)
                        {
                            Microsoft.Office.Interop.Excel.Range range = null;
                           
                            switch (j)
                            {
                                case 1: nummer.Telefonnummer = ProveriBroj((string)vratiRange(worksheet, j, i, range), ref flag); break;
                                case 2: nummer.Abonnementstype =(string) vratiRange(worksheet, j, i, range); break;
                                case 3: nummer.Fornavn = (string)vratiRange(worksheet, j, i, range); break;
                                case 4: nummer.Etternavn = (string)vratiRange(worksheet, j, i, range); break;
                                case 5: nummer.Bedrift_som_skal_faktureres = (string)vratiRange(worksheet, j, i, range); break;
                                case 6: nummer.c_o_adresse_for_SIM_levering = (string)vratiRange(worksheet, j, i, range); break;
                                case 7: nummer.Gateadresse_SIM_Skal_sendes_til = (string)vratiRange(worksheet, j, i, range); break;
                                case 8: nummer.Hus_nummer = Convert.ToInt32((string)vratiRange(worksheet, j, i, range)); break;
                                case 9: nummer.Hus_bokstav = (string)vratiRange(worksheet, j, i, range); break;
                                case 10: nummer.post_nr_ = Convert.ToInt32((string)vratiRange(worksheet, j, i, range)); break;
                                case 11: nummer.Post_sted = (string)vratiRange(worksheet, j, i, range); break;
                                case 12: nummer.Epost_for_sporings_informasjon = (string)vratiRange(worksheet, j, i, range); break;
                                case 13: nummer.Epost = (string)vratiRange(worksheet, j, i, range); break;
                                case 14: nummer.Tilleggsinfo_ansatt_ID = Convert.ToInt32((string)vratiRange(worksheet, j, i, range)); break;
                                case 15: nummer.Ekstra_talesim_ = Proveri_Ekstra_talesim_(Convert.ToInt32((string)vratiRange(worksheet, j, i, range)), ref flag); break;
                                case 16: nummer.Ekstra_datasim = Proveri_Data_sim(Convert.ToInt32((string)vratiRange(worksheet, j, i, range)),ref flag); break;
                                case 17: nummer.Kostnadsted = (string)vratiRange(worksheet, j, i, range); break;

                                default:
                                    break;
                            }
                        }
                        //brojac za greske koliko se pojavile 
                        if (flag)
                        {
                            nGreske.Add(nummer);
                        }
                        else
                        {
                            nIpspravno.Add(nummer);
                        }

                    }
                    ViewData["Ispravno"] = nIpspravno;
                    ViewData["Neispravno"] = nGreske;
                    workbook.Close();
                    return View();
                }
                else
                {
                    ViewBag.Error = "oafejg";
                    return View();
                }


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

        #endregion

        #region provera

        public string ProveriBroj(string broj,ref bool f)
        {
            string i= broj.Substring(0, 1);
            if (broj.Length == 8 && i == "4" || i == "9")
                return broj;
            else
            {
                f = true;
                return broj;
            }
            
        }

        public int Proveri_Ekstra_talesim_(int talesim,ref bool f)
        {
            if (talesim>=0 && talesim<=2)
                return talesim;
            else
            {
                f = true;
                return talesim;
            }
        }

        public int Proveri_Data_sim(int broj, ref bool f)
        {
            if (broj>=0&&broj<=5)
                return broj;
            else
            {
                f = true;
                return broj;
            }
        }

        #endregion
    }
}
