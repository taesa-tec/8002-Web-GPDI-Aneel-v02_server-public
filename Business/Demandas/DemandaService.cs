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
    public class DemandaService
    {
        protected delegate bool CanDemandaProgress(Demanda demanda, string userId);

        #region Statics

        protected static readonly List<FieldList> Forms = new List<FieldList>()
        {
            new EspecificacaoTecnicaForm()
        };

        public static FieldList GetForm(string key)
        {
            return DemandaService.Forms.FirstOrDefault(f => f.Key == key);
        }

        #endregion

        #region Props

        SistemaService sistemaService;
        MailerService mailer;
        GestorDbContext _context;
        IAuthorizationService authorization;
        public readonly DemandaLogService LogService;
        protected Dictionary<Etapa, CanDemandaProgress> DemandaProgressCheck;
        private IHostingEnvironment _hostingEnvironment;

        #endregion

        #region Contructor

        public DemandaService(
            GestorDbContext context,
            MailerService mailer,
            IAuthorizationService authorization,
            DemandaLogService logService,
            IHostingEnvironment hostingEnvironment,
            SistemaService sistemaService
        )
        {
            this._context = context;
            this.authorization = authorization;
            this._hostingEnvironment = hostingEnvironment;
            this.sistemaService = sistemaService;
            this.mailer = mailer;
            this.LogService = logService;


            DemandaProgressCheck = new Dictionary<Etapa, CanDemandaProgress>()
            {
                {Etapa.Elaboracao, ElaboracaoProgress},
                {Etapa.PreAprovacao, PreAprovacaoProgress},
                {Etapa.RevisorPendente, AprovacaoCoordenadorProgress},
                {Etapa.AprovacaoRevisor, AprovacaoRevisorProgress},
                {Etapa.AprovacaoCoordenador, AprovacaoCoordenadorProgress},
                {Etapa.AprovacaoGerente, AprovacaoGerenteProgress},
                {Etapa.AprovacaoDiretor, AprovacaoDiretorProgress},
            };
        }

        #endregion

        #region Helpers

        public Demanda GetById(int id)
        {
            return _context.Demandas
                .Include("Criador")
                .Include("SuperiorDireto")
                .Include("Comentarios.User")
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
                return (demanda.CriadorId == userId || demanda.SuperiorDiretoId == userId ||
                        demanda.RevisorId == userId);
            }

            return false;
        }

        #endregion

        #region Listar Demandas

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

        public List<Demanda> GetByEtapaStatus(DemandaStatus status, string userId = null)
        {
            return QueryDemandas(userId)
                .Where(d => d.Status == status).ToList();
        }

        public List<Demanda> GetDemandasReprovadas(string userId = null)
        {
            return QueryDemandas(userId)
                .Where(d => d.Status == DemandaStatus.ReprovadaPermanente).ToList();
        }

        public List<Demanda> GetDemandasAprovadas(string userId = null)
        {
            return QueryDemandas(userId)
                // .Where(d => d.EtapaAtual == Etapa.AprovacaoDiretor && (d.EtapaStatus == EtapaStatus.Aprovada && d.EtapaStatus == EtapaStatus.Concluido)).ToList();
                .Where(d => d.EtapaAtual == Etapa.AprovacaoDiretor && d.Status == DemandaStatus.Aprovada).ToList();
        }

        public List<Demanda> GetDemandasEmElaboracao(string userId = null)
        {
            return QueryDemandas(userId)
                .Where(d => d.EtapaAtual == Etapa.Elaboracao || d.Status == DemandaStatus.EmElaboracao ||
                            d.Status == DemandaStatus.Pendente).ToList();
        }

        public List<Demanda> GetDemandasCaptacao(string userId = null)
        {
            return QueryDemandas(userId)
                .Where(d => d.EtapaAtual == Etapa.Captacao).ToList();
        }

        public List<Demanda> GetDemandasPendentes(string userId = null)
        {
            return GetByEtapaStatus(DemandaStatus.Pendente, userId);
        }

        #endregion

        #region Criação e Alteração de Demandas

        public Demanda CriarDemanda(string titulo, string userId)
        {
            var demanda = new Demanda();
            demanda.Titulo = titulo;
            demanda.CriadorId = userId;
            demanda.EtapaAtual = Etapa.Elaboracao;
            demanda.Status = DemandaStatus.EmElaboracao;
            _context.Demandas.Add(demanda);
            _context.SaveChanges();
            demanda = GetById(demanda.Id);
            LogService.Incluir(userId, demanda.Id, "Criou Demanda",
                String.Format(" {0} criou demanda \"{1}\"", demanda.Criador.NomeCompleto, demanda.Titulo));
            return demanda;
        }

        public Demanda AlterarStatusDemanda(int id, APIGestor.Models.Demandas.DemandaStatus status)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.Status = status;
                _context.SaveChanges();
            }

            return demanda;
        }

        public void SetEtapa(int id, Etapa etapa, string userId)
        {
            var demanda = GetById(id);
            if (demanda == null)
                return;
            var user = _context.Users.Find(userId);
            demanda.IrParaEtapa(etapa);
            _context.SaveChanges();
            NotificarResponsavel(demanda, userId);

            if (demanda.EtapaAtual < Etapa.Captacao && demanda.Status == DemandaStatus.EmElaboracao)
            {
                LogService.Incluir(userId, demanda.Id, "Alterou Etapa",
                    String.Format(" {0} alterou a etapa da demanda para \"{1}\"", user.NomeCompleto,
                        demanda.EtapaDesc));
            }
            else if (demanda.EtapaAtual == Etapa.AprovacaoDiretor && demanda.Status == DemandaStatus.Concluido)
            {
                LogService.Incluir(userId, demanda.Id, "Aprovou a etapa",
                    String.Format(" {0} aprovou a demanda.", user.NomeCompleto));
            }
        }

        public void ProximaEtapa(int id, string userId, string revisorId = null)
        {
            var demanda = GetById(id);


            if (demanda != null)
            {
                if (!String.IsNullOrWhiteSpace(revisorId) && _context.Users.Any(user => user.Id == revisorId))
                {
                    demanda.RevisorId = revisorId;
                }

                if (GetResponsavelAtual(demanda) == userId)
                {
                    demanda.ProximaEtapa();
                    _context.SaveChanges();
                    NotificarResponsavel(demanda, userId);
                    var user = _context.Users.Find(userId);
                    if (demanda.Status == DemandaStatus.Aprovada)
                    {
                        LogService.Incluir(userId, demanda.Id, "Aprovação de demanda",
                            String.Format("O usuário {0} aprovou a demanda", user.NomeCompleto));
                    }
                    else
                    {
                        LogService.Incluir(userId, demanda.Id, "Avanço de Etapa",
                            String.Format(" {0} alterou a etapa da demanda para \"{1}\"", user.NomeCompleto,
                                demanda.EtapaDesc));
                    }
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
                var demanda = GetById(id);
                demanda.SuperiorDiretoId = SuperiorDiretoId;
                _context.SaveChanges();
                var user = _context.Users.Find(SuperiorDiretoId);
                LogService.Incluir(demanda.CriadorId, demanda.Id, "Definiu Superior Direto",
                    String.Format(" {0} definiu o usuário {1} como superior direto", demanda.Criador.NomeCompleto,
                        user.NomeCompleto));
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
                    _context.DemandaFormValues
                        .Where(form => form.DemandaId == id)
                        .ToList()
                        .ForEach(f => f.Revisao++);
                    _context.SaveChanges();
                    NotificarReprovacao(demanda, _context.Users.Find(userId));
                    var user = _context.Users.Find(userId);
                    LogService.Incluir(userId, id, "Reiniciou a demanda",
                        String.Format("O usuário {0} reiniciou a demanda", user.NomeCompleto));
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
                    NotificarReprovacaoPermanente(demanda, _context.Users.Find(userId));
                    var user = _context.Users.Find(userId);
                    LogService.Incluir(userId, id, "Arquivou a demanda",
                        String.Format("O usuário {0} reprovou e arquivou a demanda", user.NomeCompleto));
                }
                else
                {
                    throw new DemandaException("Usuário não tem permissão para reprovar essa demanda");
                }
            }
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

        public void EnviarCaptacao(int id, string userId)
        {
            var demanda = GetById(id);
            if (demanda != null && demanda.EtapaAtual != Etapa.Captacao)
            {
                demanda.EtapaAtual = Etapa.Captacao;
                demanda.Status = DemandaStatus.Concluido;
                demanda.CaptacaoDate = DateTime.Now;
                _context.SaveChanges();
                var user = _context.Users.Find(userId);
                LogService.Incluir(userId, id, "Demanda para capacitação",
                    String.Format("O usuário {0} enviou demanda para a capacitação", user.NomeCompleto));
            }
        }

        #endregion

        #region Progresso das demandas

        protected string GetResponsavelAtual(Demanda demanda)
        {
            switch (demanda.EtapaAtual)
            {
                case Etapa.Elaboracao:
                    return demanda.CriadorId;
                case Etapa.PreAprovacao:
                    return demanda.SuperiorDiretoId;
                case Etapa.AprovacaoRevisor:
                    return demanda.RevisorId;
                case Etapa.RevisorPendente:
                case Etapa.AprovacaoCoordenador:
                    return sistemaService.GetEquipePeD().Coordenador;
                case Etapa.AprovacaoGerente:
                    return sistemaService.GetEquipePeD().Gerente;
                case Etapa.AprovacaoDiretor:
                    return sistemaService.GetEquipePeD().Diretor;
                default:
                    return null;
            }
        }

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
            return _context.DemandaFormValues.Include("Files.File")
                .FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
        }

        public List<Models.Demandas.DemandaFile> GetDemandaFiles(int id)
        {
            return _context.DemandaFormValues
                .Include("Files.File")
                .Where(df => df.DemandaId == id)
                .SelectMany(_dfv => _dfv.Files.Select(dff => dff.File))
                .Distinct<DemandaFile>()
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
            var df_data = _context.DemandaFormValues.Include("Files")
                .FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
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
            try
            {
                SaveDemandaFormPdf(id, form);
            }
            catch (Exception)
            {
            }
        }

        public string GetDemandaFormHTML(int id, string form)
        {
            // @todo Passar para cshtml
            var _form = GetForm(form);
            var demanda = GetById(id);
            string body = string.Empty;
            var document = new HtmlDocument();
            var demandaFormValue = RenderDocument(id, form);
            var formDemanda = _context.DemandaFormValues.FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);

            if (formDemanda == null)
            {
                throw new DemandaException("Não há formulário");
            }

            document.Load(Path.Combine(_hostingEnvironment.WebRootPath, "Templates/pdf-template.html"));

            var mainContent = document.DocumentNode.SelectSingleNode("//div[@id='main-content']");
            var formName = document.DocumentNode.SelectSingleNode("//span[@id='formulario']");
            var titulo = document.DocumentNode.SelectSingleNode("//span[@id='projeto-titulo']");
            var documento = document.DocumentNode.SelectSingleNode("//span[@id='documento']");
            var revisao = document.DocumentNode.SelectSingleNode("//span[@id='revisao']");
            var descricao = document.DocumentNode.SelectSingleNode("//span[@id='projeto-descricao']");
            var data = document.DocumentNode.SelectSingleNode("//span[@id='data']");

            var autor = document.DocumentNode.SelectSingleNode("//span[@id='autor']");
            var autorFuncao = document.DocumentNode.SelectSingleNode("//span[@id='autor-funcao']");

            var gerente = document.DocumentNode.SelectSingleNode("//span[@id='gerente']");
            var gerenteFuncao = document.DocumentNode.SelectSingleNode("//span[@id='gerente-funcao']");


            if (formName != null)
            {
                formName.AppendChild(HtmlNode.CreateNode(_form.Title));
            }

            if (documento != null)
            {
                documento.AppendChild(HtmlNode.CreateNode(
                    String.Format("Documento: PED-{0}/{1}", demanda.Id.ToString().PadLeft(3, '0'),
                        demanda.CreatedAt.Year)));
            }

            if (titulo != null)
            {
                titulo.AppendChild(HtmlNode.CreateNode(demanda.Titulo));
            }

            if (revisao != null)
            {
                revisao.AppendChild(HtmlNode.CreateNode(formDemanda.Revisao.ToString()));
            }

            if (data != null)
            {
                data.AppendChild(HtmlNode.CreateNode(DateTime.Today.ToShortDateString()));
            }

            if (autor != null)
            {
                autor.AppendChild(HtmlNode.CreateNode(demanda.Criador.NomeCompleto));
            }

            if (autorFuncao != null && !String.IsNullOrWhiteSpace(demanda.Criador.Cargo))
            {
                autorFuncao.AppendChild(HtmlNode.CreateNode(demanda.Criador.Cargo));
            }


            if (demanda.SuperiorDireto != null)
            {
                if (gerente != null)
                {
                    gerente.AppendChild(HtmlNode.CreateNode(demanda.SuperiorDireto.NomeCompleto));
                }

                if (gerenteFuncao != null && !String.IsNullOrWhiteSpace(demanda.SuperiorDireto.Cargo))
                {
                    gerenteFuncao.AppendChild(HtmlNode.CreateNode(demanda.SuperiorDireto.Cargo));
                }
            }
            else
            {
                gerente.AppendChild(HtmlNode.CreateNode("SUPERIOR NÃO CADASTRADO"));
                gerenteFuncao.AppendChild(HtmlNode.CreateNode("SUPERIOR NÃO CADASTRADO"));
            }

            if (descricao != null)
            {
                descricao.AppendChild(HtmlNode.CreateNode(demanda.Titulo));
            }


            if (demandaFormValue != null && mainContent != null)
            {
                var htmlform = demandaFormValue.ToHtml();
                mainContent.AppendChild(htmlform);
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
                doc.ShowTextAligned(pages, width - 120, height - 90, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM,
                    0);
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

        #region Notificação do usuários

        public void NotificarResponsavel(Demanda demanda, string userId)
        {
            switch (demanda.EtapaAtual)
            {
                case Etapa.PreAprovacao:
                    NotificarSuperior(demanda);
                    break;

                case Etapa.RevisorPendente:
                    NotificarRevisorPendente(demanda);
                    break;
                case Etapa.AprovacaoRevisor:
                    NotificarRevisor(demanda);
                    break;
                case Etapa.AprovacaoCoordenador:
                case Etapa.AprovacaoGerente:
                    NotificarAprovador(demanda, userId);
                    break;
                case Etapa.AprovacaoDiretor:
                    if (demanda.Status != DemandaStatus.Aprovada)
                        NotificarAprovador(demanda, userId);
                    break;
            }
        }

        public void NotificarSuperior(Demanda demanda)
        {
            var titulo = $"Nova Demanda para Pré-Aprovação:\"{demanda.Titulo}\"";

            var body =
                $"O usuário {demanda.Criador.NomeCompleto} enviou a demanda \"{demanda.Titulo}\" para Pré-Aprovação pelo seu superior direto. Clique abaixo para mais detalhes.";

            mailer.SendMailBase(demanda.SuperiorDireto, titulo, body,
                ("Ver Demanda", $"/dashboard/demanda/{demanda.Id}"));
        }

        public void NotificarReprovacao(Demanda demanda, Models.ApplicationUser avaliador)
        {
            var titulo = $"Foram solicitados ajustes para o \"{demanda.Titulo}\" na etapa de Revisão";

            var body =
                $"O usuário {avaliador.NomeCompleto} revisor da sua demanda, inseriu alguns comentários e solicitou alterações no projeto. Clique abaixo para mais detalhes e enviar novamentepara revisão";

            mailer.SendMailBase(demanda.Criador, titulo, body, ("Ver Demanda", $"/dashboard/demanda/{demanda.Id}"));
        }

        public void NotificarReprovacaoPermanente(Demanda demanda, Models.ApplicationUser avaliador)
        {
            var titulo =
                $"Sua demanda \"{demanda.Titulo}\" foi reprovada e arquivada na etapa de Revisão. Nova Demanda para Pré-Aprovação:";

            var body =
                $"O usuário {avaliador.NomeCompleto} revisor da sua demanda, reprovou e arquivou sua demanda . Clique abaixo para mais detalhes:";

            mailer.SendMailBase(demanda.Criador, titulo, body, ("Ver Demanda", $"/dashboard/demanda/{demanda.Id}"));
        }

        public void NotificarRevisorPendente(Demanda demanda)
        {
            var Coordenador = _context.Users.Find(sistemaService.GetEquipePeD().Coordenador);
            var titulo = $"Nova Demanda para Definição de Revisor: \"{demanda.Titulo}\"";

            var body =
                $"O usuário {demanda.Criador.NomeCompleto} cadastrou uma nova demanda \"{demanda.Titulo}\" que já foi pré-aprovada pelo seu superior direto. Precisamos agora que seja definido o revisor responsável pela demanda. Clique abaixo para mais detalhes.";

            mailer.SendMailBase(Coordenador, titulo, body, ("Ver Demanda", $"/dashboard/demanda/{demanda.Id}"));
        }

        public void NotificarRevisor(Demanda demanda)
        {
            var Coordenador = _context.Users.Find(sistemaService.GetEquipePeD().Coordenador);
            var titulo = $"Nova Demanda para Revisão: \"{demanda.Titulo}\"";

            var body =
                $"O usuário {Coordenador.NomeCompleto} enviou a demanda \"{demanda.Titulo}\" para Revisão. Clique abaixo para mais detalhes.";

            mailer.SendMailBase(demanda.Revisor, titulo, body, ("Ver Demanda", $"/dashboard/demanda/{demanda.Id}"));
        }

        public void NotificarAprovador(Demanda demanda, string avaliadorAnteriorId)
        {
            var avaliador = _context.Users.Find(avaliadorAnteriorId);
            var responsavel = _context.Users.Find(GetResponsavelAtual(demanda));
            var titulo = $"Nova Demanda para Aprovação: \"{demanda.Titulo}\"";
            var body =
                $"O usuário {avaliador.NomeCompleto} enviou a demanda \"{demanda.Titulo}\" para Aprovação. Clique abaixo para mais detalhes.";
            mailer.SendMailBase(responsavel, titulo, body, ("Ver Demanda", $"/dashboard/demanda/{demanda.Id}"));
        }

        #endregion

        #region Logs da demanda

        public List<DemandaLog> GetDemandaLogs(int demandaId)
        {
            return _context.DemandaLogs.Include("User").Where(dl => dl.DemandaId == demandaId).OrderBy(dl => dl.Id)
                .ToList();
        }

        #endregion
    }

    public static class DemandaExtension
    {
        public static IQueryable<Demanda> ByUser(this IQueryable<Demanda> dbSet, string userId)
        {
            return dbSet.Where(demanda =>
                String.IsNullOrWhiteSpace(userId) || demanda.CriadorId == userId ||
                demanda.SuperiorDiretoId == userId || demanda.RevisorId == userId);
        }
    }
}