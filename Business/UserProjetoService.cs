using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;

namespace APIGestor.Business
{
    public class UserProjetoService
    {
        private GestorDbContext _context;
    
        public UserProjetoService(GestorDbContext context)
        {
            _context = context;
        }

        public UserProjeto Obter(int projetoId)
        {
            if (projetoId>0)
            {
                return _context.UserProjetos.Where(
                    p => p.ProjetoId == projetoId).FirstOrDefault();
            }
            else
                return null;
        }

        public IEnumerable<UserProjeto> ListarTodos(string userId)
        {
            var UserProjetos = _context.UserProjetos
                .Where(y => y.UserId == userId)
                .Include("CatalogUserPermissao")
                .Include("Projeto.CatalogStatus")
                .Include("Projeto.CatalogEmpresa")
                .OrderBy(y => y.ProjetoId)
                .ToList();
            return UserProjetos;
        }
        public Resultado IncluirProjeto(List<UserProjeto> dadosUserProjeto)
        {   
            Resultado resultado = new Resultado();
            foreach(UserProjeto dados in dadosUserProjeto)
            {
                resultado = DadosValidos(dados);

                if (resultado.Inconsistencias.Count == 0)
                {
                    // Verifica se já existe associação para o projeto e remove 
                    UserProjeto UserProjeto = _context.UserProjetos.Where(
                            p => p.ProjetoId == dados.ProjetoId).FirstOrDefault();
                    if (UserProjeto!=null)
                    {
                        _context.UserProjetos.RemoveRange(_context.UserProjetos.Where(t=>t.ProjetoId == dados.ProjetoId));
                    }
                    _context.UserProjetos.Add(dados);
                }
            }
            resultado.Acao = "Inclusão de UserProjeto";
            if (resultado.Inconsistencias.Count == 0)
            {
                _context.SaveChanges();
            }
            return resultado;
        }
        public Resultado Incluir(List<UserProjeto> dadosUserProjeto)
        {   
            Resultado resultado = new Resultado();
            foreach(UserProjeto dados in dadosUserProjeto)
            {
                resultado = DadosValidos(dados);

                if (resultado.Inconsistencias.Count == 0)
                {
                    // Verifica se já existe associação para o projeto e remove 
                    UserProjeto UserProjeto = _context.UserProjetos.Where(
                            p => p.UserId == dados.UserId).FirstOrDefault();
                    if (UserProjeto!=null)
                    {
                        _context.UserProjetos.RemoveRange(_context.UserProjetos.Where(t=>t.UserId == dados.UserId));
                    }
                    _context.UserProjetos.Add(dados);
                }
            }
            resultado.Acao = "Inclusão de UserProjeto";
            if (resultado.Inconsistencias.Count == 0)
            {
                _context.SaveChanges();
            }
            return resultado;
        }

        public Resultado Excluir(int projetoId)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de UserProjeto";

            UserProjeto UserProjeto = Obter(projetoId);
            if (UserProjeto == null)
            {
                resultado.Inconsistencias.Add(
                    "UserProjeto não encontrado");
            }
            else
            {
                _context.UserProjetos.Remove(UserProjeto);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(UserProjeto dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do UserProjeto");
            }
            else
            {
                if (dados.ProjetoId<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o ProjetoId");
                }
                if (dados.UserId==null)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o UserId");
                }
                if (dados.CatalogUserPermissaoId<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o CatalogUserPermissaoId");
                }
                 CatalogUserPermissao Permissao = _context.CatalogUserPermissoes.Where(
                    e => e.Id == dados.CatalogUserPermissaoId).FirstOrDefault();

                if (Permissao == null)
                {
                    resultado.Inconsistencias.Add(
                        "Permissao não encontrada");
                }
                ApplicationUser User = _context.Users.Where(
                    e => e.Id == dados.UserId).FirstOrDefault();

                if (User == null)
                {
                    resultado.Inconsistencias.Add(
                        "Usuário não encontrado");
                }
                Projeto Projeto = _context.Projetos.Where(
                    e => e.Id == dados.ProjetoId).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
            }

            return resultado;
        }
    }
}