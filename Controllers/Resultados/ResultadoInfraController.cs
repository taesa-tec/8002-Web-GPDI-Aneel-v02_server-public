using System.Collections.Generic;
using APIGestor.Business;
using APIGestor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGestor.Controllers.Resultados {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoInfraController : ControllerBase {
        private ResultadoInfraService _service;

        public ResultadoInfraController( ResultadoInfraService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoInfra")]
        public IEnumerable<ResultadoInfra> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoInfra/{Id}")]
        public ActionResult<ResultadoInfra> GetA( int id ) {
            var ResultadoInfra = _service.Obter(id);
            if(ResultadoInfra != null)
                return ResultadoInfra;
            else
                return NotFound();
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]ResultadoInfra ResultadoInfra ) {
            if(_service.UserProjectCan(ResultadoInfra.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(ResultadoInfra);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, ResultadoInfra.ProjetoId, ResultadoInfra);
                }
                return resultado;
            }
            return Forbid();

        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]ResultadoInfra ResultadoInfra ) {
            var Resultado = _service.Obter(ResultadoInfra.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                _service._context.Entry(Resultado).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                var resultado = _service.Atualizar(ResultadoInfra);

                if(resultado.Sucesso) {
                    this.CreateLog(_service, Resultado.ProjetoId, Resultado, ResultadoInfra);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service._context.ResultadosInfra.Find(id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, Authorizations.ProjectPermissions.Administrator)) {
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