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
    public class EtapasController : ControllerBase
    {
        private EtapaService _service;

        public EtapasController(EtapaService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/Etapas")]
        public IEnumerable<Etapa> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]Etapa Etapa)
        {
            return _service.Incluir(Etapa);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]Etapa Etapa)
        {
            return _service.Atualizar(Etapa);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}