using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace APIGestor.Business
{
    public class GeradorXmlService
    {
        private GestorDbContext _context;

        private XmlProjetoPedService _projetoPed;
        private XmlInteressePedService _interessePed;
        private XmlInicioExecService _inicioExec;
        private XmlProrrogacaoService _prorrogacaoPed;
        private XmlRelatorioFinalService _relatorioFinalPed;
        private XmlRelatorioAuditoriaService _relatorioAuditoriaPed;
        private IHostingEnvironment _hostingEnvironment;
        public GeradorXmlService(
            GestorDbContext context,
            IHostingEnvironment hostingEnvironment,
            XmlProjetoPedService projetoPed,
            XmlInteressePedService interessePed,
            XmlInicioExecService inicioExec,
            XmlProrrogacaoService prorrogacaoPed,
            XmlRelatorioFinalService relatorioFinalPed,
            XmlRelatorioAuditoriaService relatorioAuditoriaPed)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _projetoPed = projetoPed;
            _interessePed = interessePed;
            _inicioExec = inicioExec;
            _prorrogacaoPed = prorrogacaoPed;
            _relatorioFinalPed = relatorioFinalPed;
            _relatorioAuditoriaPed = relatorioAuditoriaPed;
        }
        public IEnumerable<Upload> ObterXmls(int projetoId)
        {
            var Upload = _context.Uploads
                .Include("User")
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Categoria == (CategoriaUpload)3)
                .Select(p => new Upload
                {
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
        public Resultado CriarArquivo(string XmlDoc, string Tipo, int ProjetoId, string Versao, string UserId)
        {
            var resultado = new Resultado();
            Projeto Projeto = _context.Projetos.Include("CatalogEmpresa").FirstOrDefault(p => p.Id == ProjetoId);
            if (Projeto == null)
            {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            else
            {
                XDocument xDoc = XDocument.Parse(JsonConvert.DeserializeXmlNode(XmlDoc, "PED").InnerXml);
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

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var upload = new Upload
                {
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
                // using (var writer = new XmlTextWriter(fullPath, new UTF8Encoding(true)))
                // {
                //     writer.Formatting = System.Xml.Formatting.Indented;
                //     xDoc.Save(writer);
                // }
                xDoc.Save(fullPath);
            }
            return resultado;
        }
        public Resultado DadosValidos(int ProjetoId, XmlTipo XmlTipo, string Versao, string UserId)
        {
            var resultado = new Resultado();
            if (ProjetoId <= 0)
                resultado.Inconsistencias.Add("Informe o ProjetoId");
            if (XmlTipo.ToString() == null || !Enum.IsDefined(typeof(XmlTipo), XmlTipo))
                resultado.Inconsistencias.Add("Informe o XmlTipo");
            else if (_context.Projetos.Where(p => p.Id == ProjetoId).FirstOrDefault() == null)
                resultado.Inconsistencias.Add("ProjetoId não localizado");
            if (Versao == null)
                resultado.Inconsistencias.Add("Informe a Versão");
            if (UserId == null)
                resultado.Inconsistencias.Add("UserId Não localizado");
            return resultado;
        }
        public Resultado GerarXml(int ProjetoId, XmlTipo XmlTipo, string Versao, string UserId)
        {
            Resultado resultado = DadosValidos(ProjetoId, XmlTipo, Versao, UserId);
            if (resultado.Inconsistencias.Count == 0)
            {
                var svc = (dynamic)null;
                switch (XmlTipo.ToString())
                {
                    //case "MOVIMENTACAOFINANCEIRA":
                    //case "PROGRAMA": 
                    //case "PROJETOGESTAO":
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
                    //case "RELATORIOFINALGESTAO":
                    case "RELATORIOAUDITORIAPED":
                        svc = _relatorioAuditoriaPed;
                        break;
                        //case "RELATORIOAUDITORIAGESTAO": 
                }
                var xml = svc.GerarXml(ProjetoId, Versao, UserId);
                if (xml!=null){
                    resultado = CriarArquivo(JsonConvert.SerializeObject(xml), XmlTipo.ToString(), ProjetoId, Versao, UserId);
                }else{
                    resultado.Inconsistencias.Add("Erro na gravação do arquivo");
                }
                resultado.Acao = "Geração Xml " + XmlTipo.ToString();
            }
            return resultado;
        }
        public Resultado ValidaDados(int ProjetoId, XmlTipo XmlTipo)
        {
            Resultado resultado = new Resultado();
             if (ProjetoId <= 0)
                resultado.Inconsistencias.Add("Informe o ProjetoId");
            if (XmlTipo.ToString() == null || !Enum.IsDefined(typeof(XmlTipo), XmlTipo))
                resultado.Inconsistencias.Add("Informe o XmlTipo");
            if (resultado.Inconsistencias.Count == 0)
            {
                var svc = (dynamic)null;
                switch (XmlTipo.ToString())
                {
                    case "PROJETOPED":
                        svc = _projetoPed;
                    break;
                }
                Projeto Projeto = svc.obterProjeto(ProjetoId);
                resultado = svc.ValidaXml(Projeto);
                resultado.Acao = "Validação dados Xml " + XmlTipo.ToString();
            }
            return resultado;
        }

    }
}
