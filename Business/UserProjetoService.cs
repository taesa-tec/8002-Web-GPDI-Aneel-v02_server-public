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

        public IEnumerable<Projeto> ListarTodos(string userId)
        {
            var UserProjetos = _context.Projetos
                .Where(x => _context.UserProjetos
                    .Where(y => y.UserId == userId)
                    .Any(p => p.ProjetoId == x.Id))
                .OrderBy(p => p.Titulo)
                .ToList();
            return UserProjetos;
        }

        public Resultado Incluir(UserProjeto dadosUserProjeto)
        {
            Resultado resultado = DadosValidos(dadosUserProjeto);
            resultado.Acao = "Inclusão de UserProjeto";

            UserProjeto UserProjeto = _context.UserProjetos.Where(
                    p => p.UserId == dadosUserProjeto.UserId).Where(
                    p => p.ProjetoId == dadosUserProjeto.ProjetoId).FirstOrDefault();
            if (UserProjeto!=null)
            {
                resultado.Inconsistencias.Add("UsuarioProjeto já cadastrado.");
            }
            if (resultado.Inconsistencias.Count == 0 &&
                _context.Projetos.Where(
                p => p.Id == dadosUserProjeto.ProjetoId).Count() <= 0)
            {
                resultado.Inconsistencias.Add(
                    "Projeto não existente");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                _context.UserProjetos.Add(dadosUserProjeto);
                _context.SaveChanges();
                resultado.Id = dadosUserProjeto.Id;
            }

            return resultado;
        }

        public Resultado Atualizar(UserProjeto dadosUserProjeto)
        {
            Resultado resultado = DadosValidos(dadosUserProjeto);
            resultado.Acao = "Atualização de UserProjeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                UserProjeto UserProjeto = _context.UserProjetos.Where(
                    p => p.UserId == dadosUserProjeto.UserId).Where(
                    p => p.ProjetoId == dadosUserProjeto.ProjetoId).FirstOrDefault();

                if (UserProjeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Usuário ou Projeto não encontrados.");
                }
                else
                {
                    UserProjeto.CatalogUserPermissaoId = dadosUserProjeto.CatalogUserPermissaoId;
                    _context.SaveChanges();
                }
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

        private Resultado DadosValidos(UserProjeto dadosUserProjeto)
        {
            var resultado = new Resultado();
            if (dadosUserProjeto == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do UserProjeto");
            }
            else
            {
                if (dadosUserProjeto.ProjetoId<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o ProjetoId");
                }
                if (dadosUserProjeto.UserId==null)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o UserId");
                }
                if (dadosUserProjeto.CatalogUserPermissaoId<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o CatalogUserPermissaoId");
                }
                 CatalogUserPermissao Permissao = _context.CatalogUserPermissoes.Where(
                    e => e.Id == dadosUserProjeto.CatalogUserPermissaoId).FirstOrDefault();

                if (Permissao == null)
                {
                    resultado.Inconsistencias.Add(
                        "Permissao não encontrada");
                }
                ApplicationUser User = _context.Users.Where(
                    e => e.Id == dadosUserProjeto.UserId).FirstOrDefault();

                if (User == null)
                {
                    resultado.Inconsistencias.Add(
                        "Usuário não encontrada");
                }
                Projeto Projeto = _context.Projetos.Where(
                    e => e.Id == dadosUserProjeto.ProjetoId).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrada");
                }
            }

            return resultado;
        }
    }
}