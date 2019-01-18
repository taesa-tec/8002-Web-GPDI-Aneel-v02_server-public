using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;

namespace APIGestor.Business
{
    public class ProjetoService
    {
        private GestorDbContext _context;

        public ProjetoService(GestorDbContext context)
        {
            _context = context;
        }

        public Projeto Obter(int id)
        {
            if (id>0)
            {
                return _context.Projetos
                    .Include(p => p.UsersProjeto)
                    .Include(p => p.CatalogEmpresa)
                    .Include(p => p.CatalogStatus)
                    .Include(p => p.CatalogSegmento)
                    .Include(p => p.Tema)
                    .Include(p => p.Produtos)
                    .Include(p => p.Etapas)
                    .Include(p => p.Empresas)
                    .Include(p => p.RecursosHumanos)
                    .Include(p => p.AlocacoesRh)
                    .Include(p => p.RecursosMateriais)
                    .Include(p => p.AlocacoesRm)
                    .Where(
                        p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }

        public IEnumerable<Projeto> ListarTodos()
        {
            var Projetos = _context.Projetos
                .OrderBy(p => p.Titulo)
                .ToList();
            return Projetos;
        }

        public IEnumerable<UserProjeto> ObterUsuarios(int Id)
        {
            var UserProjetos = _context.UserProjetos
                .Include("ApplicationUser")
                .Include("CatalogUserPermissao")
                .Where(p => p.ProjetoId == Id)
                .ToList();
            return UserProjetos;
        }

        public Resultado Incluir(Projeto dadosProjeto)
        {
            Resultado resultado = DadosValidos(dadosProjeto);
            resultado.Acao = "Inclusão de Projeto";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Projetos.Where(
                p => p.Numero == dadosProjeto.Numero).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "Projeto com Número já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                _context.Projetos.Add(dadosProjeto);
                _context.SaveChanges();
            }

            return resultado;
        }

        public Resultado Atualizar(Projeto dadosProjeto)
        {
            Resultado resultado = DadosValidos(dadosProjeto);
            resultado.Acao = "Atualização de Projeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto Projeto = _context.Projetos.Where(
                    p => p.Id == dadosProjeto.Id).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
                CatalogStatus Status = _context.CatalogStatus.Where(
                    p => p.Id == dadosProjeto.CatalogStatusId).FirstOrDefault();

                if (Status == null)
                {
                    resultado.Inconsistencias.Add(
                        "Status não encontrado");
                }
                else
                {
                    Projeto.Titulo = dadosProjeto.Titulo;
                    Projeto.TituloDesc = dadosProjeto.TituloDesc;
                    Projeto.Numero = dadosProjeto.Numero;
                    Projeto.CatalogEmpresa.Id = dadosProjeto.CatalogEmpresa.Id;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Projeto";

            Projeto Projeto = Obter(id);
            if (Projeto == null)
            {
                resultado.Inconsistencias.Add(
                    "Projeto não encontrado");
            }
            else
            {
                _context.Projetos.Remove(Projeto);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(Projeto Projeto)
        {
            var resultado = new Resultado();
            if (Projeto == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Projeto");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Projeto.Titulo))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Título do Projeto");
                }
                if (String.IsNullOrWhiteSpace(Projeto.TituloDesc))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o título descrição do Projeto");
                }
                if (String.IsNullOrWhiteSpace(Projeto.Numero))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Número descrição do Projeto");
                }
            }

            return resultado;
        }
    }
}