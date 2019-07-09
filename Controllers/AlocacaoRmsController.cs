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
    public class AlocacaoRmsController : ControllerBase {
        private AlocacaoRmService _service;

        public AlocacaoRmsController( AlocacaoRmService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/AlocacaoRms")]
        public IEnumerable<AlocacaoRm> Get( int projetoId ) {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]AlocacaoRm AlocacaoRm ) {
            if(_service.UserProjectCan((int)AlocacaoRm.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Incluir(AlocacaoRm);
            }
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]AlocacaoRm AlocacaoRm ) {
            if(_service.UserProjectCan((int)AlocacaoRm.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Atualizar(AlocacaoRm);
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Alocacao = _service._context.AlocacoesRm.Where(a => a.Id == id).FirstOrDefault();
            if(Alocacao != null) {
                if(_service.UserProjectCan((int)Alocacao.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                    return _service.Excluir(id);
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}