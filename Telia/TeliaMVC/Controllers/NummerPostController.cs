using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeliaMVC.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Web;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace TeliaMVC.Controllers
{
    public class NummerPostController : Controller
    {
        private TeliaEntities db = new TeliaEntities();
        // GET: PostNummers
        // GET: NummerPost
        public ActionResult Index()
        {
            var post = db.Postnummers;
            
            return View(post);
        }
        public ActionResult DeleteAll()
        {
            var postnummer = db.Postnummers;
            foreach (var item in postnummer)
            {
                db.Postnummers.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "NummerPost");
        }
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
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
                        Excel.Range range = worksheet.UsedRange;
                        for (int i = 2; i <= range.Rows.Count; i++)
                        {
                            Postnummer nummer = new Postnummer();
                            for (int j = 1; j <= range.Columns.Count; j++)
                            {
                                switch (j)
                                {
                                    case 1: nummer.PostNr = (string)vratiRange(worksheet, j, i, range); break;
                                    case 2: nummer.Poststed = (string)vratiRange(worksheet, j, i, range); break;
                                    case 3: nummer.Kommunenummer = (string)vratiRange(worksheet, j, i, range); break;
                                    case 4: nummer.Kommunenavn = (string)vratiRange(worksheet, j, i, range); break;
                                    case 5: nummer.Kategory = (string)vratiRange(worksheet, j, i, range); break;
                                    default:
                                        break;
                                }
                            }

                            db.Postnummers.Add(proveri(nummer));
                            db.SaveChanges();
                        }
                        workbook.Close();
                        application.Quit();
                        ViewBag.Error = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                return View();
            }
            return RedirectToAction("Index", "NummerPost");
        }
        public Postnummer proveri(Postnummer n)
        {
            var b = db.Postnummers.Where(s => s.PostNr == n.PostNr);
            if (b.Count() == 0)
                return n;
            else return null;
        }
        public Object vratiRange(Excel.Worksheet worksheet, int i, int j, Excel.Range range)
        {
            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[j, i];
            if (range.Value == null)
            {
                return "";
            }
            else
            {
                return range.Value.ToString();
            }
        }
    }

}