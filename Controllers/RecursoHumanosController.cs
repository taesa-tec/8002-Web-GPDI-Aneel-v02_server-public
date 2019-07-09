using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RecursoHumanosController : ControllerBase {
        private RecursoHumanoService _service;

        public RecursoHumanosController( RecursoHumanoService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/RecursoHumanos")]
        public IEnumerable<RecursoHumano> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]RecursoHumano RecursoHumano ) {
            if(_service.UserProjectCan((int)RecursoHumano.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(RecursoHumano);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]RecursoHumano RecursoHumano ) {
            if(_service.UserProjectCan((int)RecursoHumano.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(RecursoHumano);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var RecursoHumano = _service._context.RecursoHumanos.Where(rh => rh.Id == id).FirstOrDefault();

            if(RecursoHumano != null) {
                if(_service.UserProjectCan((int)RecursoHumano.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                    return _service.Excluir(id);
                return Forbid();
            }

            return NotFound();
        }
    }
}