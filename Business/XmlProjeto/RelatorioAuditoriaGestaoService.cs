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
    public class XmlRelatorioAuditoriaGestaoService
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
                    .Include("AlocacoesRh.RecursoHumano")
                    .Include("AlocacoesRm.RecursoMaterial.CategoriaContabilGestao")
                    .Include("Empresas.CatalogEmpresa")
                    .Include("RelatorioFinal.Uploads")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();

            var registros = _context.RegistrosFinanceiros
                            .Include("RecursoHumano")
                            .Include("RecursoMaterial.CategoriaContabilGestao")
                            .Where(p => p.ProjetoId == ProjetoId)
                            .Where(p => p.StatusValor == "Aprovado")
                            .ToList();
            int?[] rhIds = registros.Where(r => r.RecursoHumano != null).Select(r => r.RecursoHumanoId).ToArray();
            int?[] rmIds = registros.Where(r => r.RecursoMaterial != null).Select(r => r.RecursoMaterialId).ToArray();

            decimal? TotalRh = registros.Where(r => r.QtdHrs != null && r.RecursoHumano != null).Sum(r => r.QtdHrs * r.RecursoHumano.ValorHora);
            decimal? TotalRm = registros.Where(r => r.QtdItens != null && r.RecursoMaterial != null).Sum(r => r.QtdItens * r.RecursoMaterial.ValorUnitario);

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
                foreach (var rh in projeto.AlocacoesRh
                        .Where(p => p.Empresa == empresa)
                        .Where(p => rmIds.Contains(p.RecursoHumano.Id))
                        .GroupBy(p => p.Empresa)
                        .ToList())
                {
                    decimal? custo = 0;
                    foreach (var a in rh)
                    {
                        custo += a.RecursoHumano.ValorHora * ((a.HrsMes1+a.HrsMes2+a.HrsMes3+a.HrsMes4+a.HrsMes5+a.HrsMes6)+(a.HrsMes7+a.HrsMes8+a.HrsMes9+a.HrsMes10+a.HrsMes11+a.HrsMes12)+(a.HrsMes13+a.HrsMes14+a.HrsMes15+a.HrsMes16+a.HrsMes17+a.HrsMes18)+(a.HrsMes19+a.HrsMes20+a.HrsMes21+a.HrsMes22+a.HrsMes23+a.HrsMes24));

                    }
                    RAG_CustoCatContabil.Add(new RAG_CustoCatContabil
                    {
                        CatContabil = "RH",
                        CustoEmpresa = custo.ToString()
                    });
                }
                // RM
                foreach (var rm in projeto.AlocacoesRm
                        .Where(p => p.EmpresaRecebedora == empresa)
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList())
                {
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabilGestao.Valor))
                    {
                        decimal custo = 0;
                        foreach (var rm1 in rm0)
                        {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
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
