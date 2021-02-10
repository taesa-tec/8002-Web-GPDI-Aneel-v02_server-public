using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models;

namespace PeD.Projetos.Controllers.Projetos {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class LogProjetosController : ControllerBase {
        private LogService _service;

        public LogProjetosController( LogService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/log")]
        public object Get( int projetoId, Acoes? acao, string user = null, int pag = 1, int size = 30 ) {
            var logs = _service.ListarTodos(projetoId, acao, user, pag, size);
            return new {
                Total = logs.Count(),
                Itens = logs
                        .OrderByDescending(p => p.Created)
                        .Skip((pag - 1) * size)
                        .Take(size)
            };
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]LogProjeto LogProjeto ) {
            return Ok();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            return _service.Excluir(id);
        }
    }
}