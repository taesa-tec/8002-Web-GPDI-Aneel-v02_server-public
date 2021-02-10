using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeD.Projetos.Controllers.Projetos.Resultados {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]ResultadoIntelectual ResultadoIntelectual ) {
            if(_service.UserProjectCan(ResultadoIntelectual.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(ResultadoIntelectual);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, ResultadoIntelectual.ProjetoId, _service.Obter(ResultadoIntelectual.Id));
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]ResultadoIntelectual ResultadoIntelectual ) {
            var Resultado = _service.Obter(ResultadoIntelectual.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                _service._context.Entry(Resultado).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(ResultadoIntelectual);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Resultado.ProjetoId, _service.Obter(ResultadoIntelectual.Id), Resultado);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service.Obter(id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, ProjectPermissions.Administrator)) {
                var resultado = _service.Excluir(id);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Resultado.ProjetoId, Resultado);
                }
                return resultado;
            }
            return Forbid();
        }
    }
}