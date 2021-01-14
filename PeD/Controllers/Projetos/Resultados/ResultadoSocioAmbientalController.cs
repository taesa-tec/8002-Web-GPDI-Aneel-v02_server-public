using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Authorizations;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Services.Projetos.Resultados;

namespace PeD.Controllers.Projetos.Resultados {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]ResultadoSocioAmbiental ResultadoSocioAmbiental ) {
            if(_service.UserProjectCan(ResultadoSocioAmbiental.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(ResultadoSocioAmbiental);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, ResultadoSocioAmbiental.ProjetoId, ResultadoSocioAmbiental);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]ResultadoSocioAmbiental ResultadoSocioAmbiental ) {
            var Resultado = _service._context.ResultadosSocioAmbiental.Find(ResultadoSocioAmbiental.Id);
            if(_service.UserProjectCan(Resultado.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                _service._context.Entry(Resultado).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(ResultadoSocioAmbiental);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Resultado.ProjetoId, ResultadoSocioAmbiental, Resultado);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Resultado = _service._context.ResultadosSocioAmbiental.Find(id);
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