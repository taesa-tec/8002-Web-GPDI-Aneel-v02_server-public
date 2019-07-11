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
    public class EmpresasController : ControllerBase {
        private EmpresaService _service;

        public EmpresasController( EmpresaService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/Empresas")]
        public IEnumerable<Empresa> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]Empresa Empresa ) {
            if(_service.UserProjectCan(Empresa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(Empresa);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]Empresa Empresa ) {
            if(_service.UserProjectCan(Empresa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(Empresa);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Empresa = _service._context.Empresas.Where(e => e.Id == id).FirstOrDefault();

            if(Empresa != null) {
                if(_service.UserProjectCan(Empresa.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
                    return _service.Excluir(id);
                return Forbid();
            }
            return NotFound();
        }
    }
}