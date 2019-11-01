using System;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Models.Demandas;
using APIGestor.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.IO;
using iText.Html2pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Hosting;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using APIGestor.Models.Demandas.Forms;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace APIGestor.Business
{

    public class DemandaService : BaseGestorService
    {

        protected static readonly List<FieldList> Forms = new List<FieldList>(){
                new EspecificacaoTecnicaForm()
        };
        public static FieldList GetForm(string key)
        {
            return DemandaService.Forms.FirstOrDefault(f => f.Key == key);
        }
        private IHostingEnvironment _hostingEnvironment;
        public DemandaService(GestorDbContext context, IAuthorizationService authorization, LogService logService, IHostingEnvironment hostingEnvironment) : base(context, authorization, logService)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public Demanda GetById(int id)
        {
            return _context.Demandas.FirstOrDefault(d => d.Id == id);
        }

        public bool DemandaExist(int id)
        {
            return _context.Demandas.Any(d => d.Id == id);
        }

        public List<Demanda> GetByEtapa(Etapa etapa)
        {
            return _context.Demandas.Where(d => d.EtapaAtual == etapa).ToList();
        }
        public List<Demanda> GetByEtapaStatus(EtapaStatus status)
        {
            return _context.Demandas.Where(d => d.EtapaStatus == status).ToList();
        }
        public List<Demanda> GetDemandasReprovadas()
        {
            return _context.Demandas.Where(d => d.EtapaStatus == EtapaStatus.Reprovada || d.EtapaStatus == EtapaStatus.ReprovadaPermanente).ToList();
        }
        public List<Demanda> GetDemandasAprovadas()
        {
            return _context.Demandas.Where(d => d.EtapaStatus == EtapaStatus.Aprovada && d.EtapaAtual == Etapa.AprovacaoDiretor).ToList();
        }
        public List<Demanda> GetDemandasEmElaboracao()
        {
            return _context.Demandas.Where(d => d.EtapaAtual == Etapa.Elaboracao || d.EtapaStatus == EtapaStatus.EmElaboracao).ToList();
        }
        public List<Demanda> GetDemandasCaptacao()
        {
            return _context.Demandas.Where(d => d.EtapaAtual == Etapa.Captacao).ToList();
        }
        public List<Demanda> GetDemandasPendentes()
        {
            return GetByEtapaStatus(EtapaStatus.Pendente);
        }
        public void EnviarCaptacao(int id)
        {

            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaAtual = Etapa.Captacao;
                _context.SaveChanges();
            }
        }
        public Demanda CriarDemanda(string titulo, string userId)
        {
            var demanda = new Demanda();
            demanda.Titulo = titulo;
            demanda.CriadorId = userId;
            demanda.EtapaAtual = Etapa.Elaboracao;
            demanda.EtapaStatus = EtapaStatus.EmElaboracao;
            _context.Demandas.Add(demanda);
            _context.SaveChanges();
            return demanda;
        }
        public Demanda AlterarStatusDemanda(int id, APIGestor.Models.Demandas.EtapaStatus status)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaStatus = status;
                _context.SaveChanges();
            }
            return demanda;
        }
        public void AprovarDemanda(int id)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.ProximaEtapa();
                demanda.EtapaStatus = EtapaStatus.EmElaboracao;
                _context.SaveChanges();
            }
            return;
        }
        public void ReprovarDemanda(int id, DemandaComentario comentario, bool permanente)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaStatus = permanente ? EtapaStatus.ReprovadaPermanente : EtapaStatus.Reprovada;
                demanda.Comentarios = demanda.Comentarios != null ? demanda.Comentarios : new List<DemandaComentario>();
                demanda.Comentarios.Add(comentario);
                _context.SaveChanges();
            }
            return;
        }
        public JObject GetDemandaFormData(int id, string form)
        {
            var data = _context.DemandaFormValues.FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
            if (data != null)
            {
                return data.ToObject();
            }
            return null;
        }
        public void SalvarDemandaFormData(int id, string form, object data)
        {
            var df_data = _context.DemandaFormValues.FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
            if (df_data != null)
            {
                df_data.SetValue(data);
            }
            else
            {
                df_data = new DemandaFormValues();
                df_data.DemandaId = id;
                df_data.FormKey = form;
                df_data.SetValue(data);
                _context.DemandaFormValues.Add(df_data);
            }
            _context.SaveChanges();
        }

        protected IElement GerarHeader(string titulo, string documento, string folhas)
        {
            Table table = new Table(3, true).UseAllAvailableWidth();
            //table.StartNewRow()
            table.AddCell("A");
            table.AddCell("B");
            table.AddCell("C");

            return table;
        }

        protected string GetDemandaFormHTML(int id, string form)
        {

            string body = string.Empty;
            var document = new HtmlDocument();
            document.Load(Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates/pdf-template.html"));

            var mainContent = document.DocumentNode.SelectSingleNode("//div[@id='main-content']");

            var demandaFormValue = RenderDocument(id, form);
            if (demandaFormValue != null && mainContent != null)
            {
                var data = demandaFormValue.ToHtml();
                mainContent.AppendChild(data);

            }
            return document.DocumentNode.InnerHtml;
        }

        public FieldRendered RenderDocument(int id, string form)
        {
            var field = GetForm(form);
            field.RenderHandler = SanitizerFieldRender;

            var data = GetDemandaFormData(id, form);
            if (data != null && field != null)
            {
                return field.Render(data);
            }
            return new FieldRendered("Error", "No data or Form found");
        }

        public string SaveDemandaFormPdf(int id, string form)
        {
            string folderName = String.Format("uploads/demandas/{0}/{1}/", id, form);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string filename = String.Format("demanda-{0}-{1}.pdf", id, form);
            string fullname = Path.Combine(newPath, filename);

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            var html = GetDemandaFormHTML(id, form);
            var writer = new PdfWriter(fullname);

            HtmlConverter.ConvertToPdf(html, new FileStream(fullname, FileMode.Create));
            var _form = GetForm(form);
            UpdatePdf(fullname, "ABC-1234", _form.Title);

            return fullname;
        }

        protected void UpdatePdf(string filename, string demanda, string formname)
        {
            var font = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            var filetmp = filename + ".tmp";
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(filename), new PdfWriter(filetmp));
            Document doc = new Document(pdfDoc);

            Paragraph paragraphDemanda = new Paragraph("Documento: ").Add(new Paragraph(demanda).SetBold())
            .SetFont(font)
            .SetFontSize(12)
            .SetFontColor(ColorConstants.BLACK);
            Paragraph paragraphForm = new Paragraph(formname)
                .SetFont(font)
                .SetFontSize(13)
                .SetBold()
                .SetFontColor(ColorConstants.BLACK);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                float width = pdfDoc.GetPage(i).GetPageSize().GetWidth();
                float height = pdfDoc.GetPage(i).GetPageSize().GetTop();
                float bottom = pdfDoc.GetPage(i).GetPageSize().GetBottom();

                float x = pdfDoc.GetPage(i).GetPageSize().GetWidth() / 2;
                float y = pdfDoc.GetPage(i).GetPageSize().GetBottom() + 20;

                Paragraph pages = new Paragraph(String.Format("Folha {0} de {1}", i, pdfDoc.GetNumberOfPages()))
                                .SetFont(font)
                                .SetFontSize(12)
                                .SetFontColor(ColorConstants.BLACK);

                doc.ShowTextAligned(paragraphForm, width / 2, height - 75, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);

                doc.ShowTextAligned(paragraphDemanda, width - 120, height - 60, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
                doc.ShowTextAligned(pages, width - 120, height - 90, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
                //doc.ShowTextAligned(pages, width, top + 40, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);

            }
            doc.Close();
            File.Delete(filename);
            File.Move(filetmp, filename);




        }

        protected void SanitizerFieldRender(Field field, JToken _data, ref FieldRendered fieldRendered)
        {
            if (field.FieldType == "Temas")
            {
                fieldRendered.Value = "";

                var tema = (_data as JObject).GetValue("value") as JObject;
                var catalogId = tema.GetValue("catalogTemaId").Value<int>();
                var outroDesc = tema.GetValue("outroDesc").Value<string>();

                var catalogTema = _context.CatalogTema.Find(catalogId);
                if (catalogTema != null)
                {
                    fieldRendered.Value = catalogTema.Nome;
                    if (!String.IsNullOrWhiteSpace(outroDesc))
                    {
                        fieldRendered.Value = String.Concat(catalogTema.Nome, ": ", outroDesc);
                    }
                }


                var subtemas = tema.GetValue("subTemas") as JArray;
                var subtemasList = new List<FieldRendered>();
                subtemas.Children().ToList().ForEach(child =>
                {
                    var catalogSubTemaId = (child as JObject).GetValue("catalogSubTemaId").Value<int>();
                    var subOutroDesc = (child as JObject).GetValue("outroDesc").Value<string>();

                    var catalogSubTema = _context.CatalogSubTemas.Find(catalogSubTemaId);
                    if (catalogSubTema != null)
                    {
                        var item = new FieldRendered("Sub Tema", catalogSubTema.Nome);
                        if (!String.IsNullOrWhiteSpace(subOutroDesc))
                        {
                            item.Value = String.Concat(catalogSubTema.Nome, ": ", subOutroDesc);
                        }
                        subtemasList.Add(item);
                    }
                });

                fieldRendered.Children.AddRange(subtemasList);

            }

        }


    }
}