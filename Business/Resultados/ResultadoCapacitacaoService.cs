using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using APIGestor.Data;
using APIGestor.Models;
using APIGestor.Business;
using Microsoft.AspNetCore.Authorization;

namespace APIGestor.Business {
    public class ResultadoCapacitacaoService : BaseAuthorizationService {

        public ResultadoCapacitacaoService( GestorDbContext context, IAuthorizationService authorizationService ) : base(context, authorizationService) { }

        public ResultadoCapacitacao Obter( int id ) {
            if(id > 0) {

                return _context.ResultadosCapacitacao
                    .Include("Uploads.User")
                    .Include("RecursoHumano")
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        public IEnumerable<ResultadoCapacitacao> ListarTodos( int projetoId ) {
            var RecursoHumano = _context.ResultadosCapacitacao
                .Include("RecursoHumano")
                .Include("Uploads")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir( ResultadoCapacitacao dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Resultado Capacitação";

            if(resultado.Inconsistencias.Count == 0) {
                _context.ResultadosCapacitacao.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar( ResultadoCapacitacao dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Resultado Capacitação";

            if(resultado.Inconsistencias.Count == 0) {
                ResultadoCapacitacao ResultadoCapacitacao = _context.ResultadosCapacitacao.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(ResultadoCapacitacao == null) {
                    resultado.Inconsistencias.Add(
                        "Resultado Capacitação não encontrado");
                }
                else {
                    ResultadoCapacitacao.Tipo = !Enum.IsDefined(typeof(TipoCapacitacao), dados.Tipo) ? ResultadoCapacitacao.Tipo : dados.Tipo;
                    ResultadoCapacitacao.DataConclusao = (dados.DataConclusao == null) ? ResultadoCapacitacao.DataConclusao : dados.DataConclusao;
                    ResultadoCapacitacao.Conclusao = (!dados.Conclusao.HasValue) ? ResultadoCapacitacao.Conclusao : dados.Conclusao;
                    ResultadoCapacitacao.CnpjInstituicao = (dados.CnpjInstituicao == null) ? ResultadoCapacitacao.CnpjInstituicao : dados.CnpjInstituicao;
                    ResultadoCapacitacao.AreaPesquisa = (dados.AreaPesquisa == null) ? ResultadoCapacitacao.AreaPesquisa : dados.AreaPesquisa;
                    ResultadoCapacitacao.RecursoHumanoId = (dados.RecursoHumanoId <= 0) ? ResultadoCapacitacao.RecursoHumanoId : dados.RecursoHumanoId;
                    ResultadoCapacitacao.TituloTrabalho = (dados.TituloTrabalho == null) ? ResultadoCapacitacao.TituloTrabalho : dados.TituloTrabalho;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Resultado Capacitação";

            ResultadoCapacitacao ResultadoCapacitacao = _context.ResultadosCapacitacao.FirstOrDefault(p => p.Id == id);
            if(ResultadoCapacitacao == null) {
                resultado.Inconsistencias.Add("Resultado Capacitação não encontrado");
            }
            else {
                _context.Uploads.RemoveRange(_context.Uploads.Where(t => t.ResultadoCapacitacaoId == id));
                _context.ResultadosCapacitacao.Remove(ResultadoCapacitacao);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos( ResultadoCapacitacao dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do Resultado Capacitação");
            }

            return resultado;
        }
    }
}