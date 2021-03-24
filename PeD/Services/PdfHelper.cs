using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace PeD.Services
{
    public class PdfHelper
    {
        public static void AddPagesToPdf(string filename, int x, int y)
        {
            var filetmp = filename + ".tmp";
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(filename), new PdfWriter(filetmp));
            Document doc = new Document(pdfDoc);
            var numPages = pdfDoc.GetNumberOfPages();
            //var font = PdfFontFactory.CreateFont(Path.Combine(_hostingEnvironment.WebRootPath, "Assets/fonts/Roboto-Regular.ttf"));
            //doc.SetFont(font);
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                float height = pdfDoc.GetPage(i).GetPageSize().GetTop();
                float width = pdfDoc.GetPage(i).GetPageSize().GetWidth();
                /*
                float bottom = pdfDoc.GetPage(i).GetPageSize().GetBottom();

                float x = pdfDoc.GetPage(i).GetPageSize().GetWidth() / 2;
                float y = pdfDoc.GetPage(i).GetPageSize().GetBottom() + 20;
                 */

                Paragraph pages = new Paragraph(string.Format("Folha {0} de {1}", i, numPages))
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.BLACK);
                doc.ShowTextAligned(pages, x, height - y, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM,
                    0);
                //doc.ShowTextAligned(pages, width, top + 40, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            }

            doc.Close();
            File.Delete(filename);
            File.Move(filetmp, filename);
        }
    }
}