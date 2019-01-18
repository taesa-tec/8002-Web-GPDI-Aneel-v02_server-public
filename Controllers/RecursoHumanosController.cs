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
    public class RecursoHumanosController : ControllerBase
    {
        private RecursoHumanoService _service;

        public RecursoHumanosController(RecursoHumanoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/RecursoHumanos")]
        public IEnumerable<RecursoHumano> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]RecursoHumano RecursoHumano)
        {
            return _service.Incluir(RecursoHumano);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]RecursoHumano RecursoHumano)
        {
            return _service.Atualizar(RecursoHumano);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}