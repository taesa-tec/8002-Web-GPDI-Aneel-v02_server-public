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
    public class RelatorioFinalController : ControllerBase
    {
        private RelatorioFinalService _service;

        public RelatorioFinalController(RelatorioFinalService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/RelatorioFinal")]
        public ActionResult<RelatorioFinal> Get(int projetoId)
        {
            var RelatorioFinal = _service.Obter(projetoId);
            if (RelatorioFinal != null)
                return RelatorioFinal;
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]RelatorioFinal RelatorioFinal)
        {
            return _service.Incluir(RelatorioFinal);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]RelatorioFinal RelatorioFinal)
        {
            return _service.Atualizar(RelatorioFinal);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}