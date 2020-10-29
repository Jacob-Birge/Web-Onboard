using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace Web_Onboard.Data
{
    [ApiController]
    public class SaveFileController : Controller
    {
        private static string fullHtml = "";

        [NonAction]
        public IActionResult Index()
        {
            return View();
        }

        [Route("api/buildhtml")]
        [HttpGet]
        public int BuildFullHtml(string html)
        {
            fullHtml += html;
            return 1;
        }

        [Route("api/savecustom")]
        [HttpGet]
        public int SaveCustomPdf(string html, string filename, int companyid)
        {
            //System.Diagnostics.Debug.Print(companyId.ToString());
            int id = -1;
            fullHtml += html;
            SavePdfInDB(filename, companyid);
            fullHtml = "";
            return id;
        }

        private void SavePdfInDB(string filename, int companyid)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(fullHtml);
                doc.Save(ms);
                string fileBase64Data = Convert.ToBase64String(ms.ToArray());
                string command = $"INSERT INTO pages (company_id, custom_page, [name], [file]) VALUES({companyid}, '{true}', '{filename}', CONVERT(VARBINARY(MAX), '{fileBase64Data}'))";
                Functions.GetDataTableFromSQL(command);
                doc.Close();
            }
        }
    }
}
