using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business.Sistema;
using APIGestor.Core.Equipe;

namespace APIGestor.Controllers.Sistema
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
        public EquipePeD GetEquipePeD()
        {
            return sistemaService.GetEquipePeD();
        }
        [HttpPut]
        public ActionResult SetEquipePeD(EquipePeD equipePeD)
        {
            sistemaService.SetEquipePeD(equipePeD);
            return Ok();
        }


    }
}