using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.Linq;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class LogProjetosController : ControllerBase
    {
        private LogProjetoService _service;

        public LogProjetosController(LogProjetoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/log")]
        public object Get(int projetoId, Acoes? acao, string user=null, int pag=1, int size=30)
        {
            var logs = _service.ListarTodos(projetoId, acao, user, pag, size);

            return new{
                Total = logs.Count(),
                Itens =  logs
                        .OrderByDescending(p=>p.Created)
                        .Skip((pag-1) * size)
                        .Take(size)
            };
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post([FromBody]LogProjeto LogProjeto)
        {
            return _service.Incluir(LogProjeto);
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}