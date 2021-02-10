using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeD.Projetos.Controllers.Projetos
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post([FromBody]AtividadesGestao dados)
        {
            return _service.Incluir(dados);
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put([FromBody]AtividadesGestao dados)
        {
            return _service.Atualizar(dados);
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}