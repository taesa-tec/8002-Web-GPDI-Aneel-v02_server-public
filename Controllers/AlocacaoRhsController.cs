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
    public class AlocacaoRhsController : ControllerBase
    {
        private AlocacaoRhService _service;

        public AlocacaoRhsController(AlocacaoRhService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/AlocacaoRhs")]
        public IEnumerable<AlocacaoRh> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]AlocacaoRh AlocacaoRh)
        {
            return _service.Incluir(AlocacaoRh);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]AlocacaoRh AlocacaoRh)
        {
            return _service.Atualizar(AlocacaoRh);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}