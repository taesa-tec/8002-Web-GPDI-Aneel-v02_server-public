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
    public class ResultadoInfraController : ControllerBase
    {
        private ResultadoInfraService _service;

        public ResultadoInfraController(ResultadoInfraService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoInfra")]
        public IEnumerable<ResultadoInfra> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoInfra/{Id}")]
        public ActionResult<ResultadoInfra> GetA(int id)
        {
            var ResultadoInfra = _service.Obter(id);
            if (ResultadoInfra != null)
                return ResultadoInfra;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post([FromBody]ResultadoInfra ResultadoInfra)
        {
            return _service.Incluir(ResultadoInfra);
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put([FromBody]ResultadoInfra ResultadoInfra)
        {
            return _service.Atualizar(ResultadoInfra);
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}