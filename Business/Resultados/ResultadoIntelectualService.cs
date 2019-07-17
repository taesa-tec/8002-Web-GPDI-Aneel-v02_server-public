using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace APIGestor.Business
{
    public class ResultadoIntelectualService : BaseGestorService {
        
        public ResultadoIntelectualService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) { }

        public ResultadoIntelectual Obter(int id)
        {
            if (id>0)
            {
                return _context.ResultadosIntelectual
                    .Include("Inventores.RecursoHumano")
                    .Include("Depositantes.Empresa")
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        public IEnumerable<ResultadoIntelectual> ListarTodos(int projetoId)
        {
            var RecursoHumano = _context.ResultadosIntelectual
                    .Include("Inventores.RecursoHumano")
                    .Include("Depositantes.Empresa")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir(ResultadoIntelectual dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Resultado Propriedade Intelectual";
                         
            if (resultado.Inconsistencias.Count == 0)
            {
                _context.ResultadosIntelectual.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar(ResultadoIntelectual dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Resultado Propriedade Intelectual";

            if (resultado.Inconsistencias.Count == 0)
            {
                ResultadoIntelectual ResultadoIntelectual = _context.ResultadosIntelectual.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (ResultadoIntelectual == null)
                {
                    resultado.Inconsistencias.Add(
                        "Resultado Propriedade Intelectual não encontrado");
                }
                else
                {
                    ResultadoIntelectual.Tipo = !Enum.IsDefined(typeof(TipoIntelectual),dados.Tipo) ? ResultadoIntelectual.Tipo : dados.Tipo;
                    ResultadoIntelectual.Titulo = (dados.Titulo==null) ? ResultadoIntelectual.Titulo : dados.Titulo;
                    ResultadoIntelectual.NumeroPedido = (dados.NumeroPedido==null) ? ResultadoIntelectual.NumeroPedido : dados.NumeroPedido;
                    ResultadoIntelectual.DataPedido = (dados.DataPedido==null) ? ResultadoIntelectual.DataPedido : dados.DataPedido;
                     
                    if (dados.Inventores!=null){
                        _context.ResultadoIntelectualInventores.RemoveRange(_context.ResultadoIntelectualInventores.Where(p => p.ResultadoIntelectualId == dados.Id));
                        ResultadoIntelectual.Inventores = dados.Inventores;
                    }
                    if (dados.Depositantes!=null){
                        _context.ResultadoIntelectualDepositantes.RemoveRange(_context.ResultadoIntelectualDepositantes.Where(p => p.ResultadoIntelectualId == dados.Id));  
                        ResultadoIntelectual.Depositantes = dados.Depositantes;
                    }
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Resultado Propriedade Intelectual";

            ResultadoIntelectual ResultadoIntelectual = _context.ResultadosIntelectual.FirstOrDefault(p=>p.Id==id);
            if (ResultadoIntelectual == null)
            {
                resultado.Inconsistencias.Add("Resultado Propriedade Intelectual não encontrado");
            }
            else
            {
                _context.ResultadoIntelectualInventores.RemoveRange(_context.ResultadoIntelectualInventores.Where(p => p.ResultadoIntelectualId == id));
                _context.ResultadoIntelectualDepositantes.RemoveRange(_context.ResultadoIntelectualDepositantes.Where(p => p.ResultadoIntelectualId == id));   
                _context.ResultadosIntelectual.Remove(ResultadoIntelectual);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(ResultadoIntelectual dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Resultado Propriedade Intelectual");
            }

            return resultado;
        }
    }
}