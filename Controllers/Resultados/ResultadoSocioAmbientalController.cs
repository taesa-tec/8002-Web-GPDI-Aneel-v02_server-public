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
    public class ResultadoSocioAmbientalController : ControllerBase
    {
        private ResultadoSocioAmbientalService _service;

        public ResultadoSocioAmbientalController(ResultadoSocioAmbientalService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoSocioAmbiental")]
        public IEnumerable<ResultadoSocioAmbiental> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoSocioAmbiental/{Id}")]
        public ActionResult<ResultadoSocioAmbiental> GetA(int id)
        {
            var ResultadoSocioAmbiental = _service.Obter(id);
            if (ResultadoSocioAmbiental != null)
                return ResultadoSocioAmbiental;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]ResultadoSocioAmbiental ResultadoSocioAmbiental)
        {
            return _service.Incluir(ResultadoSocioAmbiental);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]ResultadoSocioAmbiental ResultadoSocioAmbiental)
        {
            return _service.Atualizar(ResultadoSocioAmbiental);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}