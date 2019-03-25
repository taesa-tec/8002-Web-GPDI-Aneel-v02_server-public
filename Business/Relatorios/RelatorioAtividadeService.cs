
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;

namespace APIGestor.Business
{
    public class RelatorioAtividadeService
    {
        private GestorDbContext _context;

        public RelatorioAtividadeService(GestorDbContext context)
        {
            _context = context;
        }
        public RelatorioAtividade ExtratoFinanceiro(int projetoId)
        {
            var atividadeRh = _context.CatalogCategoriaContabilGestao.Include("Atividades").Where(c=>c.Valor=="RH").First().Atividades.First();
            var AlocacoesRh = _context.AlocacoesRh
                .Include("CatalogEmpresa")
                .Where(m => m.ProjetoId == projetoId)
                .Select(m=>new { 
                        AtividadeId = atividadeRh.Id,
                        Atividade = atividadeRh,
                        Empresa = m.Empresa,
                        CatalogEmpresa = m.Empresa.CatalogEmpresa
                        })
                .ToList();
            var AlocacoesRm = _context.AlocacoesRm
                .Include("CatalogEmpresa")
                .Include("RecursoMaterial.Atividade")
                .Where(m => m.ProjetoId == projetoId)
                .Where(m => m.RecursoMaterial.Atividade != null)
                .Select(m=>new { 
                        AtividadeId = m.RecursoMaterial.Atividade.Id,
                        Atividade = m.RecursoMaterial.Atividade,
                        Empresa = m.EmpresaFinanciadora,
                        CatalogEmpresa = m.EmpresaFinanciadora.CatalogEmpresa
                    })
                .ToList(); 
            var Atividades = AlocacoesRh
                .Concat(AlocacoesRm)
                .OrderBy(p=>p.AtividadeId);

            var RelatorioAtividades =Atividades
                .GroupBy(p=>p.Atividade.Valor)
                .Distinct()
                .ToList();
            
            RelatorioAtividade relatorio = new RelatorioAtividade();
            relatorio.Atividades = new List<RelatorioAtividades>();
            relatorio.Total = RelatorioAtividades.Count();
            relatorio.Valor = 0;
            var i = 1;
            foreach(var Atividade in  RelatorioAtividades){
                decimal? ValorAtividade = 0;
                string nomeAtividade = Atividade.First().Atividade.Nome;

                //var empresas = new List<RelatorioAtividadeEmpresas>();
                var empresas = Atividades
                            .Where(e=>e.Atividade.Valor == Atividade.First().Atividade.Valor)
                            .GroupBy(e=>e.Empresa);

                var RelatorioAtividadeEmpresas = new List<RelatorioAtividadeEmpresas>();
                foreach(var empresa in empresas)
                {   
                    decimal? ValorEmpresa = 0;
                    string nomeEmpresa = null;
                    if (empresa.First().Empresa.CatalogEmpresaId>0)
                        nomeEmpresa = empresa.First().Empresa.CatalogEmpresa.Nome;
                    else
                        nomeEmpresa = empresa.First().Empresa.RazaoSocial;
                    
                    var data = new List<RelatorioAtividadeItems>();
                    foreach(CatalogCategoriaContabilGestao categoria in _context.CatalogCategoriaContabilGestao.ToList())
                    {   
                        int total = 0;
                        if (categoria.Valor.ToString()=="RH"){
                        //obter alocações recursos humanos                    
                        var rhs = _context.AlocacoesRh
                            .Where(p=>p.EmpresaId == empresa.First().Empresa.Id)
                            .Where(p=>p.RecursoHumano!=null)
                            .Include("RecursoHumano")
                            .ToList();
                        total = AlocacoesRh.Count();
                        if (total>0){
                            foreach(AlocacaoRh a in rhs){
                                decimal? valor = ((a.HrsMes1+a.HrsMes2+a.HrsMes3+a.HrsMes4+a.HrsMes5+a.HrsMes6)+(a.HrsMes7+a.HrsMes8+a.HrsMes9+a.HrsMes10+a.HrsMes11+a.HrsMes12)+(a.HrsMes13+a.HrsMes14+a.HrsMes15+a.HrsMes16+a.HrsMes17+a.HrsMes18)+(a.HrsMes19+a.HrsMes20+a.HrsMes21+a.HrsMes22+a.HrsMes23+a.HrsMes24))*a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioAtividadeItems
                                    {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoHumano.NomeCompleto,
                                        RecursoHumano = a.RecursoHumano,
                                        CategoriaContabil = categoria.Valor.ToString(),
                                        Valor = valor
                                    });
                                ValorEmpresa += valor;
                            }
                        }
                        // Fim RH
                        }else{
                        // Outras Categorias
                            var rms = _context.AlocacoesRm
                                .Where(p=>p.RecursoMaterial!=null)
                                .Where(p=>p.RecursoMaterial.Atividade.Valor == Atividade.First().Atividade.Valor)
                                .Where(p=>p.EmpresaFinanciadoraId == empresa.First().Empresa.Id)
                                .Include(p=>p.RecursoMaterial.CategoriaContabilGestao)
                                .Include(p=>p.RecursoMaterial.Atividade)
                                .Where(p=>p.RecursoMaterial.CategoriaContabilGestao.Valor==categoria.Valor)
                                .ToList();
                            total = AlocacoesRm.Count();
                            if (total>0){
                                foreach(AlocacaoRm a in rms){
                                    decimal valor = (a.Qtd)*a.RecursoMaterial.ValorUnitario;
                                    data.Add(new RelatorioAtividadeItems
                                        {
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
                    if (data.Count()>0){
                        RelatorioAtividadeEmpresas.Add(new RelatorioAtividadeEmpresas{
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
                relatorio.Atividades.Add(new RelatorioAtividades
                    {
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