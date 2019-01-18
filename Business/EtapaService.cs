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
    public class EtapaService
    {
        private GestorDbContext _context;

        public EtapaService(GestorDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Etapa> ListarTodos(int projetoId)
        {
            var Etapa = _context.Etapas
                .Include("EtapaProdutos")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return Etapa;
        }
        private List<EtapaProduto> MontEtapaProdutos(Etapa dados)
        {
            var etapaProdutos = new List<EtapaProduto>();
            foreach (var p in dados.EtapaProdutos)
            {
                etapaProdutos.Add(new EtapaProduto { ProdutoId = p.ProdutoId });
            }
            return etapaProdutos;
        } 
        public Resultado Incluir(Etapa dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Etapa";
            if (dados.ProjetoId <= 0)
            {
                resultado.Inconsistencias.Add("Preencha o ProjetoId");
            }
            Projeto Projeto = _context.Projetos.Where(
                    p => p.Id == dados.ProjetoId).FirstOrDefault();

            if (Projeto == null)
            {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            // Etapa Etapa = _context.Etapas.Where(
            //     p => p.ProjetoId == dados.ProjetoId).FirstOrDefault();

            // if (Etapa != null)
            // {
            //     resultado.Inconsistencias.Add(
            //         "Etapa já cadastrado para esse projeto. Remova ou Atualize.");
            // }
            if (resultado.Inconsistencias.Count == 0)
            {
                var etapa = new Etapa
                {
                    ProjetoId = dados.ProjetoId,
                    Desc = dados.Desc,
                    DataInicio = dados.DataInicio,
                    DataFim = dados.DataFim,
                    EtapaProdutos = this.MontEtapaProdutos(dados)
                };
                _context.Etapas.Add(etapa);
                _context.SaveChanges();
            }
            return resultado;
        }
        public Resultado Atualizar(Etapa dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Etapa";

            if (resultado.Inconsistencias.Count == 0)
            {
                Etapa Etapa = _context.Etapas.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (Etapa == null)
                {
                    resultado.Inconsistencias.Add(
                        "Etapa não encontrado");
                }
                else
                {
                    _context.EtapaProdutos.RemoveRange(_context.EtapaProdutos.Where(p => p.EtapaId == dados.Id));
                    Etapa.DataInicio = dados.DataInicio;
                    Etapa.DataFim = dados.DataFim;
                    Etapa.Desc = dados.Desc;
                    Etapa.EtapaProdutos = this.MontEtapaProdutos(dados);
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos(Etapa dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Etapa");
            }
            else
            {   
                DateTime DataInicio;
                if (!DateTime.TryParse(dados.DataInicio.ToString(), out DataInicio))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a data de Início do Projeto");
                }
                DateTime DataFim;
                if (!DateTime.TryParse(dados.DataInicio.ToString(), out DataFim))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a data de Fim do Projeto");
                }
 
                if (String.IsNullOrEmpty(dados.Desc))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a descrição das atividades da Etapa");
                }
                if (dados.EtapaProdutos.Count <= 0)
                {
                    resultado.Inconsistencias.Add("Informe os Produtos relacionados a Etapa.");
                }
                foreach (var etapaProduto in dados.EtapaProdutos)
                {
                    Produto Produto = _context.Produtos.Where(
                        p => p.Id == etapaProduto.ProdutoId).FirstOrDefault();

                    if (Produto == null)
                    {
                        resultado.Inconsistencias.Add("Produto não localizado ou não relacionados");
                    }
                }
            }
            return resultado;
        }
        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Etapa";

            Etapa Etapa = _context.Etapas.First(t => t.Id == id);
            if (Etapa == null)
            {
                resultado.Inconsistencias.Add("Etapa não encontrada");
            }
            else
            {   _context.EtapaProdutos.RemoveRange(_context.EtapaProdutos.Where(t=>t.EtapaId == id));
                _context.Etapas.Remove(Etapa);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}