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
    public class ResultadoCapacitacaoController : ControllerBase
    {
        private ResultadoCapacitacaoService _service;

        public ResultadoCapacitacaoController(ResultadoCapacitacaoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/ResultadoCapacitacao")]
        public IEnumerable<ResultadoCapacitacao> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [HttpGet("ResultadoCapacitacao/{Id}")]
        public ActionResult<ResultadoCapacitacao> GetA(int id)
        {
            var ResultadoCapacitacao = _service.Obter(id);
            if (ResultadoCapacitacao != null)
                return ResultadoCapacitacao;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post([FromBody]ResultadoCapacitacao ResultadoCapacitacao)
        {
            return _service.Incluir(ResultadoCapacitacao);
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put([FromBody]ResultadoCapacitacao ResultadoCapacitacao)
        {
            return _service.Atualizar(ResultadoCapacitacao);
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}