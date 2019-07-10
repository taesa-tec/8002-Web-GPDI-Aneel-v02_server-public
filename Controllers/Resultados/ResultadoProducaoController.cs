using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoProducaoController : ControllerBase {
        private ResultadoProducaoService _service;

        public ResultadoProducaoController( ResultadoProducaoService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoProducao")]
        public IEnumerable<ResultadoProducao> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoProducao/{Id}")]
        public ActionResult<ResultadoProducao> GetA( int id ) {
            var ResultadoProducao = _service.Obter(id);
            if(ResultadoProducao != null)
                return ResultadoProducao;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]ResultadoProducao ResultadoProducao ) {
            if(_service.UserProjectCan(ResultadoProducao.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(ResultadoProducao);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]ResultadoProducao ResultadoProducao ) {
            var Resultado = _service._context.ResultadosProducao.Find(ResultadoProducao.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(ResultadoProducao);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service._context.ResultadosProducao.Find(id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
                return _service.Excluir(id);
            return Forbid();

        }
    }
}