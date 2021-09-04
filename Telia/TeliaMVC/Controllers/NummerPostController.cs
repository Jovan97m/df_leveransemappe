﻿using System;
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
        public ActionResult Index()
        {
            var numer = from s in db.Postnummers select s;
            return View(numer.ToList());
        }
        [HttpPost,ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
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
                            if (proveri(nummer) != null)
                            {
                                db.Postnummers.Add(nummer);
                                db.SaveChanges();
                            }
                        }
                        workbook.Close();
                        application.Quit();
                        ViewBag.Error = "Success";
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var error in e.EntityValidationErrors)
                {
                    foreach (var propertyError in error.ValidationErrors)
                    {
                        Console.WriteLine($"{propertyError.PropertyName} had the following issue: {propertyError.ErrorMessage}");
                    }
                }
            }
            return RedirectToAction("Index","NummerPost");
        }
        public Postnummer proveri(Postnummer n)
        {
            var b = db.Postnummers.Where(s => s.PostNr == n.PostNr);
            if (b.Count() ==0)
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