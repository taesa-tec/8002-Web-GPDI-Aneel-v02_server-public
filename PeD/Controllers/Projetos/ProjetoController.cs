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
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [Route("api/Projetos")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProjetoController : ControllerServiceBase<Projeto>
    {
        private new ProjetoService Service;

        public ProjetoController(ProjetoService service, IMapper mapper) : base(service, mapper)
        {
            Service = service;
        }

        [HttpGet("EmExecucao")]
        public ActionResult GetEmExecucao()
        {
            var projetos = Service.Filter(q => q.Where(p => p.Status == Status.Execucao));
            return Ok(Mapper.Map<List<ProjetoDto>>(projetos));
        }

        [HttpGet("Finalizados")]
        public ActionResult GetFinalizados()
        {
            var projetos = Service.Filter(q => q.Where(p => p.Status == Status.Finalizado));
            return Ok(Mapper.Map<List<ProjetoDto>>(projetos));
        }

        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var projeto = Service.Filter(q =>
                q.Include(p => p.Proponente)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();

            return Ok(Mapper.Map<ProjetoDto>(projeto));
        }
        
        
    }
}