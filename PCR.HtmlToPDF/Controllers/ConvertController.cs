using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Net.Http.Headers;

using SelectPdf;

namespace PCR.HtmlToPDF.Controllers
{
    public class ConvertController : ApiController
    {
        /// <summary>
        /// This function is used to read the HTML website url from config file
        /// Set the margins for PDF
        /// Set header section values
        /// Set footer section values
        /// Read HTML from URL and convert it to PDF
        /// Prepare response message 
        /// </summary>
        /// <returns>Returns the PDF file</returns>
        [HttpGet]
        public HttpResponseMessage HtmlToPDF()
        {
            try
            {
                string pdfFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Content\test.pdf"; 

                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 20;

                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 15;

                // Set the sample data in header
                PdfTextSection headerText = new PdfTextSection(4, 0, "Cloud PCR API", new Font("Arial", 12));
                headerText.HorizontalAlign = PdfTextHorizontalAlign.Left;
                headerText.VerticalAlign = PdfTextVerticalAlign.Middle;
                converter.Header.Add(headerText);

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 20;

                // Set the current datetime in footer
                PdfTextSection footerText = new PdfTextSection(4, 0, DateTime.Now.ToString("dd-MM-yyyy HH:mm"), new Font("Arial", 8));
                footerText.HorizontalAlign = PdfTextHorizontalAlign.Left;
                footerText.VerticalAlign = PdfTextVerticalAlign.Middle;
                converter.Footer.Add(footerText);

                // Set the page numbers in footer
                footerText = new PdfTextSection(4, 0, "Page: {page_number} of {total_pages}  ", new Font("Arial", 8));
                footerText.HorizontalAlign = PdfTextHorizontalAlign.Right;
                footerText.VerticalAlign = PdfTextVerticalAlign.Middle;
                converter.Footer.Add(footerText);

                //Retrieve the HTML webpage url and convert it to pdf
                PdfDocument resultDocument = converter.ConvertUrl(ConfigurationManager.AppSettings["WebsiteURL"]);
                resultDocument.Save(pdfFilePath);
                resultDocument.Close();

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(pdfFilePath, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(pdfFilePath);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentLength = stream.Length;
                return result;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message.ToString());
                return response;
            }
        }
    }
}
