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
    public class TemasController : ControllerBase
    {
        private TemaService _service;

        public TemasController(TemaService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/Temas")]
        public ActionResult<Tema> Get(int projetoId)
        {
            return _service.ListarTema(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]Tema Tema)
        {
            return _service.Incluir(Tema);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]Tema Tema)
        {
            return _service.Atualizar(Tema);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}