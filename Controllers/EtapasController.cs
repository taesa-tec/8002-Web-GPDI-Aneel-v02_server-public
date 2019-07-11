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

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]Etapa Etapa ) {
            if(_service.UserProjectCan(Etapa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Incluir(Etapa);
            }
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]Etapa Etapa ) {
            var etapa = _service._context.Etapas.Find(Etapa.Id);
            if(_service.UserProjectCan(etapa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Atualizar(Etapa);
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Etapa = _service._context.Etapas.Where(e => e.Id == id).FirstOrDefault();
            if(Etapa != null) {
                if(_service.UserProjectCan(Etapa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                    return _service.Excluir(id);
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}