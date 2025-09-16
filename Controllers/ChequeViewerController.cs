using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class ChequeViewerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ChequeViewerController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult test1()
        {
            string sourceFileName = "ChatbotForm.htm";
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", Path.GetFileName(sourceFileName));

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string htmlCode = sr.ReadToEnd();
                // Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                // Spire.Pdf.PdfNewPage page = doc.Pages.Add() as Spire.Pdf.PdfNewPage;

                string htmlText = "Customer Signature : 'sig, req=1' ";
                // Spire.Pdf.Graphics.PdfFont font = new Spire.Pdf.Graphics.PdfFont(Spire.Pdf.Graphics.PdfFontFamily.Helvetica, 10f);
                // Spire.Pdf.Graphics.PdfBrush brush = Spire.Pdf.Graphics.PdfBrushes.Green;

                // Spire.Pdf.HtmlTextElement richTextElement = new Spire.Pdf.HtmlTextElement(htmlText, font, brush);
                // richTextElement.TextAlign = Spire.Pdf.TextAlign.Left;

                // Spire.Pdf.PdfMetafileLayoutFormat format = new Spire.Pdf.PdfMetafileLayoutFormat();
                //format.Layout = PdfLayoutType.Paginate;
                //format.Break = PdfLayoutBreakType.FitPage;

                // richTextElement.Draw(page, new System.Drawing.RectangleF(0, 40, page.GetClientSize().Width, page.GetClientSize().Height / 2), format);
                //doc.SaveToFile("C:\temp\TEST\Output1.pdf");
            }

            return View();
        }

        public IActionResult testamman()
        {
            string sourceFileName = "ChatbotForm.htm";
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", Path.GetFileName(sourceFileName));

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string htmlCode = sr.ReadToEnd();
                // iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
                // iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, memoryStream);
                    // pdfDoc.Open();
                    try
                    {
                        // htmlparser.Parse(new StringReader(htmlCode));
                    }
                    catch (Exception ex)
                    {
                        string ss = "";
                    }

                    // pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    System.IO.File.WriteAllBytes("C:\\temp\\file.pdf", bytes);
                    memoryStream.Close();
                }
            }
            return View();
        }

        public IActionResult PrintAllOutwardCHQ()
        {
            try
            {
                // Placeholder for GetAllCategoriesForTree() - needs implementation
                ViewBag.Tree = new List<object>(); // Or call a method that retrieves categories
            }
            catch (Exception ex)
            {
                // Log exception if necessary
            }
            return View();
        }

        public JsonResult get_SearchList(string Branchs, string Bank, string FromChq, string ToChq, string DrwAccNo, string ToValueDate, string FromValueDate, string BenfAccNo, string Amount, string SRC, string STS)
        {
            string clr = "ALL";

            try
            {
                if (SRC == "0")
                {
                    clr = "Outward";
                }
                else if (SRC == "1")
                {
                    clr = "Inward";
                }
                else if (SRC == "2")
                {
                    clr = "OnUs";
                }
                else if (SRC == "4")
                {
                    clr = "PDC";
                }
            }
            catch (Exception ex)
            {
                // Log exception if necessary
            }

            if (string.IsNullOrEmpty(ToValueDate) || string.IsNullOrEmpty(FromValueDate))
            {
                return Json(new { ErrorMsg = "Please Fill From and To Date", lstPDC = new List<object>() });
            }

            return Json(new { ErrorMsg = "", lstPDC = new List<object>() }); // Dummy return for now
        }

        public IActionResult GetChequeDetails(string Drw_Bank, string Drw_Branch, string Chq_No, string Drw_Acc_No, string Ben_Acc_No, string Input_Date, string TYPE)
        {
            return View(); // Dummy return for now
        }

        public JsonResult getSearchListAllout(string Branchs, string DrwAccNo, string ToInputDate, string FromInputeDate, string CHQSRC, string CLRCNT)
        {
            List<object> list = new List<object>(); // Using object for now, replace with actual model

            if (string.IsNullOrEmpty(DrwAccNo))
            {
                return Json(new { ErrorMsg = "Please Enter Account Number" });
            }

            try
            {
                string tDate = DateTime.Now.ToString("yyyy-MM-dd");

                string _date = ToInputDate.Split('T')[0];
                string _date1 = FromInputeDate.Split('T')[0];

                if (_date != tDate && _date1 != tDate)
                {
                    return Json(new { ErrorMsg = "Date Muste Be Date Of Today" });
                }

                string tDate1 = Convert.ToDateTime(ToInputDate).ToString("yyyyMMdd HH:mm");
                string tDate21 = Convert.ToDateTime(FromInputeDate).ToString("yyyyMMdd HH:mm");

                string sqlQ = "SELECT * FROM ALL_OUTWARD_CHEQUES_VIEW  WHERE waspdc= 0";

                sqlQ += "  AND  CHQ_TYPE ='" + CHQSRC + "'";
                if (CLRCNT != "ALL")
                {
                    sqlQ += "  AND  clrcenter ='" + CLRCNT + "'";
                }

                if (!string.IsNullOrEmpty(Branchs))
                {
                    sqlQ += "  AND  InputBrn ='" + Branchs + "'";
                }

                if (!string.IsNullOrEmpty(DrwAccNo))
                {
                    sqlQ += "  AND  BenAccountNo like'%'" + DrwAccNo + "%'";
                }

                if (!string.IsNullOrEmpty(FromInputeDate) && string.IsNullOrEmpty(ToInputDate))
                {
                    sqlQ = sqlQ + " and inputdate >= '" + tDate21 + "'";
                }
                else if (string.IsNullOrEmpty(FromInputeDate) && !string.IsNullOrEmpty(ToInputDate))
                {
                    sqlQ = sqlQ + " and inputdate <= '" + tDate1 + "'";
                }
                else if (!string.IsNullOrEmpty(FromInputeDate) && !string.IsNullOrEmpty(ToInputDate))
                {
                    sqlQ = sqlQ + " and inputdate >= '" + tDate1 + "' and inputdate <= '" + tDate21 + "'";
                }
                else if (!string.IsNullOrEmpty(FromInputeDate) && !string.IsNullOrEmpty(tDate21) && FromInputeDate == tDate1)
                {
                    sqlQ = sqlQ + " and inputdate = '" + tDate21 + "'";
                }

                return Json(new { ErrorMsg = "", Locked_user = ".", List = list });
            }
            catch (Exception ex)
            {
                return Json(new { ErrorMsg = "Error !" });
            }
        }

        public string GetBranchNameAR(string branchcode)
        {
            string result = "";
            try
            {
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public JsonResult checkAuthchq(string serial, string CLR_CENTER)
        {
            List<string> list = serial.Split(',').ToList();
            List<object> list_CHQ = new List<object>(); // Placeholder for ALL_OUTWARD_CHEQUES_VIEW

            string list_of_serial = string.Join(",", list);
            if (list_of_serial.EndsWith(","))
            {
                list_of_serial = list_of_serial.Substring(0, list_of_serial.Length - 1);
            }

            string xmlTempPDC = "";
            string xmlTempOut = "";

            try
            {
                int count = 0;
                string sql = "";
                sql = " select count(*) as count from ALL_OUTWARD_CHEQUES_VIEW where  auth_status<>'Done' and chqsequance in ( " + list_of_serial + ")";

                if (count > 0)
                {
                    return Json(new { ErrorMsg = "F", MSG = "There Is Some CHQ Need Authrization" });
                }
                else
                {
                    sql = " select * from ALL_OUTWARD_CHEQUES_VIEW where  auth_status='Done' and chqsequance in ( " + list_of_serial + ")";
                }

                if (list_CHQ.Count == 0)
                {
                    return Json(new { ErrorMsg = "F", MSG = "Please  Select  The CHQ " });
                }

                int COUNT = 0;
                string _DueDate = "";
                double Total = 0;

                if (CLR_CENTER == "PDC")
                {
                    try
                    {
                        string _PayBranchName = "";
                        string _drwBranchName = "";
                        string amountInWord = "";

                        string termsOutward = xmlTempPDC;
                        List<string> acclst = new List<string>();
                        foreach (var item in list_CHQ)
                        {
                            // Assuming item has an ISSAccount property
                            // if (!acclst.Contains(item.ISSAccount))
                            // {
                            //     acclst.Add(item.ISSAccount);
                            // }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception
                    }
                }

                return Json(new { ErrorMsg = "", Locked_user = ".", List = list_CHQ });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { ErrorMsg = "Error !" });
            }
        }
    }
}


