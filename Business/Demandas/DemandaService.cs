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
using Microsoft.EntityFrameworkCore;
using APIGestor.Exceptions.Demandas;
using APIGestor.Business.Sistema;

namespace APIGestor.Business.Demandas
{

    public class DemandaService : BaseGestorService
    {
        SistemaService sistemaService;
        protected static readonly List<FieldList> Forms = new List<FieldList>(){
                new EspecificacaoTecnicaForm()
        };
        protected delegate bool CanDemandaProgress(Demanda demanda, string userId);

        protected Dictionary<Etapa, CanDemandaProgress> DemandaProgressCheck;

        public static FieldList GetForm(string key)
        {
            return DemandaService.Forms.FirstOrDefault(f => f.Key == key);
        }
        private IHostingEnvironment _hostingEnvironment;
        public DemandaService(
            GestorDbContext context,
            IAuthorizationService authorization,
            LogService logService,
            IHostingEnvironment hostingEnvironment,
            SistemaService sistemaService
            ) : base(context, authorization, logService)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.sistemaService = sistemaService;
            DemandaProgressCheck = new Dictionary<Etapa, CanDemandaProgress>(){
                {Etapa.Elaboracao, ElaboracaoProgress},
                {Etapa.PreAprovacao, PreAprovacaoProgress},
                {Etapa.RevisorPendente, AprovacaoCoordenadorProgress},
                {Etapa.AprovacaoRevisor, AprovacaoRevisorProgress },
                {Etapa.AprovacaoCoordenador, AprovacaoCoordenadorProgress },
                {Etapa.AprovacaoGerente, AprovacaoGerenteProgress },
                {Etapa.AprovacaoDiretor, AprovacaoDiretorProgress },
            };
        }

        public Demanda GetById(int id)
        {
            return _context.Demandas
            .Include("Criador")
            .Include("SuperiorDireto")
            .Include("Comentarios")
            .FirstOrDefault(d => d.Id == id);
        }

