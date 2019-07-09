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
    public class AlocacaoRhsController : ControllerBase {
        private AlocacaoRhService _service;

        public AlocacaoRhsController( AlocacaoRhService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/AlocacaoRhs")]
        public IEnumerable<AlocacaoRh> Get( int projetoId ) {

            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]AlocacaoRh AlocacaoRh ) {
            if(_service.UserProjectCan((int)AlocacaoRh.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(AlocacaoRh);
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]AlocacaoRh AlocacaoRh ) {
            if(_service.UserProjectCan((int)AlocacaoRh.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(AlocacaoRh);
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var alocacao = _service._context.AlocacoesRh.Where(a => a.Id == id).FirstOrDefault();
            if(alocacao != null) {
                if(_service.UserProjectCan((int)alocacao.ProjetoId, User, Authorizations.ProjectPermissions.Administrator))
                    return _service.Excluir(id);
                return Forbid();
            }
            return NotFound();
        }
    }
}