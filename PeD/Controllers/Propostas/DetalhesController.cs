using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using PeD.Services.Demandas;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class DetalhesController : PropostaNodeBaseController<Proposta>
    {
        public DetalhesController(IService<Proposta> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
        }

        [HttpGet("")]
        public async Task<ActionResult> Index()
        {
            if (Proposta != null)
            {
                var authorizationResult = await this.Authorize(Proposta);
                if (authorizationResult.Succeeded)
                    return Ok(Mapper.Map<PropostaDto>(Proposta));
            }

            return NotFound();
        }

        [HttpGet("Pdf/{form}")]
        public async Task<ActionResult> GetDemandaPdf([FromRoute] Guid propostaId, string form,
            [FromServices] GestorDbContext context,
            [FromServices] DemandaService demandaService)
        {
            var authorizationResult = await this.Authorize(Proposta);
            if (!authorizationResult.Succeeded)
                return NotFound();

            var captacao = context.Set<Captacao>()
                .Include(c => c.EspecificacaoTecnicaFile)
                .Where(c => c.Id == Proposta.CaptacaoId)
                .FirstOrDefault();
            if (captacao is null)
                return NotFound();
            var file = captacao.EspecificacaoTecnicaFile;

            if (file is null)
                return NotFound();

            return PhysicalFile(file.Path, file.ContentType, file.Name ?? $"{form}.pdf");
        }
    }
}