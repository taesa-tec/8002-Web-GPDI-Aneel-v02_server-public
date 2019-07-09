using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;

namespace APIGestor.Business {
    public class EtapaService : BaseAuthorizationService {

        public EtapaService( GestorDbContext context, IAuthorizationService authorization ) : base(context, authorization) {
        }

        public IEnumerable<Etapa> ListarTodos( int projetoId ) {


            var TodasEtapas = _context.Etapas.ToList();

            var Etapas = _context.Etapas
                .Include("EtapaProdutos")
                .Include("EtapaMeses")
                .Where(p => p.ProjetoId == projetoId)
                .OrderBy(p => p.Id)
                .ToList();

            if(Etapas.Count > 0)
                return AddDataEtapas(Etapas);

            return null;
        }

        private string NomeEtapa( int projetoId ) {
            int total = _context.Etapas
                                .Where(p => p.ProjetoId == projetoId)
                                .OrderBy(p => p.Id)
                                .Count() + 1;
            return "Etapa " + total.ToString();
        }

        public List<Etapa> AddDataEtapas( List<Etapa> etapas ) {
            Projeto projeto = _context.Projetos
                                .Where(p => p.Id == etapas.FirstOrDefault().ProjetoId)
                                .Where(p => p.DataInicio != null)
                                .FirstOrDefault();
            if(projeto != null) {
                DateTime? dataInicio = projeto.DataInicio;
                foreach(Etapa etapa in etapas) {
                    etapa.DataInicio = dataInicio;
                    dataInicio = dataInicio.Value.AddMonths(etapa.Duracao);
                    etapa.DataFim = dataInicio;
                }
            }
            return etapas;
        }
        public List<EtapaProduto> MontEtapaProdutos( Etapa dados ) {
            if(dados.EtapaProdutos != null) {
                var etapaProdutos = new List<EtapaProduto>();
                foreach(var p in dados.EtapaProdutos) {
                    etapaProdutos.Add(new EtapaProduto { ProdutoId = p.ProdutoId });
                }
                return etapaProdutos;
            }
            return null;
        }
        public Resultado Incluir( Etapa dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Etapa";
            if(dados.ProjetoId <= 0) {
                resultado.Inconsistencias.Add("Preencha o ProjetoId");
            }
            Projeto Projeto = _context.Projetos.Where(
                    p => p.Id == dados.ProjetoId).FirstOrDefault();

            if(Projeto == null) {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            // Etapa Etapa = _context.Etapas.Where(
            //     p => p.ProjetoId == dados.ProjetoId).FirstOrDefault();

            // if (Etapa != null)
            // {
            //     resultado.Inconsistencias.Add(
            //         "Etapa já cadastrado para esse projeto. Remova ou Atualize.");
            // }
            if(resultado.Inconsistencias.Count == 0) {
                var etapa = new Etapa {
                    ProjetoId = dados.ProjetoId,
                    Nome = NomeEtapa(dados.ProjetoId),
                    Desc = dados.Desc,
                    Duracao = (dados.EtapaMeses != null && dados.EtapaMeses.Count() > 0) ? dados.EtapaMeses.Count() : 6,
                    EtapaProdutos = this.MontEtapaProdutos(dados),
                    EtapaMeses = dados.EtapaMeses
                };
                _context.Etapas.Add(etapa);
                _context.SaveChanges();
                resultado.Id = etapa.Id.ToString();
            }
            return resultado;
        }
        public Resultado Atualizar( Etapa dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Etapa";

            if(resultado.Inconsistencias.Count == 0) {
                Etapa Etapa = _context.Etapas.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(Etapa == null) {
                    resultado.Inconsistencias.Add(
                        "Etapa não encontrado");
                }
                else {

                    Etapa.Desc = (dados.Desc == null) ? Etapa.Desc : dados.Desc;
                    Etapa.AtividadesRealizadas = (dados.AtividadesRealizadas == null) ? Etapa.AtividadesRealizadas : dados.AtividadesRealizadas;
                    if(dados.EtapaProdutos != null) {
                        _context.EtapaProdutos.RemoveRange(_context.EtapaProdutos.Where(p => p.EtapaId == dados.Id));
                        Etapa.EtapaProdutos = this.MontEtapaProdutos(dados);
                    }
                    if(dados.EtapaMeses != null) {
                        _context.EtapaMeses.RemoveRange(_context.EtapaMeses.Where(p => p.EtapaId == dados.Id));
                        Etapa.EtapaMeses = dados.EtapaMeses;
                        Etapa.Duracao = dados.EtapaMeses.Count();
                    }
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos( Etapa dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do Etapa");
            }
            else {
                if(String.IsNullOrEmpty(dados.Desc)) {
                    resultado.Inconsistencias.Add(
                        "Preencha a descrição das atividades da Etapa");
                }
                if(dados.EtapaProdutos == null && dados.EtapaMeses == null) {
                    resultado.Inconsistencias.Add("Informe os Produtos ou Meses relacionados a Etapa.");
                }
                else {
                    if(dados.EtapaProdutos != null) {
                        foreach(var etapaProduto in dados.EtapaProdutos) {
                            Produto Produto = _context.Produtos.Where(
                                p => p.Id == etapaProduto.ProdutoId).FirstOrDefault();

                            if(Produto == null) {
                                resultado.Inconsistencias.Add("Produto não localizado ou não relacionados");
                            }
                        }
                    }
                }
            }
            return resultado;
        }
        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Etapa";

            Etapa Etapa = _context.Etapas.First(t => t.Id == id);
            int Last = _context.Etapas
                .OrderByDescending(e => e.Id)
                .First(e => e.ProjetoId == Etapa.ProjetoId).Id;
            if(Etapa == null) {
                resultado.Inconsistencias.Add("Etapa não encontrada");
            }
            else if(Last != id) {
                resultado.Inconsistencias.Add("Possível somente excluir a última etapa do projeto");
            }
            else {
                _context.EtapaMeses.RemoveRange(_context.EtapaMeses.Where(t => t.EtapaId == id));
                _context.EtapaProdutos.RemoveRange(_context.EtapaProdutos.Where(t => t.EtapaId == id));
                _context.Etapas.Remove(Etapa);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}