using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using APIGestor.Models.Projetos.Resultados;
using Microsoft.AspNetCore.Authorization;

namespace APIGestor.Services.Projetos.Resultados {
    public class ResultadoInfraService : BaseGestorService {

        public ResultadoInfraService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) { }

        public ResultadoInfra Obter( int id ) {
            if(id > 0) {
                return _context.ResultadosInfra
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        public IEnumerable<ResultadoInfra> ListarTodos( int projetoId ) {
            var RecursoHumano = _context.ResultadosInfra
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir( ResultadoInfra dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Resultado Infra-Estrutura";

            if(resultado.Inconsistencias.Count == 0) {
                _context.ResultadosInfra.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar( ResultadoInfra dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Resultado Infra-Estrutura";

            if(resultado.Inconsistencias.Count == 0) {
                ResultadoInfra ResultadoInfra = _context.ResultadosInfra.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(ResultadoInfra == null) {
                    resultado.Inconsistencias.Add(
                        "Resultado Infra-Estrutura não encontrado");
                }
                else {
                    ResultadoInfra.Tipo = !Enum.IsDefined(typeof(TipoInfra), dados.Tipo) ? ResultadoInfra.Tipo : dados.Tipo;
                    ResultadoInfra.CnpjReceptora = (dados.CnpjReceptora == null) ? ResultadoInfra.CnpjReceptora : dados.CnpjReceptora;
                    ResultadoInfra.NomeLaboratorio = (dados.NomeLaboratorio == null) ? ResultadoInfra.NomeLaboratorio : dados.NomeLaboratorio;
                    ResultadoInfra.AreaPesquisa = (dados.AreaPesquisa == null) ? ResultadoInfra.AreaPesquisa : dados.AreaPesquisa;
                    ResultadoInfra.ListaMateriais = (dados.ListaMateriais == null) ? ResultadoInfra.ListaMateriais : dados.ListaMateriais;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Resultado Infra-Estrutura";

            ResultadoInfra ResultadoInfra = _context.ResultadosInfra.FirstOrDefault(p => p.Id == id);
            if(ResultadoInfra == null) {
                resultado.Inconsistencias.Add("Resultado Infra-Estrutura não encontrado");
            }
            else {
                _context.ResultadosInfra.Remove(ResultadoInfra);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos( ResultadoInfra dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do Resultado Infra-Estrutura");
            }

            return resultado;
        }
    }
}