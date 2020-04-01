using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.Linq;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class EtapasController : ControllerBase {
        private EtapaService _service;

        public EtapasController( EtapaService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/Etapas")]
        public IEnumerable<Etapa> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]Etapa Etapa ) {
            if(_service.UserProjectCan(Etapa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(Etapa);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Etapa.ProjetoId, Etapa);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]Etapa Etapa ) {
            var etapa = _service.Obter(Etapa.Id);
            if(_service.UserProjectCan(etapa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                _service._context.Entry(etapa).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(Etapa);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, etapa.ProjetoId, _service.Obter(Etapa.Id), etapa);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Etapa = _service._context.Etapas.Where(e => e.Id == id).FirstOrDefault();
            if(Etapa != null) {
                if(_service.UserProjectCan(Etapa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, Etapa.ProjetoId, Etapa);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}