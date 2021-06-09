using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models.Projetos;
using PeD.Services.Projetos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [Route("api/Projetos/{id:int}/Orcamento")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProjetoOrcamento : ControllerServiceBase<Projeto>
    {
        private new ProjetoService Service;

        public ProjetoOrcamento(ProjetoService service, IMapper mapper) : base(service, mapper)
        {
            Service = service;
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            //Previsto
            var orcamentos = Service.GetOrcamentos(id);
            //Realizado
            var extratos = Service.GetExtratos(id);
            
            
            return Ok(orcamentos);
        }
    }
}