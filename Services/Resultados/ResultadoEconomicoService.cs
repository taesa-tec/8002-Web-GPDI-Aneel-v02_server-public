using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using APIGestor.Models.Projetos.Resultados;
using Microsoft.AspNetCore.Authorization;

namespace APIGestor.Services.Resultados
{
    public class ResultadoEconomicoService : BaseGestorService {
        
        public ResultadoEconomicoService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) { }

        public ResultadoEconomico Obter(int id)
        {
            if (id>0)
            {
                return _context.ResultadosEconomico
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        public IEnumerable<ResultadoEconomico> ListarTodos(int projetoId)
        {
            var RecursoHumano = _context.ResultadosEconomico
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir(ResultadoEconomico dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Resultado Economico";
                         
            if (resultado.Inconsistencias.Count == 0)
            {
                _context.ResultadosEconomico.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar(ResultadoEconomico dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Resultado Economico";

            if (resultado.Inconsistencias.Count == 0)
            {
                ResultadoEconomico ResultadoEconomico = _context.ResultadosEconomico.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (ResultadoEconomico == null)
                {
                    resultado.Inconsistencias.Add(
                        "Resultado Economico não encontrado");
                }
                else
                {
                    ResultadoEconomico.Tipo = !Enum.IsDefined(typeof(TipoEconomico),dados.Tipo) ? ResultadoEconomico.Tipo : dados.Tipo;
                    ResultadoEconomico.Desc = (dados.Desc==null) ? ResultadoEconomico.Desc : dados.Desc;
                    ResultadoEconomico.UnidadeBase = (dados.UnidadeBase==null) ? ResultadoEconomico.UnidadeBase : dados.UnidadeBase;
                    ResultadoEconomico.ValorIndicador = (dados.ValorIndicador==null) ? ResultadoEconomico.ValorIndicador : dados.ValorIndicador;
                    ResultadoEconomico.Percentagem = (dados.Percentagem==null) ? ResultadoEconomico.Percentagem : dados.Percentagem;
                    ResultadoEconomico.ValorBeneficio = (dados.ValorBeneficio==null) ? ResultadoEconomico.ValorBeneficio : dados.ValorBeneficio;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Resultado Economico";

            ResultadoEconomico ResultadoEconomico = _context.ResultadosEconomico.FirstOrDefault(p=>p.Id==id);
            if (ResultadoEconomico == null)
            {
                resultado.Inconsistencias.Add("Resultado Economico não encontrado");
            }
            else
            {
                _context.ResultadosEconomico.Remove(ResultadoEconomico);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(ResultadoEconomico dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Resultado Economico");
            }

            return resultado;
        }
    }
}