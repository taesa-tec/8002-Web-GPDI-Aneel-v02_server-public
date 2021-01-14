using System.Collections.Generic;
using PeD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Authorizations;
using PeD.Core.Models.Projetos;
using PeD.Services.Projetos;

namespace PeD.Controllers.Projetos {
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/projeto/")]
    public class AlocacaoRhsController : ControllerBase {
        private AlocacaoRhService _service;

        public AlocacaoRhsController( AlocacaoRhService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/AlocacaoRhs")]
        public IEnumerable<AlocacaoRh> Get( int projetoId ) {

            return _service.ListarTodos(projetoId);
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]AlocacaoRh AlocacaoRh ) {
            if(_service.UserProjectCan((int)AlocacaoRh.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(AlocacaoRh);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)AlocacaoRh.ProjetoId, _service.Obter(AlocacaoRh.Id));
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]AlocacaoRh AlocacaoRh ) {
            if(_service.UserProjectCan((int)AlocacaoRh.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var oldAlocacao = _service.Obter(AlocacaoRh.Id);
                _service._context.Entry(oldAlocacao).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(AlocacaoRh);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)AlocacaoRh.ProjetoId, _service.Obter(AlocacaoRh.Id), oldAlocacao);
                }

                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var alocacao = _service.Obter(id);
            if(alocacao != null) {
                if(_service.UserProjectCan((int)alocacao.ProjetoId, User, ProjectPermissions.Administrator)) {
                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, (int)alocacao.ProjetoId, alocacao);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();
        }
    }
}