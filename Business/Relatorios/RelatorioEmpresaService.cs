
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Text;
using System.Reflection;

namespace APIGestor.Business
{
    public class RelatorioEmpresaService
    {
        private GestorDbContext _context;

        public RelatorioEmpresaService(GestorDbContext context)
        {
            _context = context;
        }

        public RelatorioEmpresa ExportarRelatorio(int projetoId)
        {
            var relatorio = ExtratoFinanceiro(projetoId);
            return relatorio;
        }
        public RelatorioEmpresa ExtratoFinanceiro(int projetoId)
        {
            RelatorioEmpresa relatorio = new RelatorioEmpresa();
            var Empresas = _context.Empresas
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Classificacao.ToString() == "Proponente" || p.Classificacao.ToString() == "Energia" || p.Classificacao.ToString() == "Parceira")
                .Include("CatalogEmpresa")
                .ToList();

            relatorio.Empresas = new List<RelatorioEmpresas>();
            relatorio.Total = Empresas.Count();
            relatorio.Valor = 0;
            foreach (Empresa empresa in Empresas)
            {
                decimal ValorEmpresa = 0;
                var categorias = new List<RelatorioEmpresaCategorias>();
                string nomeEmpresa = null;
                if (empresa.CatalogEmpresaId > 0)
                    nomeEmpresa = empresa.CatalogEmpresa.Nome;
                else
                    nomeEmpresa = empresa.RazaoSocial;

                foreach (CategoriaContabil categoria in CategoriaContabil.GetValues(typeof(CategoriaContabil)))
                {
                    //obter alocações recursos humanos
                    var data = new List<RelatorioEmpresaItems>();
                    int total = 0;
                    decimal ValorCategoria = 0;
                    if (categoria.ToString() == "RH")
                    {
                        var AlocacoesRh = _context.AlocacoesRh
                            .Where(p => p.EmpresaId == empresa.Id)
                            .Include("RecursoHumano")
                            .Include("Etapa.EtapaProdutos")
                            .ToList();
                        total = AlocacoesRh.Count();
                        if (total > 0)
                        {
                            foreach (AlocacaoRh a in AlocacoesRh)
                            {
                                decimal valor = (a.HrsMes1 + a.HrsMes2 + a.HrsMes3 + a.HrsMes4 + a.HrsMes5 + a.HrsMes6) * a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioEmpresaItems
                                {
                                    AlocacaoId = a.Id,
                                    Desc = a.RecursoHumano.NomeCompleto,
                                    Etapa = a.Etapa,
                                    Valor = valor
                                });
                                ValorCategoria += valor;
                            }
                        }
                        // Fim RH
                    }
                    else
                    {
                        // Outras Categorias
                        var AlocacoesRm = _context.AlocacoesRm
                        .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                        .Include(p => p.RecursoMaterial)
                        .Where(p => p.RecursoMaterial.CategoriaContabil == categoria)
                        .Include("Etapa.EtapaProdutos")
                        .ToList();
                        total = AlocacoesRm.Count();
                        if (total > 0)
                        {
                            foreach (AlocacaoRm a in AlocacoesRm)
                            {
                                decimal valor = (a.Qtd) * a.RecursoMaterial.ValorUnitario;
                                data.Add(new RelatorioEmpresaItems
                                {
                                    AlocacaoId = a.Id,
                                    Desc = a.RecursoMaterial.Nome,
                                    Etapa = a.Etapa,
                                    Valor = valor
                                });
                                ValorCategoria += valor;
                            }

                        }
                    }
                    if (total > 0)
                    {
                        categorias.Add(new RelatorioEmpresaCategorias
                        {
                            CategoriaContabil = categoria,
                            Desc = categoria.ToString(),
                            Items = data,
                            Total = total,
                            Valor = ValorCategoria
                        });
                    }
                    ValorEmpresa += ValorCategoria;
                }
                // Fim Outros Relatorios
                relatorio.Empresas.Add(new RelatorioEmpresas
                {
                    Nome = nomeEmpresa,
                    Relatorios = categorias,
                    Total = categorias.Count(),
                    Valor = ValorEmpresa
                });
                relatorio.Valor += ValorEmpresa;
            }
            return relatorio;
        }
    }
}