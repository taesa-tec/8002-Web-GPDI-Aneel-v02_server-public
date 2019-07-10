using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoIntelectualController : ControllerBase {
        private ResultadoIntelectualService _service;

        public ResultadoIntelectualController( ResultadoIntelectualService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoIntelectual")]
        public IEnumerable<ResultadoIntelectual> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoIntelectual/{Id}")]
        public ActionResult<ResultadoIntelectual> GetA( int id ) {
            var ResultadoIntelectual = _service.Obter(id);
            if(ResultadoIntelectual != null)
                return ResultadoIntelectual;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]ResultadoIntelectual ResultadoIntelectual ) {
            if(_service.UserProjectCan(ResultadoIntelectual.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(ResultadoIntelectual);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]ResultadoIntelectual ResultadoIntelectual ) {
            var Resultado = _service._context.ResultadosIntelectual.Find(ResultadoIntelectual.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(ResultadoIntelectual);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service._context.ResultadosIntelectual.Find(id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
                return _service.Excluir(id);
            return Forbid();
        }
    }
}