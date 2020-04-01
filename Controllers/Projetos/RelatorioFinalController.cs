using APIGestor.Models.Projetos;
using APIGestor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGestor.Controllers.Projetos {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RelatorioFinalController : ControllerBase {
        private RelatorioFinalService _service;

        public RelatorioFinalController( RelatorioFinalService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/RelatorioFinal")]
        public ActionResult<RelatorioFinal> Get( int projetoId ) {

            var RelatorioFinal = _service.Obter(projetoId);
            if(RelatorioFinal != null)
                return RelatorioFinal;
            else
                return NotFound();
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]RelatorioFinal RelatorioFinal ) {
            if(_service.UserProjectCan(RelatorioFinal.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(RelatorioFinal);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, RelatorioFinal.ProjetoId, RelatorioFinal);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]RelatorioFinal RelatorioFinal ) {
            var Relatorio = _service._context.RelatorioFinal.Find(RelatorioFinal.Id);
            if(Relatorio != null) {
                if(_service.UserProjectCan(Relatorio.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {

                    _service._context.Entry(Relatorio).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                    var resultado = _service.Atualizar(RelatorioFinal);

                    if(resultado.Sucesso) {
                        this.CreateLog(_service, Relatorio.ProjetoId, RelatorioFinal, Relatorio);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var Relatorio = _service.Obter(id);
            if(Relatorio != null) {
                if(_service.UserProjectCan(Relatorio.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, Relatorio.ProjetoId, Relatorio);
                    }
                    return resultado;
                }
                return Forbid();
            }
            return NotFound();

        }
    }
}