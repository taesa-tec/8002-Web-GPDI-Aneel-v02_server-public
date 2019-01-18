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
    public class AlocacaoRmsController : ControllerBase
    {
        private AlocacaoRmService _service;

        public AlocacaoRmsController(AlocacaoRmService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/AlocacaoRms")]
        public IEnumerable<AlocacaoRm> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]AlocacaoRm AlocacaoRm)
        {
            return _service.Incluir(AlocacaoRm);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]AlocacaoRm AlocacaoRm)
        {
            return _service.Atualizar(AlocacaoRm);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}