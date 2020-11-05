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
    public class ApiController : Controller
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

        [Route("api/downloadfile")]
        [HttpGet]
        public IActionResult downloadFile(int table, int fileId)
        {
            DataTable dt = new DataTable();
            //save from booklets table
            if (table == 0)
            {
                dt = Functions.GetDataTableFromSQL($"SELECT [name], [file] FROM [booklets] where id={fileId}");
            }
            //save from pages table
            else if (table == 1)
            {
                dt = Functions.GetDataTableFromSQL($"SELECT [name], [file] FROM [pages] where id={fileId}");
            }
            if (dt.Rows.Count > 0 && dt.Rows[0][1] != DBNull.Value)
            {
                string base64str = System.Text.Encoding.ASCII.GetString((byte[])dt.Rows[0][1]);
                byte[] byteArr = Convert.FromBase64String(base64str);
                Stream s = new MemoryStream(byteArr);
                string filename = dt.Rows[0][0].ToString() + ".pdf";
                return File(s, "application/octet-stream", filename);
            }
            return null;
        }
    }
}
