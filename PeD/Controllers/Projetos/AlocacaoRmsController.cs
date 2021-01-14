using System.Collections.Generic;
using PeD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Models.Projetos;
using PeD.Services.Projetos;

namespace PeD.Controllers.Projetos {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]AlocacaoRm AlocacaoRm ) {
            if(_service.UserProjectCan((int)AlocacaoRm.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(AlocacaoRm);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)AlocacaoRm.ProjetoId, AlocacaoRm);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]AlocacaoRm AlocacaoRm ) {
            if(_service.UserProjectCan((int)AlocacaoRm.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var oldAlocacao = _service.Obter(AlocacaoRm.Id);
                _service._context.Entry(oldAlocacao).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(AlocacaoRm);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)AlocacaoRm.ProjetoId, _service.Obter(AlocacaoRm.Id), oldAlocacao);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Alocacao = _service.Obter(id);
            if(Alocacao != null) {
                if(_service.UserProjectCan((int)Alocacao.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, (int)Alocacao.ProjetoId, Alocacao);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}