        public bool DemandaExist(int id)
        {
            return _context.Demandas.Any(d => d.Id == id);
        }
        public bool UserCanAccess(int id, string userId)
        {
            if (DemandaExist(id))
            {
                if (sistemaService.GetEquipePeD().Ids.Contains(userId))
                    return true;
                var demanda = _context.Demandas.Find(id);
                return (demanda.CriadorId == userId || demanda.SuperiorDiretoId == userId || demanda.RevisorId == userId);
            }
            return false;
        }
        protected IQueryable<Demanda> QueryDemandas(string userId = null)
        {
            var CargosChavesIds = sistemaService.GetEquipePeD().CargosChavesIds;
            return _context.Demandas
            .Include("Criador")
            .Include("SuperiorDireto")
            .Include("Revisor")
            .ByUser(CargosChavesIds.Contains(userId) ? null : userId);
        }
        public List<Demanda> GetByEtapa(Etapa etapa, string userId = null)
        {

            return QueryDemandas(userId).Where(d => d.EtapaAtual == etapa).ToList();
        }
        public List<Demanda> GetByEtapaStatus(EtapaStatus status, string userId = null)
        {
            return QueryDemandas(userId)
            .Where(d => d.EtapaStatus == status).ToList();
        }
        public List<Demanda> GetDemandasReprovadas(string userId = null)
        {
            return QueryDemandas(userId)
            .Where(d => d.EtapaStatus == EtapaStatus.ReprovadaPermanente).ToList();
        }
        public List<Demanda> GetDemandasAprovadas(string userId = null)
        {
            return QueryDemandas(userId)
            // .Where(d => d.EtapaAtual == Etapa.AprovacaoDiretor && (d.EtapaStatus == EtapaStatus.Aprovada && d.EtapaStatus == EtapaStatus.Concluido)).ToList();
            .Where(d => d.EtapaAtual == Etapa.AprovacaoDiretor).ToList();
        }
        public List<Demanda> GetDemandasEmElaboracao(string userId = null)
        {
            return QueryDemandas(userId)
            .Where(d => d.EtapaAtual == Etapa.Elaboracao || d.EtapaStatus == EtapaStatus.EmElaboracao).ToList();
        }
        public List<Demanda> GetDemandasCaptacao(string userId = null)
        {
            return QueryDemandas(userId)
            .Where(d => d.EtapaAtual == Etapa.Captacao).ToList();
        }
        public List<Demanda> GetDemandasPendentes(string userId = null)
        {
            return GetByEtapaStatus(EtapaStatus.Pendente, userId);
        }
        public void EnviarCaptacao(int id)
        {

            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaAtual = Etapa.Captacao;
                demanda.EtapaStatus = EtapaStatus.Concluido;
                demanda.CaptacaoDate = DateTime.Now;
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

        public void ProximaEtapa(int id, string userId, string revisorId = null)
        {
            var demanda = GetById(id);
            if (!String.IsNullOrWhiteSpace(revisorId) && _context.Users.Any(user => user.Id == revisorId))
            {
                demanda.RevisorId = revisorId;
            }
            if (demanda != null && this.DemandaProgressCheck.ContainsKey(demanda.EtapaAtual))
            {
                if (this.DemandaProgressCheck[demanda.EtapaAtual](demanda, userId))
                {
                    demanda.ProximaEtapa();

                    _context.SaveChanges();
                }
                else
                {
                    throw new DemandaException("O usuário não é responsável pela continuidade da demanda");
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public void SetSuperiorDireto(int id, string SuperiorDiretoId)
        {
            if (DemandaExist(id))
            {
                GetById(id).SuperiorDiretoId = SuperiorDiretoId;
                _context.SaveChanges();
                return;
            }
            throw new DemandaException("Demanda Não existe");

        }
        public string GetSuperiorDireto(int id)
        {
            if (DemandaExist(id))
            {
                return GetById(id).SuperiorDiretoId;
            }
            return null;
        }
        public void ReprovarReiniciar(int id, string userId)
        {
            if (!DemandaExist(id))
            {
                throw new DemandaException("Demanda Não existe");
            }
            var demanda = GetById(id);

            if (demanda != null && this.DemandaProgressCheck.ContainsKey(demanda.EtapaAtual))
            {
                if (this.DemandaProgressCheck[demanda.EtapaAtual](demanda, userId))
                {
                    demanda.ReprovarReiniciar();
                    _context.SaveChanges();
                }
                else
                {
                    throw new DemandaException("Usuário não tem permissão para reiniciar essa demanda");
                }
            }

        }
        public void ReprovarPermanente(int id, string userId)
        {
            if (!DemandaExist(id))
            {
                throw new DemandaException("Demanda Não existe");
            }

            var demanda = GetById(id);

            if (demanda != null && this.DemandaProgressCheck.ContainsKey(demanda.EtapaAtual))
            {
                if (this.DemandaProgressCheck[demanda.EtapaAtual](demanda, userId))
                {
                    demanda.ReprovarPermanente();
                    _context.SaveChanges();
                }
                else
                {
                    throw new DemandaException("Usuário não tem permissão para reiniciar essa demanda");
                }
            }
        }
        protected void AprovarDemanda(int id)
        {
            var demanda = GetById(id);
            AprovarDemanda(demanda);
            return;
        }
        protected void AprovarDemanda(Demanda demanda)
        {
            if (demanda != null)
            {
                demanda.ProximaEtapa();
                demanda.EtapaStatus = EtapaStatus.EmElaboracao;
                _context.SaveChanges();
            }
            return;
        }
        public void AddComentario(int id, DemandaComentario comentario)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.Comentarios = demanda.Comentarios != null ? demanda.Comentarios : new List<DemandaComentario>();
                demanda.Comentarios.Add(comentario);
                _context.SaveChanges();
            }
            return;
        }

        public void AddComentario(int id, string comentario, string userId)
        {
            AddComentario(id, new DemandaComentario()
            {
                Content = comentario,
                DemandaId = id,
                UserId = userId,
                CreatedAt = DateTime.Now
            });
        }

        #region Progresso das demandas


        protected bool ElaboracaoProgress(Demanda demanda, string userId)
        {
            return demanda.CriadorId == userId;
        }

        protected bool PreAprovacaoProgress(Demanda demanda, string userId)
        {
            return demanda.SuperiorDiretoId == userId;
        }
        protected bool AprovacaoRevisorProgress(Demanda demanda, string userId)
        {
            return demanda.RevisorId == userId;
        }
        protected bool AprovacaoCoordenadorProgress(Demanda demanda, string userId)
        {
            return userId == sistemaService.GetEquipePeD().Coordenador;
        }
        protected bool AprovacaoGerenteProgress(Demanda demanda, string userId)
        {
            return userId == sistemaService.GetEquipePeD().Gerente;
        }
        protected bool AprovacaoDiretorProgress(Demanda demanda, string userId)
        {
            return userId == sistemaService.GetEquipePeD().Diretor;
        }


        #endregion

        #region Documentos das demandas

        public DemandaFormValues GetDemandaFormData(int id, string form)
        {
            return _context.DemandaFormValues.Include("Files.File").FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
        }
        public List<Models.FileUpload> GetDemandaFiles(int id)
        {
            return _context.DemandaFormValues
            .Include("Files.File")
            .Where(df => df.DemandaId == id)
            .SelectMany(_dfv => _dfv.Files.Select(dff => dff.File))
            .ToList();
        }
        public Models.FileUpload GetDemandaFile(int id, int file_id)
        {
            return GetDemandaFiles(id).First(file => file.Id == file_id);
        }
        public void SalvarDemandaFormData(int id, string form, JObject data)
        {
            var formdata = data.Value<JObject>("form");
            var formanexos = data.Value<JArray>("anexos");
            var df_data = _context.DemandaFormValues.Include("Files").FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
            if (df_data != null)
            {
                df_data.SetValue(formdata);

                df_data.Files = formanexos.ToList().Select(item => new DemandaFormFile()
                {
                    DemandaFormId = df_data.Id,
                    FileId = item.Value<int>()
                }).ToList();
            }
            else
            {
                df_data = new DemandaFormValues();
                df_data.DemandaId = id;
                df_data.FormKey = form;
                df_data.SetValue(formdata);
                df_data.Files = formanexos.ToList().Select(item => new DemandaFormFile()
                {
                    FileId = item.Value<int>()
                }).ToList();
                _context.DemandaFormValues.Add(df_data);
            }
            _context.SaveChanges();
            SaveDemandaFormPdf(id, form);
        }
        public string GetDemandaFormHTML(int id, string form)
        {
            var _form = GetForm(form);
            string body = string.Empty;
            var document = new HtmlDocument();
            document.Load(Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates/pdf-template.html"));

            var mainContent = document.DocumentNode.SelectSingleNode("//div[@id='main-content']");
            var formName = document.DocumentNode.SelectSingleNode("//span[@id='formulario']");
            var titulo = document.DocumentNode.SelectSingleNode("//span[@id='titulo']");
            var documento = document.DocumentNode.SelectSingleNode("//span[@id='documento']");

            if (formName != null)
            {
                formName.AppendChild(HtmlNode.CreateNode(_form.Title));
            }
            if (documento != null)
            {
                documento.AppendChild(HtmlNode.CreateNode("Documento: 00000"));
            }

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
                return field.Render(data.Object);
            }
            return new FieldRendered("Error", "No data or Form found");
        }

        public string SaveDemandaFormPdf(int id, string form)
        {
            string fullname = GetDemandaFormPdfFilename(id, form, true);
            var html = GetDemandaFormHTML(id, form);
            var writer = new PdfWriter(fullname);

            HtmlConverter.ConvertToPdf(html, new FileStream(fullname, FileMode.Create));

            UpdatePdf(fullname);

            return fullname;
        }

        public string GetDemandaFormPdfFilename(int id, string form, bool createDirectory = false)
        {
            string folderName = String.Format("uploads/demandas/{0}/{1}/", id, form);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string filename = String.Format("demanda-{0}-{1}.pdf", id, form);
            if (createDirectory && !Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            return Path.Combine(newPath, filename);
        }

        protected void UpdatePdf(string filename)
        {
            var filetmp = filename + ".tmp";
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(filename), new PdfWriter(filetmp));
            Document doc = new Document(pdfDoc);
            //var font = PdfFontFactory.CreateFont(Path.Combine(_hostingEnvironment.WebRootPath, "Assets/fonts/Roboto-Regular.ttf"));
            //doc.SetFont(font);
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                float width = pdfDoc.GetPage(i).GetPageSize().GetWidth();
                float height = pdfDoc.GetPage(i).GetPageSize().GetTop();
                float bottom = pdfDoc.GetPage(i).GetPageSize().GetBottom();

                float x = pdfDoc.GetPage(i).GetPageSize().GetWidth() / 2;
                float y = pdfDoc.GetPage(i).GetPageSize().GetBottom() + 20;

                Paragraph pages = new Paragraph(String.Format("Folha {0} de {1}", i, pdfDoc.GetNumberOfPages()))
                                .SetFontSize(12)
                                .SetFontColor(ColorConstants.BLACK);
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
        #endregion

    }
    public static class DemandaExtension
    {
        public static IQueryable<Demanda> ByUser(this IQueryable<Demanda> dbSet, string userId)
        {
            return dbSet.Where(demanda => String.IsNullOrWhiteSpace(userId) || demanda.CriadorId == userId || demanda.SuperiorDiretoId == userId || demanda.RevisorId == userId);
        }
    }
}