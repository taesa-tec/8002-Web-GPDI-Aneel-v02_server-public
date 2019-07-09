using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Business {
    public class ProdutoService : BaseAuthorizationService {
        public ProdutoService( GestorDbContext context, IAuthorizationService authorization ) : base(context, authorization) { }

        public Produto Obter( int id ) {
            if(id > 0) {
                return _context.Produtos
                    .Include("EtapaProduto")
                    .Include("CatalogProdutoFaseCadeia")
                    .Include("CatalogProdutoTipoDetalhado")
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }

        public List<Produto> ListarTodos( int projetoId ) {
            if(projetoId > 0) {
                var Produtos = _context.Produtos
                    .Include("CatalogProdutoFaseCadeia")
                    .Include("CatalogProdutoTipoDetalhado")
                    .Include("EtapaProduto")
                    .Where(
                    p => p.ProjetoId == projetoId).OrderBy(p => p.Titulo);

                if(Produtos.Count() > 0) {
                    return Produtos.ToList();
                }
                else
                    return null;

            }
            else
                return null;

        }

        public Resultado Incluir( Produto dadosProduto ) {
            Resultado resultado = DadosValidos(dadosProduto);
            resultado.Acao = "Inclusão de Produto";

            if(dadosProduto.Classificacao.ToString() == "Final") {
                Produto Produto = _context.Produtos.Where(
                        p => p.ProjetoId == dadosProduto.ProjetoId).Where(
                        p => p.Classificacao == dadosProduto.Classificacao).FirstOrDefault();

                if(Produto != null) {
                    resultado.Inconsistencias.Add("Já existe um produto com classificação final para o projeto. Remova-o ou atualize.");
                }
            }

            if(resultado.Inconsistencias.Count == 0) {
                _context.Produtos.Add(dadosProduto);
                _context.SaveChanges();
                resultado.Id = dadosProduto.Id.ToString();
            }

            return resultado;
        }

        public Resultado Atualizar( Produto dadosProduto ) {
            Resultado resultado = DadosValidos(dadosProduto);
            resultado.Acao = "Atualização de Produto";

            if(resultado.Inconsistencias.Count == 0) {
                Produto Produto = _context.Produtos.Where(
                    p => p.Id == dadosProduto.Id).FirstOrDefault();

                if(Produto == null) {
                    resultado.Inconsistencias.Add(
                        "Produto não encontrado");
                }
                else {
                    Produto.Titulo = dadosProduto.Titulo;
                    Produto.Desc = dadosProduto.Desc;
                    Produto.Classificacao = dadosProduto.Classificacao;
                    Produto.Tipo = dadosProduto.Tipo;
                    Produto.CatalogProdutoFaseCadeiaId = dadosProduto.CatalogProdutoFaseCadeiaId;
                    Produto.CatalogProdutoTipoDetalhadoId = dadosProduto.CatalogProdutoTipoDetalhadoId;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Produto";

            Produto Produto = Obter(id);
            if(Produto == null) {
                resultado.Inconsistencias.Add(
                    "Produto não encontrado");
            }
            else {
                _context.Produtos.Remove(Produto);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos( Produto Produto ) {
            var resultado = new Resultado();
            if(Produto == null) {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Produto");
            }
            else {
                if(String.IsNullOrWhiteSpace(Produto.Titulo)) {
                    resultado.Inconsistencias.Add(
                        "Preencha o Título");
                }
                if(String.IsNullOrWhiteSpace(Produto.Desc)) {
                    resultado.Inconsistencias.Add(
                        "Preencha a descrição");
                }
                if(Produto.ProjetoId <= 0) {
                    resultado.Inconsistencias.Add(
                        "Preencha o ProjetoId");
                }
            }

            return resultado;
        }
    }
}