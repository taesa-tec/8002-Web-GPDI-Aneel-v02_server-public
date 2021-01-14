using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Data;
using PeD.Models.Projetos;
using PeD.Models.Projetos.Xmls;

namespace PeD.Services.Projetos.XmlProjeto
{
    public class XmlRelatorioAuditoriaGestaoService : IXmlService<XmlRelatorioAuditoriaGestao>
    {
        private GestorDbContext _context;
        public XmlRelatorioAuditoriaGestaoService(GestorDbContext context)
        {
            _context = context;
        }
        public Resultado ValidaXml(int ProjetoId)
        {
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            // if (projeto.Codigo == null)
            //     resultado.Inconsistencias.Add("Código do Projeto não gerado");
            // int Duracao = projeto.Etapas.Sum(p => p.Duracao);
            // if ((projeto.TipoValor == "PD" && Duracao > 60) || (projeto.TipoValor == "PG" && Duracao > 12))
            //     resultado.Inconsistencias.Add("Duração máxima execedida para o projeto");
            return resultado;
        }
        public XmlRelatorioAuditoriaGestao GerarXml(int ProjetoId, string Versao, string UserId)
        {
            XmlRelatorioAuditoriaGestao relatorio = new XmlRelatorioAuditoriaGestao();
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Include("Empresas.Estado")
                    .Include("Etapas")
                    .Include("RegistroFinanceiro.RecursoHumano")
                    .Include("RegistroFinanceiro.RecursoMaterial.CategoriaContabilGestao")
                    .Include("Empresas.CatalogEmpresa")
                    .Include("RelatorioFinal.Uploads")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();


            decimal? TotalRh = projeto.RegistroFinanceiro.Where(r => r.StatusValor == "Aprovado" && r.QtdHrs != null && r.RecursoHumano != null).Sum(r => r.QtdHrs * r.RecursoHumano.ValorHora);
            decimal? TotalRm = projeto.RegistroFinanceiro.Where(r => r.StatusValor == "Aprovado" && r.QtdItens != null && r.RecursoMaterial != null).Sum(r => r.QtdItens * r.ValorUnitario);

            relatorio.PD_RelAuditoriaPG = new PD_RelAuditoriaPG
            {
                CodProjeto = projeto.Codigo,
                ArquivoPDF = projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAuditoria").FirstOrDefault().NomeArquivo,
                CustoTotal = (TotalRh + TotalRm).ToString()
            };
            var EmpresasFinanciadoras = projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                .ToList();

            // RecursoEmpresa
            var ListRecursoEmpresa = new List<RAG_RecursoEmpresa>();
            foreach (Empresa empresa in EmpresasFinanciadoras)
            {
                var RAG_CustoCatContabil = new List<RAG_CustoCatContabil>();
                //RH
                foreach (var rh in projeto.RegistroFinanceiro
                        .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                        .Where(p => p.RecursoHumano != null)
                        .Where(p => p.StatusValor == "Aprovado")
                        .GroupBy(p => p.EmpresaFinanciadora)
                        .ToList())
                {
                    decimal? custo = 0;
                    foreach (var a in rh)
                    {
                        custo += a.RecursoHumano.ValorHora * ((a.QtdHrs));

                    }
                    RAG_CustoCatContabil.Add(new RAG_CustoCatContabil
                    {
                        CatContabil = "RH",
                        CustoEmpresa = custo.ToString()
                    });
                }
                // RM
                foreach (var rm in projeto.RegistroFinanceiro
                        .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                        .Where(p => p.RecursoMaterial != null)
                        .Where(p => p.StatusValor == "Aprovado")
                        .GroupBy(p => p.EmpresaFinanciadora)
                        .ToList())
                {
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabilGestao.Valor))
                    {
                        decimal? custo = 0;
                        foreach (var rm1 in rm0)
                        {
                            custo += rm1.ValorUnitario * rm1.QtdItens;
                        }
                        RAG_CustoCatContabil.Add(new RAG_CustoCatContabil
                        {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilGestao.Valor,
                            CustoEmpresa = custo.ToString()
                        });
                    }
                }

                ListRecursoEmpresa.Add(new RAG_RecursoEmpresa
                {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    CustoCatContabil = RAG_CustoCatContabil
                });
            }
            relatorio.PD_RelAuditoriaPG.RecursoEmpresa = ListRecursoEmpresa;

            return relatorio;
        }
    }
}
