using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using SelectPdf;

namespace PCR.HtmlToPDF.Models
{
    public class Helper
    {
        /// <summary>
        /// This function is used to set the PDF configuration options
        /// 1. Header Settings
        /// 2. Footer Setings
        /// 3. Font Setting
        /// 4. Date time and page number in footer
        /// </summary>
        /// <param name="converter">Object of the SelectPdf class</param>
        public static void ConfigurePDFOptions(ref HtmlToPdf converter)
        {
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
            PdfTextSection headerText = new PdfTextSection(4, 0, "Cloud PCR", new Font("Arial", 12));
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
        }
    }
}