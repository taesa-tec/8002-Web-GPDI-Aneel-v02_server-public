using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoSocioAmbientalController : ControllerBase {
        private ResultadoSocioAmbientalService _service;

        public ResultadoSocioAmbientalController( ResultadoSocioAmbientalService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoSocioAmbiental")]
        public IEnumerable<ResultadoSocioAmbiental> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoSocioAmbiental/{Id}")]
        public ActionResult<ResultadoSocioAmbiental> GetA( int id ) {
            var ResultadoSocioAmbiental = _service.Obter(id);
            if(ResultadoSocioAmbiental != null)
                return ResultadoSocioAmbiental;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]ResultadoSocioAmbiental ResultadoSocioAmbiental ) {
            if(_service.UserProjectCan(ResultadoSocioAmbiental.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(ResultadoSocioAmbiental);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]ResultadoSocioAmbiental ResultadoSocioAmbiental ) {
            var Resultado = _service._context.ResultadosSocioAmbiental.Find(ResultadoSocioAmbiental.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(ResultadoSocioAmbiental);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service._context.ResultadosSocioAmbiental.Find(id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
                return _service.Excluir(id);
            return Forbid();
        }
    }
}