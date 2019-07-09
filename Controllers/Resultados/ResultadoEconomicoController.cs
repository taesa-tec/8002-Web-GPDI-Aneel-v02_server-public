using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResultadoEconomicoController : ControllerBase
    {
        private ResultadoEconomicoService _service;

        public ResultadoEconomicoController(ResultadoEconomicoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoEconomico")]
        public IEnumerable<ResultadoEconomico> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoEconomico/{Id}")]
        public ActionResult<ResultadoEconomico> GetA(int id)
        {
            var ResultadoEconomico = _service.Obter(id);
            if (ResultadoEconomico != null)
                return ResultadoEconomico;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post([FromBody]ResultadoEconomico ResultadoEconomico)
        {
            return _service.Incluir(ResultadoEconomico);
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put([FromBody]ResultadoEconomico ResultadoEconomico)
        {
            return _service.Atualizar(ResultadoEconomico);
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}