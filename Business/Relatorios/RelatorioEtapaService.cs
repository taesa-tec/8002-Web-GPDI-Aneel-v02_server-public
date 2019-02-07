
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            RelatorioEtapa relatorio = new RelatorioEtapa();
            var Etapas = _context.Etapas
                .Where(p => p.ProjetoId == projetoId)
                .OrderBy(p=>p.Id)
                .ToList();

            relatorio.Etapas = new List<RelatorioEtapas>();
            relatorio.Total = Etapas.Count();
            relatorio.Valor = 0;
            var i = 1;
            foreach(Etapa Etapa in Etapas){
                decimal ValorEtapa = 0;
                var empresas = new List<RelatorioEtapaEmpresas>();
                string nomeEtapa = "Etapa "+i;
                
                foreach(RelatorioEtapaEmpresas empresa in empresas)
                {   
                    //obter alocações recursos humanos
                    var data = new List<RelatorioEtapaItems>();
                    int total = 0;
                    decimal ValorCategoria = 0;
                    
                        var AlocacoesRh = _context.AlocacoesRh
                            .Where(p=>p.EtapaId == Etapa.Id)
                            .Include("RecursoHumano")
                            .ToList();
                        total = AlocacoesRh.Count();
                        if (total>0){
                            foreach(AlocacaoRh a in AlocacoesRh){
                                decimal valor = (a.HrsMes1+a.HrsMes2+a.HrsMes3+a.HrsMes4+a.HrsMes5+a.HrsMes6)*a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioEtapaItems
                                    {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoHumano.NomeCompleto,
                                        //Etapa = a.Etapa,
                                        Valor = valor
                                    });
                                ValorCategoria += valor;
                            }
                        }
                    // Fim RH
               
                        // Outras Categorias
                        var AlocacoesRm = _context.AlocacoesRm
                        //.Where(p=>p.EmpresaFinanciadoraId == empresa.Id)
                        .Include(p=>p.RecursoMaterial)
                        //.Where(p=>p.RecursoMaterial.CategoriaContabil==categoria)
                        .Include("Etapa.EtapaProdutos")
                        .ToList();
                        total = AlocacoesRm.Count();
                        if (total>0){
                            foreach(AlocacaoRm a in AlocacoesRm){
                                decimal valor = (a.Qtd)*a.RecursoMaterial.ValorUnitario;
                                data.Add(new RelatorioEtapaItems
                                    {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoMaterial.Nome,
                                       // Etapa = a.Etapa,
                                        Valor = valor
                                    });
                                ValorCategoria += valor;
                            }
                            
                        }
                    
                    if (total>0){
                        // categorias.Add(new RelatorioEtapaEmpresas{
                        //     CategoriaContabil = categoria,
                        //     Desc = categoria.ToString(),
                        //     Items = data,
                        //     Total = total,
                        //     Valor = ValorCategoria
                        // });
                    }
                    ValorEtapa += ValorCategoria;
                }
                // Fim Outros Relatorios
                // relatorio.Etapas.Add(new RelatorioEtapas
                //     {
                //         Nome = nomeEtapa,
                //         Relatorios = categorias,
                //         Total = categorias.Count(),
                //         Valor = ValorEtapa
                //     });
                relatorio.Valor += ValorEtapa;
                i++;
            }
            return relatorio;
        }
    }
}