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
    public class ResultadoProducaoController : ControllerBase
    {
        private ResultadoProducaoService _service;

        public ResultadoProducaoController(ResultadoProducaoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoProducao")]
        public IEnumerable<ResultadoProducao> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoProducao/{Id}")]
        public ActionResult<ResultadoProducao> GetA(int id)
        {
            var ResultadoProducao = _service.Obter(id);
            if (ResultadoProducao != null)
                return ResultadoProducao;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post([FromBody]ResultadoProducao ResultadoProducao)
        {
            return _service.Incluir(ResultadoProducao);
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put([FromBody]ResultadoProducao ResultadoProducao)
        {
            return _service.Atualizar(ResultadoProducao);
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}