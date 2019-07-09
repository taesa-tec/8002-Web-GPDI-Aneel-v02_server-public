using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using Microsoft.AspNetCore.Authorization;

namespace APIGestor.Business {
    public class LogProjetoService : BaseAuthorizationService {

        public LogProjetoService( GestorDbContext context, IAuthorizationService authorization ) : base(context, authorization) { }

        public IEnumerable<LogProjeto> ListarTodos( int projetoId, Acoes? acao, string user, int pag, int size ) {
            var LogProjeto = _context.LogProjetos
                .Include("User")
                .Where(p => p.ProjetoId == projetoId);
            if(acao != null) {
                LogProjeto = LogProjeto
                    .Where(p => p.Acao == (Acoes)acao);
            }
            if(user != null) {
                LogProjeto = LogProjeto
                    .Where(p => p.UserId == user);
            }
            return LogProjeto;
        }
        public Resultado Incluir( LogProjeto dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de LogProjeto";

            if(resultado.Inconsistencias.Count == 0) {
                _context.LogProjetos.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }
        private Resultado DadosValidos( LogProjeto dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do LogProjeto");
            }
            else {
                if(dados.ProjetoId <= 0) {
                    resultado.Inconsistencias.Add("Preencha o ProjetoId");
                }
                else {
                    Projeto Projeto = _context.Projetos.Where(
                            p => p.Id == dados.ProjetoId).FirstOrDefault();

                    if(Projeto == null) {
                        resultado.Inconsistencias.Add("Projeto não localizado");
                    }
                }

                if(String.IsNullOrEmpty(dados.Tela)) {
                    resultado.Inconsistencias.Add("Preencha a Tela");
                }
                if(dados.Acao.ToString() == null) {
                    resultado.Inconsistencias.Add("Preencha a Ação");
                }
                else {
                    if(dados.Acao.ToString() == "Create" || dados.Acao.ToString() == "Update") {
                        if(String.IsNullOrEmpty(dados.StatusNovo)) {
                            resultado.Inconsistencias.Add("Preencha o StatusNovo");
                        }
                    }
                    if(dados.Acao.ToString() == "Update" || dados.Acao.ToString() == "Delete") {
                        if(String.IsNullOrEmpty(dados.StatusAnterior)) {
                            resultado.Inconsistencias.Add("Preencha o StatusAnterior");
                        }
                    }
                }
            }
            return resultado;
        }
        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de LogProjeto";

            LogProjeto LogProjeto = _context.LogProjetos.First(t => t.Id == id);
            if(LogProjeto == null) {
                resultado.Inconsistencias.Add("LogProjeto não localizada");
            }
            else {
                _context.LogProjetos.Remove(LogProjeto);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}