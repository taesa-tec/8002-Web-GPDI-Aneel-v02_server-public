using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Business
{
    public class ResultadoSocioAmbientalService
    {
        private GestorDbContext _context;

        public ResultadoSocioAmbientalService(GestorDbContext context)
        {
            _context = context;
        }

        public ResultadoSocioAmbiental Obter(int id)
        {
            if (id>0)
            {
                return _context.ResultadosSocioAmbiental
                    .Where(p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        public IEnumerable<ResultadoSocioAmbiental> ListarTodos(int projetoId)
        {
            var RecursoHumano = _context.ResultadosSocioAmbiental
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir(ResultadoSocioAmbiental dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Resultado SocioAmbiental";
                         
            if (resultado.Inconsistencias.Count == 0)
            {
                _context.ResultadosSocioAmbiental.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar(ResultadoSocioAmbiental dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Resultado SocioAmbiental";

            if (resultado.Inconsistencias.Count == 0)
            {
                ResultadoSocioAmbiental ResultadoSocioAmbiental = _context.ResultadosSocioAmbiental.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (ResultadoSocioAmbiental == null)
                {
                    resultado.Inconsistencias.Add(
                        "Resultado SocioAmbiental não encontrado");
                }
                else
                {
                    ResultadoSocioAmbiental.Tipo = !Enum.IsDefined(typeof(TipoIndicador),dados.Tipo) ? ResultadoSocioAmbiental.Tipo : dados.Tipo;
                    ResultadoSocioAmbiental.Desc = (dados.Desc==null) ? ResultadoSocioAmbiental.Desc : dados.Desc;
                    ResultadoSocioAmbiental.Positivo = (dados.Positivo==null) ? ResultadoSocioAmbiental.Positivo : dados.Positivo;
                    ResultadoSocioAmbiental.TecnicaPrevista = (dados.TecnicaPrevista==null) ? ResultadoSocioAmbiental.TecnicaPrevista : dados.TecnicaPrevista;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Resultado SocioAmbiental";

            ResultadoSocioAmbiental ResultadoSocioAmbiental = _context.ResultadosSocioAmbiental.FirstOrDefault(p=>p.Id==id);
            if (ResultadoSocioAmbiental == null)
            {
                resultado.Inconsistencias.Add("Resultado SocioAmbiental não encontrado");
            }
            else
            {
                _context.ResultadosSocioAmbiental.Remove(ResultadoSocioAmbiental);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(ResultadoSocioAmbiental dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Resultado SocioAmbiental");
            }

            return resultado;
        }
    }
}