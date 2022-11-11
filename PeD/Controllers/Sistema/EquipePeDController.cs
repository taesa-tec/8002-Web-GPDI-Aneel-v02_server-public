using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PeD.Authorizations;
using PeD.Core;
using PeD.Services.Sistema;

namespace PeD.Controllers.Sistema
{
    [Route("api/Sistema/Equipe")]
    [ApiController]
    [Authorize("Bearer")]
    public class EquipePeDController : ControllerBase
    {
        SistemaService sistemaService;

        public EquipePeDController(SistemaService sistemaService)
        {
            this.sistemaService = sistemaService;
        }

        [Authorize(Policy = Policies.IsColaborador)]
        [HttpGet]
        public object GetEquipePeD()
        {
            return sistemaService.GetEquipePedUsers();
        }

        [Authorize(Policy = Policies.IsAdmin)]
        [HttpPut]
        public ActionResult SetEquipePeD(EquipePeD equipePeD)
        {
            sistemaService.SetEquipePeD(equipePeD);
            return Ok();
        }
    }
}