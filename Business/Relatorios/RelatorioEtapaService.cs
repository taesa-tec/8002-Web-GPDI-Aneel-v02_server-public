
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
    public class RelatorioEtapaService
    {
        private GestorDbContext _context;

        public RelatorioEtapaService(GestorDbContext context)
        {
            _context = context;
        }
        public RelatorioEtapa ExtratoFinanceiro(int projetoId)
        {
            var AlocacoesRh = _context.AlocacoesRh
                .Include("CatalogEmpresa")
                .Where(m => m.ProjetoId == projetoId)
                .Select(m=>new { 
                        EtapaId = m.EtapaId,
                        Etapa = m.Etapa,
                        Empresa = m.Empresa,
                        CatalogEmpresa = m.Empresa.CatalogEmpresa
                        })
                .ToList();
            var AlocacoesRm = _context.AlocacoesRm
                .Include("CatalogEmpresa")
                .Where(m => m.ProjetoId == projetoId)
                .Select(m=>new { 
                        EtapaId = m.EtapaId,
                        Etapa = m.Etapa,
                        Empresa = m.EmpresaFinanciadora,
                        CatalogEmpresa = m.EmpresaFinanciadora.CatalogEmpresa
                    })
                .ToList(); 
            var Etapas = AlocacoesRh
                .Concat(AlocacoesRm)
                .OrderBy(p=>p.EtapaId);

            var RelatorioEtapas =Etapas
                .GroupBy(p=>p.Etapa)
                .Distinct()
                .ToList();
            
            RelatorioEtapa relatorio = new RelatorioEtapa();
            relatorio.Etapas = new List<RelatorioEtapas>();
            relatorio.Total = RelatorioEtapas.Count();
            relatorio.Valor = 0;
            var i = 1;
            foreach(var Etapa in  RelatorioEtapas){
                decimal ValorEtapa = 0;
                string nomeEtapa = "Etapa "+i;

                //var empresas = new List<RelatorioEtapaEmpresas>();
                var empresas = Etapas
                            .Where(e=>e.EtapaId == Etapa.First().EtapaId)
                            .GroupBy(e=>e.Empresa);

                var RelatorioEtapaEmpresas = new List<RelatorioEtapaEmpresas>();
                foreach(var empresa in empresas)
                {   
                    decimal ValorEmpresa = 0;
                    string nomeEmpresa = null;
                    if (empresa.First().Empresa.CatalogEmpresaId>0)
                        nomeEmpresa = empresa.First().Empresa.CatalogEmpresa.Nome;
                    else
                        nomeEmpresa = empresa.First().Empresa.RazaoSocial;
                    
                    var data = new List<RelatorioEtapaItems>();
                    foreach(CategoriaContabil categoria in CategoriaContabil.GetValues(typeof(CategoriaContabil)))
                    {   
                        int total = 0;
                        if (categoria.ToString()=="RH"){
                        //obter alocações recursos humanos                    
                        var rhs = _context.AlocacoesRh
                            .Where(p=>p.EtapaId == Etapa.First().EtapaId)
                            .Include("RecursoHumano")
                            .ToList();
                        total = AlocacoesRh.Count();
                        if (total>0){
                            foreach(AlocacaoRh a in rhs){
                                decimal valor = (a.HrsMes1+a.HrsMes2+a.HrsMes3+a.HrsMes4+a.HrsMes5+a.HrsMes6)*a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioEtapaItems
                                    {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoHumano.NomeCompleto,
                                        CategoriaContabil = categoria.ToString(),
                                        Valor = valor
                                    });
                                ValorEmpresa += valor;
                            }
                        }
                        // Fim RH
                        }else{
                        // Outras Categorias
                            var rms = _context.AlocacoesRm
                                .Where(p=>p.EmpresaFinanciadoraId == empresa.First().Empresa.Id)
                                .Include(p=>p.RecursoMaterial)
                                .Where(p=>p.RecursoMaterial.CategoriaContabil==categoria)
                                .Include("Etapa.EtapaProdutos")
                                .ToList();
                            total = AlocacoesRm.Count();
                            if (total>0){
                                foreach(AlocacaoRm a in rms){
                                    decimal valor = (a.Qtd)*a.RecursoMaterial.ValorUnitario;
                                    data.Add(new RelatorioEtapaItems
                                        {
                                            AlocacaoId = a.Id,
                                            Desc = a.RecursoMaterial.Nome,
                                            CategoriaContabil = categoria.ToString(),
                                            Valor = valor
                                        });
                                    ValorEmpresa += valor;
                                }
                            }
                        }   
                    }
                    if (data.Count()>0){
                        RelatorioEtapaEmpresas.Add(new RelatorioEtapaEmpresas{
                            Desc = nomeEmpresa,
                            Empresa = empresa.First().Empresa,
                            Items = data,
                            Total = data.Count(),
                            Valor = ValorEmpresa
                        });
                    }
                    ValorEtapa += ValorEmpresa;
                }
                //Fim Outros Relatorios
                relatorio.Etapas.Add(new RelatorioEtapas
                    {
                        Nome = nomeEtapa,
                        Etapa = Etapa.First().Etapa,
                        Empresas = RelatorioEtapaEmpresas,
                        Total = RelatorioEtapaEmpresas.Count(),
                        Valor = ValorEtapa
                    });
                relatorio.Valor += ValorEtapa;
                i++;
            }
            return relatorio;
        }
    }
}