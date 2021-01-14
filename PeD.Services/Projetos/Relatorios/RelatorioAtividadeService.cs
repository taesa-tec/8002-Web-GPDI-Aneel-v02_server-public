using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Catalogs;
using PeD.Core.Models.Projetos;
using PeD.Data;

namespace PeD.Services.Projetos.Relatorios {
    public class RelatorioAtividadeService {
        private GestorDbContext _context;
        protected RelatorioEmpresaService empresaService;

        public RelatorioAtividadeService(GestorDbContext context) {
            _context = context;
        }

        public RelatorioAtividades ExtratoFinanceiro(int projetoId) {
            var atividadeRh = _context.CatalogCategoriaContabilGestao.Include("Atividades").Where(c => c.Valor == "RH").First().Atividades.First();

            var AlocacoesRhContext = _context.AlocacoesRh
                .Include("Empresa.CatalogEmpresa")
                .Include("RecursoHumano")
                .Where(m => m.ProjetoId == projetoId)
                .ToList();

            var AlocacoesRh = AlocacoesRhContext.Select(m => new {
                AtividadeId = atividadeRh.Id,
                Atividade = atividadeRh,
                Empresa = m.Empresa,
                CatalogEmpresa = m.Empresa.CatalogEmpresa
            }).ToList();

            var AlocacoesRmContext = _context.AlocacoesRm
                .Include("EmpresaFinanciadora.CatalogEmpresa")
                .Include("RecursoMaterial.Atividade")
                .Where(m => m.ProjetoId == projetoId)
                .Where(m => m.RecursoMaterial.Atividade != null)
                .ToList();

            var AlocacoesRm = AlocacoesRmContext.Select(m => new {
                AtividadeId = m.RecursoMaterial.Atividade.Id,
                Atividade = m.RecursoMaterial.Atividade,
                Empresa = m.EmpresaFinanciadora,
                CatalogEmpresa = m.EmpresaFinanciadora.CatalogEmpresa
            }).ToList();

            var Atividades = AlocacoesRh
                .Concat(AlocacoesRm)
                .OrderBy(p => p.AtividadeId);

            var RelatorioAtividades = Atividades
                .GroupBy(p => p.Atividade.Valor)
                .Distinct()
                .ToList();

            var categorias = _context.CatalogCategoriaContabilGestao.Where(cc => cc.Valor != "RH").ToList();

            var i = 1;

            RelatorioAtividades relatorio = new RelatorioAtividades();
            relatorio.Atividades = new List<RelatorioAtividade>();
            relatorio.Total = RelatorioAtividades.Count();
            relatorio.Valor = 0;

            foreach (var Atividade in RelatorioAtividades) {
                decimal? ValorAtividade = 0;
                string nomeAtividade = Atividade.First().Atividade.Nome;

                var empresas = Atividades
                            .Where(e => e.Atividade.Valor == Atividade.First().Atividade.Valor)
                            .GroupBy(e => e.Empresa);

                var RelatorioAtividadeEmpresas = new List<RelatorioAtividadeEmpresas>();
                foreach (var empresa in empresas) {
                    decimal? ValorEmpresa = 0;

                    string nomeEmpresa = empresa.First().Empresa.NomeEmpresa;

                    var data = new List<RelatorioAtividadeItems>();

                    if (Atividade.First().Atividade.Valor == "HH") {
                        var rhs = AlocacoesRhContext
                                .Where(p => p.EmpresaId == empresa.First().Empresa.Id)
                                .Where(p => p.RecursoHumano != null)
                                .ToList();

                        if (AlocacoesRh.Count() > 0) {
                            foreach (AlocacaoRh a in rhs) {
                                decimal? valor = a.HrsTotais * a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioAtividadeItems {
                                    AlocacaoId = a.Id,
                                    Desc = a.RecursoHumano.NomeCompleto,
                                    RecursoHumano = a.RecursoHumano,
                                    CategoriaContabil = "RH",
                                    Valor = valor
                                });
                                ValorEmpresa += valor;
                            }
                        }
                    }
                    else {
                        foreach (CatalogCategoriaContabilGestao categoria in categorias) {
                            int total = 0;

                            // Outras Categorias
                            var rms = AlocacoesRmContext
                                .Where(p => p.RecursoMaterial != null)
                                .Where(p => p.RecursoMaterial.Atividade.Valor == Atividade.First().Atividade.Valor)
                                .Where(p => p.EmpresaFinanciadoraId == empresa.First().Empresa.Id)
                                .Where(p => p.RecursoMaterial.CategoriaContabilGestao.Valor == categoria.Valor)
                                .ToList();

                            total = AlocacoesRm.Count();

                            if (total > 0) {
                                foreach (AlocacaoRm a in rms) {
                                    decimal valor = (a.Qtd) * a.RecursoMaterial.ValorUnitario;
                                    data.Add(new RelatorioAtividadeItems {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoMaterial.Nome,
                                        RecursoMaterial = a.RecursoMaterial,
                                        CategoriaContabil = categoria.Valor.ToString(),
                                        Valor = valor
                                    });
                                    ValorEmpresa += valor;
                                }
                            }
                        }
                    }




                    if (data.Count() > 0) {
                        RelatorioAtividadeEmpresas.Add(new RelatorioAtividadeEmpresas {
                            Desc = nomeEmpresa,
                            Empresa = empresa.First().Empresa,
                            Items = data,
                            Total = data.Count(),
                            Valor = ValorEmpresa
                        });
                    }
                    ValorAtividade += ValorEmpresa;
                }
                //Fim Outros Relatorios
                relatorio.Atividades.Add(new RelatorioAtividade {
                    Nome = nomeAtividade,
                    Atividade = Atividade.First().Atividade,
                    Empresas = RelatorioAtividadeEmpresas,
                    Total = RelatorioAtividadeEmpresas.Count(),
                    Valor = ValorAtividade
                });
                relatorio.Valor += ValorAtividade;
                i++;
            }
            return relatorio;
        }
    }
}