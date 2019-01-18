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
    public class RecursoMateriaisController : ControllerBase
    {
        private RecursoMaterialService _service;

        public RecursoMateriaisController(RecursoMaterialService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/RecursoMateriais")]
        public IEnumerable<RecursoMaterial> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]RecursoMaterial RecursoMaterial)
        {
            return _service.Incluir(RecursoMaterial);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]RecursoMaterial RecursoMaterial)
        {
            return _service.Atualizar(RecursoMaterial);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}