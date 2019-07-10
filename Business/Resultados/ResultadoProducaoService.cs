using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APIGestor.Business
{
    public class ResultadoProducaoService : BaseAuthorizationService {
        

        public ResultadoProducaoService( GestorDbContext context, IAuthorizationService authorizationService ) : base(context, authorizationService) { }

        public ResultadoProducao Obter(int id)
        {
            if (id>0)
            {
                return _context.ResultadosProducao
                    .Include("Uploads.User")
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        public IEnumerable<ResultadoProducao> ListarTodos(int projetoId)
        {
            var RecursoHumano = _context.ResultadosProducao
                .Include("Uploads")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir(ResultadoProducao dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Resultado Produção";
                         
            if (resultado.Inconsistencias.Count == 0)
            {
                _context.ResultadosProducao.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar(ResultadoProducao dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Resultado Produção";

            if (resultado.Inconsistencias.Count == 0)
            {
                ResultadoProducao ResultadoProducao = _context.ResultadosProducao.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (ResultadoProducao == null)
                {
                    resultado.Inconsistencias.Add(
                        "Resultado Produção não encontrado");
                }
                else
                {
                    ResultadoProducao.Tipo = !Enum.IsDefined(typeof(TipoProducao),dados.Tipo) ? ResultadoProducao.Tipo : dados.Tipo;
                    ResultadoProducao.DataPublicacao = (dados.DataPublicacao==null) ? ResultadoProducao.DataPublicacao : dados.DataPublicacao;
                    ResultadoProducao.Confirmacao = (!dados.Confirmacao.HasValue) ? ResultadoProducao.Confirmacao : dados.Confirmacao;
                    ResultadoProducao.Nome = (dados.Nome==null) ? ResultadoProducao.Nome : dados.Nome;
                    ResultadoProducao.Url = (dados.Url==null) ? ResultadoProducao.Url : dados.Url;
                    ResultadoProducao.CatalogPaisId = (dados.CatalogPaisId<=0) ? ResultadoProducao.CatalogPaisId : dados.CatalogPaisId;
                    ResultadoProducao.Cidade = (dados.Cidade==null) ? ResultadoProducao.Cidade : dados.Cidade;
                    ResultadoProducao.Titulo = (dados.Titulo==null) ? ResultadoProducao.Titulo : dados.Titulo;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Resultado Produção";

            ResultadoProducao ResultadoProducao = _context.ResultadosProducao.FirstOrDefault(p=>p.Id==id);
            if (ResultadoProducao == null)
            {
                resultado.Inconsistencias.Add("Resultado Produção não encontrado");
            }
            else
            {
                _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.ResultadoProducaoId == id));
                _context.ResultadosProducao.Remove(ResultadoProducao);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(ResultadoProducao dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Resultado Produção");
            }

            return resultado;
        }
    }
}