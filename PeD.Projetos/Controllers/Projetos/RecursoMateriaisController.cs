using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeD.Projetos.Controllers.Projetos {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]RecursoMaterial RecursoMaterial ) {
            if(_service.UserProjectCan((int)RecursoMaterial.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(RecursoMaterial);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)RecursoMaterial.ProjetoId, RecursoMaterial);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]RecursoMaterial RecursoMaterial ) {
            if(_service.UserProjectCan((int)RecursoMaterial.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var oldRecurso = _service.Obter(RecursoMaterial.Id);
                _service._context.Entry(oldRecurso).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(RecursoMaterial);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)RecursoMaterial.ProjetoId, _service.Obter(RecursoMaterial.Id), oldRecurso);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var recurso = this._service._context.RecursoMateriais.Where(r => r.Id == id).FirstOrDefault();
            if(recurso != null) {
                if(_service.UserProjectCan((int)recurso.ProjetoId, User, ProjectPermissions.Administrator)) {

                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, (int)recurso.ProjetoId, recurso);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}