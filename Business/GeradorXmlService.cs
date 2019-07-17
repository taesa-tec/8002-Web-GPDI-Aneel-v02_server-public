using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace APIGestor.Business {
    public class GeradorXmlService : BaseGestorService {


        private XmlProjetoPedService _projetoPed;
        private XmlInteressePedService _interessePed;
        private XmlInicioExecService _inicioExec;
        private XmlProrrogacaoService _prorrogacaoPed;
        private XmlRelatorioFinalService _relatorioFinalPed;
        private XmlRelatorioAuditoriaService _relatorioAuditoriaPed;
        private XmlRelatorioFinalGestaoService _relatorioFinalGestao;
        private XmlRelatorioAuditoriaGestaoService _relatorioAuditoriaGestao;
        private XmlProjetoGestaoService _projetoGestao;
        private IHostingEnvironment _hostingEnvironment;

        public GeradorXmlService(
            GestorDbContext context,
            IHostingEnvironment hostingEnvironment,
            XmlProjetoPedService projetoPed,
            XmlInteressePedService interessePed,
            XmlInicioExecService inicioExec,
            XmlProrrogacaoService prorrogacaoPed,
            XmlRelatorioFinalService relatorioFinalPed,
            XmlRelatorioAuditoriaService relatorioAuditoriaPed,
            XmlProjetoGestaoService projetoGestao,
            XmlRelatorioFinalGestaoService relatorioFinalGestao,
            XmlRelatorioAuditoriaGestaoService relatorioAuditoriaGestao,
            IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) {

            _hostingEnvironment = hostingEnvironment;
            _projetoPed = projetoPed;
            _interessePed = interessePed;
            _inicioExec = inicioExec;
            _prorrogacaoPed = prorrogacaoPed;
            _relatorioFinalPed = relatorioFinalPed;
            _relatorioAuditoriaPed = relatorioAuditoriaPed;
            _projetoGestao = projetoGestao;
            _relatorioFinalGestao = relatorioFinalGestao;
            _relatorioAuditoriaGestao = relatorioAuditoriaGestao;
        }
        public IEnumerable<Upload> ObterXmls( int projetoId ) {
            var Upload = _context.Uploads
                .Include("User")
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Categoria == (CategoriaUpload)3)
                .Select(p => new Upload {
                    Id = p.Id,
                    NomeArquivo = p.NomeArquivo,
                    ProjetoId = p.ProjetoId,
                    TemaId = p.TemaId,
                    RegistroFinanceiroId = p.RegistroFinanceiroId,
                    Categoria = p.Categoria,
                    UserId = p.UserId,
                    User = new ApplicationUser { NomeCompleto = p.User.NomeCompleto },
                    Created = p.Created
                })
                .ToList();
            return Upload;
        }
        public Resultado CriarArquivo( string XmlDoc, string Tipo, int ProjetoId, string Versao, string UserId ) {
            var resultado = new Resultado();
            Projeto Projeto = _context.Projetos.Include("CatalogEmpresa").FirstOrDefault(p => p.Id == ProjetoId);
            if(Projeto == null) {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            else {
                string innerXml = JsonConvert.DeserializeXmlNode(XmlDoc, "PED").InnerXml;

                innerXml = Regex.Replace(innerXml, "\u2013", "-");
                innerXml = Regex.Replace(innerXml, "\u201C", "\"");
                innerXml = Regex.Replace(innerXml, "\u201D", "\"");

                XDocument xDoc = XDocument.Parse(innerXml);
                xDoc.Declaration = new XDeclaration("1.0", "ISO8859-1", null);
                xDoc.Root.SetAttributeValue("Tipo", Tipo);
                xDoc.Root.SetAttributeValue("CodigoEmpresa", Projeto.CatalogEmpresa.Valor);
                xDoc.Descendants()
                    .Where(a => a.IsEmpty || String.IsNullOrWhiteSpace(a.Value))
                    .Remove();

                string folderName = "uploads/xmls/";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                string fileName = "APLPED" + Projeto.CatalogEmpresa.Valor + "_" + Tipo + "_" + Projeto.Numero + "_" + Versao + ".XML";

                if(!Directory.Exists(newPath)) {
                    Directory.CreateDirectory(newPath);
                }

                var upload = new Upload {
                    NomeArquivo = fileName,
                    UserId = UserId,
                    Url = "wwwroot/" + folderName,
                    ProjetoId = Projeto.Id,
                    Categoria = CategoriaUpload.XmlGerado
                };
                _context.Uploads.Add(upload);
                _context.SaveChanges();
                resultado.Id = upload.Id.ToString();
                string fullPath = Path.Combine(newPath, upload.Id.ToString());
                //using (var writer = new XmlTextWriter( fullPath, new UTF8Encoding(true)))
                // {
                //     writer.Formatting = System.Xml.Formatting.Indented;
                //     xDoc.Save(writer);
                // }
                xDoc.Save(fullPath);

                // My little gambi
                // Um fihote de gambiarra abaixo:
                string[] lines = File.ReadAllLines(fullPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                lines[0] = Regex.Replace(lines[0], "encoding=\"iso-8859-1\"", "encoding=\"ISO8859-1\"");
                File.WriteAllLines(fullPath, lines, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
            return resultado;
        }
        public Resultado DadosValidos( int ProjetoId, XmlTipo XmlTipo, string Versao, string UserId ) {
            var resultado = new Resultado();
            try {
                if(ProjetoId <= 0)
                    resultado.Inconsistencias.Add("Informe o ProjetoId");
                if(XmlTipo.ToString() == null || !Enum.IsDefined(typeof(XmlTipo), XmlTipo))
                    resultado.Inconsistencias.Add("Informe o XmlTipo");
                else if(_context.Projetos.Where(p => p.Id == ProjetoId).FirstOrDefault() == null)
                    resultado.Inconsistencias.Add("ProjetoId não localizado");
                if(Versao == null)
                    resultado.Inconsistencias.Add("Informe a Versão");
                if(UserId == null)
                    resultado.Inconsistencias.Add("UserId Não localizado");
            }
            catch(Exception ex) {
                resultado.Inconsistencias.Add(ex.Message);
            }

            return resultado;
        }
        public Resultado GerarXml( int ProjetoId, XmlTipo XmlTipo, string Versao, string UserId ) {
            Resultado resultado = DadosValidos(ProjetoId, XmlTipo, Versao, UserId);
            if(resultado.Inconsistencias.Count == 0) {
                var svc = obterXmlTipo(XmlTipo);
                Resultado ValidaXml = svc.ValidaXml(ProjetoId);

                if(ValidaXml.Inconsistencias.Count() > 0)
                    return ValidaXml;

                var xml = svc.GerarXml(ProjetoId, Versao, UserId);

                if(xml != null) {
                    resultado = CriarArquivo(JsonConvert.SerializeObject(xml), XmlTipo.ToString(), ProjetoId, Versao, UserId);
                }
                else {
                    resultado.Inconsistencias.Add("Erro na gravação do arquivo");
                }

                resultado.Acao = "Geração Xml " + XmlTipo.ToString();
            }
            return resultado;
        }
        private dynamic obterXmlTipo( XmlTipo XmlTipo ) {
            var svc = (dynamic)null;
            switch(XmlTipo.ToString()) {
                //case "MOVIMENTACAOFINANCEIRA":
                //case "PROGRAMA": 
                case "PROJETOGESTAO":
                    svc = _projetoGestao;
                    break;
                case "PROJETOPED":
                    svc = _projetoPed;
                    break;
                case "INTERESSEPROJETOPED":
                    svc = _interessePed;
                    break;
                case "INICIOEXECUCAOPROJETO":
                    svc = _inicioExec;
                    break;
                case "PRORROGAEXECUCAOPROJETO":
                    svc = _prorrogacaoPed;
                    break;
                case "RELATORIOFINALPED":
                    svc = _relatorioFinalPed;
                    break;
                case "RELATORIOFINALGESTAO":
                    svc = _relatorioFinalGestao;
                    break;
                case "RELATORIOAUDITORIAPED":
                    svc = _relatorioAuditoriaPed;
                    break;
                case "RELATORIOAUDITORIAGESTAO":
                    svc = _relatorioAuditoriaGestao;
                    break;
            }
            return svc;
        }
        public Resultado ValidaDados( int ProjetoId, XmlTipo XmlTipo ) {
            Resultado resultado = new Resultado();
            if(ProjetoId <= 0)
                resultado.Inconsistencias.Add("Informe o ProjetoId");
            if(XmlTipo.ToString() == null || !Enum.IsDefined(typeof(XmlTipo), XmlTipo))
                resultado.Inconsistencias.Add("Informe o XmlTipo");
            if(resultado.Inconsistencias.Count == 0) {
                var svc = obterXmlTipo(XmlTipo);
                resultado = svc.ValidaXml(ProjetoId);
                resultado.Acao = "Validação dados Xml " + XmlTipo.ToString();
            }
            return resultado;
        }

    }
}
