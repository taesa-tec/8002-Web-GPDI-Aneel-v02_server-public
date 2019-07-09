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
    public class TemasController : ControllerBase {
        private TemaService _service;

        public TemasController( TemaService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/Temas")]
        public ActionResult<Tema> Get( int projetoId ) {
            if(this._service.UserProjectCan(projetoId, User)) {
                return _service.ListarTema(projetoId);
            }
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]Tema Tema ) {
            if(this._service.UserProjectCan(Tema.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Incluir(Tema);
            }
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]Tema Tema ) {
            if(this._service.UserProjectCan(Tema.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Atualizar(Tema);
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var tema = _service._context.Temas.First(t => t.Id == id);
            if(this._service.UserProjectCan(tema.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Excluir(id);
            }
            return Forbid();
        }
    }
}