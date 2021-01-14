using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PeD.Core.Equipe;
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

        [HttpGet]
        public object GetEquipePeD()
        {
            return sistemaService.GetEquipePedUsers();
        }
        [HttpPut]
        public ActionResult SetEquipePeD(EquipePeD equipePeD)
        {
            sistemaService.SetEquipePeD(equipePeD);
            return Ok();
        }


    }
}