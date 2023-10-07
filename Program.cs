using PdfiumViewer;
using PdfSharp;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Printer
{
    class Printer
    {
        static string GetDefaultPrinterName()
        {
            PrinterSettings settings = new PrinterSettings();
            var paperSizes = settings.PaperSizes;

            Console.WriteLine("Supported Paper Sizes:");

            foreach (PaperSize paperSize in paperSizes)
            {
                Console.WriteLine($"Paper Name: {paperSize.PaperName}, Kind: {paperSize.Kind}, Width: {paperSize.Width}, Height: {paperSize.Height}");
            }
            string defaultPrinterName = settings.PrinterName;

            return defaultPrinterName;
        }
        static PaperSize GetDefaultPaperSize()
        {
            PrinterSettings settings = new PrinterSettings();
            var paperSizes = settings.PaperSizes;

            Console.WriteLine("Supported Paper Sizes:");

            foreach (PaperSize paperSize in paperSizes)
            {
                Console.WriteLine($"Paper Name: {paperSize.PaperName}, Kind: {paperSize.Kind}, Width: {paperSize.Width}, Height: {paperSize.Height}");
                if (paperSize.PaperName.Contains("A4"))
                {
                    return paperSize;
                }
            }

            return null;
        }

        static List<string> GetPaperNames()
        {
            PrinterSettings settings = new PrinterSettings();
            var paperSizes = settings.PaperSizes;
            var paperNames = new List<string>();

            Console.WriteLine("Supported Paper Sizes:");

            foreach (PaperSize paperSize in paperSizes)
            {
                Console.WriteLine($"Paper Name: {paperSize.PaperName}, Kind: {paperSize.Kind}, Width: {paperSize.Width}, Height: {paperSize.Height}");
                paperNames.Add(paperSize.PaperName);
            }

            return paperNames;
        }

        public bool PrintPDF(string printer, string filename, string paperName = "A4", int copies = 1)
        {
            string defaultPrinterName = GetDefaultPrinterName();
            var defaultPaperSize = GetDefaultPaperSize();
            try
            {
                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = String.IsNullOrEmpty(printer) ? defaultPrinterName : printer,
                    Copies = (short)copies,
                };

                // Create our page settings for the paper size selected
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                    PaperSize = defaultPaperSize,
                };
                //foreach (PaperSize paperSize in printerSettings.PaperSizes)
                //{
                //    if (paperSize.PaperName.Contains(paperName))
                //    {
                //        pageSettings.PaperSize = paperSize;
                //        break;
                //    }
                //}

                // Now print the PDF document
                using (var document = PdfDocument.Load(filename))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool PrintPDF(string printer, Stream stream, string paperName = "A4", int copies = 1)
        {
            string defaultPrinterName = GetDefaultPrinterName();
            var defaultPaperSize = GetDefaultPaperSize();

            try
            {
                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = String.IsNullOrEmpty(printer) ? defaultPrinterName : printer,
                    Copies = (short)copies,
                };

                // Create our page settings for the paper size selected
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                    PaperSize = defaultPaperSize,
                };
                //foreach (PaperSize paperSize in printerSettings.PaperSizes)
                //{
                //    if (paperSize.PaperName == paperName)
                //    {
                //        pageSettings.PaperSize = paperSize;
                //        break;
                //    }
                //}

                // Now print the PDF document
                using (var document = PdfDocument.Load(stream))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        //private PdfSharpAdapter()
        //{
        //    AddFontFamilyMapping("monospace", "Courier New");
        //    AddFontFamilyMapping("Helvetica", "Arial");

        //    var families = new InstalledFontCollection();

        //    foreach (var family in families.Families)
        //    {
        //        AddFontFamily(new FontFamilyAdapter(new XFontFamily(family.Name)));
        //    }
        //}

        public bool PrintHtml(string html)
        {
            var pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
            pdf.AddPage();

            MemoryStream stream = new MemoryStream();
            pdf.Save(stream);
            return PrintPDF("", stream);
        }
    }
    class Program
    {
        static void Main()
        {
            Printer printer = new Printer();
            //var ret = printer.PrintPDF("", "F:\\Users\\swplo\\Downloads\\员工理赔申请书（外服版）本人.pdf");
            string htmlCode = "text <div> <b> bold </ b> or <u> underlined </ u> <div/>";
            printer.PrintHtml(htmlCode);
            Console.WriteLine("Hello, World!");
        }
        
    }
}
