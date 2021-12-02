using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Authorizations;
using PeD.Core.Models;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [SwaggerTag("Contratos Padrão")]
    [Route("api/Sistema/Contratos")]
    [ApiController]
    [Authorize("Bearer")]
    public class ContratosController : ControllerCrudBase<Contrato>
    {
        public ContratosController(IService<Contrato> service, IMapper mapper) : base(service, mapper)
        {
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("Shortcodes")]
        public ActionResult GetShortCodes()
        {
            return Ok(ContratoService.GetShortcodesDescriptions());
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Post(Contrato model)
        {
            return base.Post(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Put(Contrato model)
        {
            return base.Put(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}