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
    public class ResultadoIntelectualController : ControllerBase
    {
        private ResultadoIntelectualService _service;

        public ResultadoIntelectualController(ResultadoIntelectualService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoIntelectual")]
        public IEnumerable<ResultadoIntelectual> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoIntelectual/{Id}")]
        public ActionResult<ResultadoIntelectual> GetA(int id)
        {
            var ResultadoIntelectual = _service.Obter(id);
            if (ResultadoIntelectual != null)
                return ResultadoIntelectual;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]ResultadoIntelectual ResultadoIntelectual)
        {
            return _service.Incluir(ResultadoIntelectual);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]ResultadoIntelectual ResultadoIntelectual)
        {
            return _service.Atualizar(ResultadoIntelectual);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}