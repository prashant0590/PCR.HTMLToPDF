using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Net.Http.Headers;

using SelectPdf;

using PCR.HtmlToPDF.Models;

using log4net;

namespace PCR.HtmlToPDF.Controllers
{
    public class ConvertController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

                //Set the PDF configurations
                Helper.ConfigurePDFOptions(ref converter);

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

        /// <summary>
        /// This function is used to read HTML as a parameter as well as base url of html application, convert it to PDF and return it as byte array
        /// </summary>
        /// <param name="baseURL">Base URL of HTML application</param>
        /// <param name="htmlString">HTML Code as string</param>
        /// <returns>Returns the byte array of PDF, that was generated from HTML code</returns>
        [HttpPost]
        [Route("api/HTMLCodeToPDF")]
        public HttpResponseMessage HTMLCodeToPDF(Helper helper)
        {
            try
            {
                helper.htmlString = helper.htmlString.Replace("|", "&").Replace("||", "#");

                HtmlToPdf converter = new HtmlToPdf();

                //Set the PDF configurations
                Helper.ConfigurePDFOptions(ref converter);
                
                // create a new pdf document converting an url
                PdfDocument resultDocument = converter.ConvertHtmlString(helper.htmlString, helper.baseURL);

                // save pdf document
                byte[] pdf = resultDocument.Save();

                // close pdf document
                resultDocument.Close();

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(pdf);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                HttpError err = new HttpError(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
                
            }
        }
    }
}
