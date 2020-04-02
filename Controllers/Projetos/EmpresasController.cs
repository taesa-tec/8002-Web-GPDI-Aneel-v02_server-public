using System.Collections.Generic;
using APIGestor.Models.Projetos;
using APIGestor.Services;
using APIGestor.Services.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGestor.Controllers.Projetos {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]Empresa Empresa ) {
            if(_service.UserProjectCan(Empresa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {

                var resultado = _service.Incluir(Empresa);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Empresa.ProjetoId, Empresa);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]Empresa Empresa ) {
            if(_service.UserProjectCan(Empresa.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var empresaOld = _service.Obter(Empresa.Id);
                _service._context.Entry(empresaOld).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(Empresa);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Empresa.ProjetoId, _service.Obter(Empresa.Id), empresaOld);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {

            var Empresa = _service.Obter(id);

            if(Empresa != null) {
                if(_service.UserProjectCan(Empresa.ProjetoId, User, Authorizations.ProjectPermissions.Administrator)) {
                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, Empresa.ProjetoId, Empresa);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}