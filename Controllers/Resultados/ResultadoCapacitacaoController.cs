using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;


namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoCapacitacaoController : ControllerBase {
        private ResultadoCapacitacaoService _service;

        public ResultadoCapacitacaoController( ResultadoCapacitacaoService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoCapacitacao")]
        public IEnumerable<ResultadoCapacitacao> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoCapacitacao/{Id}")]
        public ActionResult<ResultadoCapacitacao> GetA( int id ) {
            var ResultadoCapacitacao = _service.Obter(id);
            if(ResultadoCapacitacao != null)
                return ResultadoCapacitacao;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]ResultadoCapacitacao ResultadoCapacitacao ) {
            if(_service.UserProjectCan(ResultadoCapacitacao.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(ResultadoCapacitacao);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]ResultadoCapacitacao ResultadoCapacitacao ) {
            var Resultado = _service._context.ResultadosCapacitacao.Find(ResultadoCapacitacao.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(ResultadoCapacitacao);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service._context.ResultadosCapacitacao.Find(id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
                return _service.Excluir(id);
            return Forbid();
        }
    }
}