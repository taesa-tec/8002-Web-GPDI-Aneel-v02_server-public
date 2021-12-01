using System;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Configuration;
using PeD.Core.Models;

namespace PeD.Services
{
    public class PdfService
    {
        private ArquivoService _arquivoService;
        private IConfiguration _configuration;

        public PdfService(ArquivoService arquivoService, IConfiguration configuration)
        {
            _arquivoService = arquivoService;
            _configuration = configuration;
        }

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
                doc.ShowTextAligned(pages, x, height - y, i, TextAlignment.CENTER, VerticalAlignment.MIDDLE,
                    0);
                //doc.ShowTextAligned(pages, width, top + 40, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            }

            doc.Close();
            File.Delete(filename);
            File.Move(filetmp, filename);
        }

        public FileUpload HtmlToPdf(string content, string name)
        {
            var storagePath = _configuration.GetValue<string>("StoragePath");
            var file = Path.Combine(storagePath, "temp", Path.GetRandomFileName());
            var stream = File.Create(file);
            HtmlConverter.ConvertToPdf(content, stream);
            stream.Close();
            var arquivo = _arquivoService.FromPath(file, "application/pdf", $"{name}.pdf");
            File.Delete(file);
            return arquivo;
        }
    }
}