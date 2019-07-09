using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RecursoMateriaisController : ControllerBase {
        private RecursoMaterialService _service;

        public RecursoMateriaisController( RecursoMaterialService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/RecursoMateriais")]
        public IEnumerable<RecursoMaterial> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]RecursoMaterial RecursoMaterial ) {
            if(_service.UserProjectCan((int)RecursoMaterial.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Incluir(RecursoMaterial);
            }
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]RecursoMaterial RecursoMaterial ) {
            if(_service.UserProjectCan((int)RecursoMaterial.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Atualizar(RecursoMaterial);
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var recurso = this._service._context.RecursoMateriais.Where(r => r.Id == id).FirstOrDefault();
            if(recurso != null) {
                _service.UserProjectCan((int)recurso.ProjetoId, User, Authorizations.ProjectPermissions.Administrator);
            }

            return _service.Excluir(id);
        }
    }
}