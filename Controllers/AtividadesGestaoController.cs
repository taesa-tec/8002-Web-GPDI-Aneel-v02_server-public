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
    public class AtividadesGestaoController : ControllerBase
    {
        private AtividadeGestaoService _service;

        public AtividadesGestaoController(AtividadeGestaoService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/AtividadesGestao")]
        public ActionResult<AtividadesGestao> Get(int projetoId)
        {
            return _service.Obter(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]AtividadesGestao dados)
        {
            return _service.Incluir(dados);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]AtividadesGestao dados)
        {
            return _service.Atualizar(dados);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}