using System.Collections.Generic;
using APIGestor.Business;
using APIGestor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGestor.Controllers.Resultados
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoCapacitacaoController : ControllerBase
    {
        private ResultadoCapacitacaoService _service;

        public ResultadoCapacitacaoController(ResultadoCapacitacaoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoCapacitacao")]
        public IEnumerable<ResultadoCapacitacao> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoCapacitacao/{Id}")]
        public ActionResult<ResultadoCapacitacao> GetA(int id)
        {
            var ResultadoCapacitacao = _service.Obter(id);
            if (ResultadoCapacitacao != null)
                return ResultadoCapacitacao;
            else
                return NotFound();
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post([FromBody] ResultadoCapacitacao ResultadoCapacitacao)
        {
            if (_service.UserProjectCan(ResultadoCapacitacao.ProjetoId, User,
                Authorizations.ProjectPermissions.LeituraEscrita))
            {
                var resultado = _service.Incluir(ResultadoCapacitacao);
                if (resultado.Sucesso)
                {
                    this.CreateLog(_service, ResultadoCapacitacao.ProjetoId, ResultadoCapacitacao);
                }

                return resultado;
            }

            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put([FromBody] ResultadoCapacitacao ResultadoCapacitacao)
        {
            var Resultado = _service._context.ResultadosCapacitacao.Find(ResultadoCapacitacao.Id);
            if (_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
            {
                _service._context.Entry(Resultado).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(ResultadoCapacitacao);
                if (resultado.Sucesso)
                {
                    this.CreateLog(_service, Resultado.ProjetoId, _service.Obter(ResultadoCapacitacao.Id), Resultado);
                }

                return resultado;
            }

            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            var Resultado = _service.Obter(id);
            if (_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
            {
                var resultado = _service.Excluir(id);
                if (resultado.Sucesso)
                {
                    this.CreateLog(_service, Resultado.ProjetoId, Resultado);
                }

                return resultado;
            }

            return Forbid();
        }
    }
}