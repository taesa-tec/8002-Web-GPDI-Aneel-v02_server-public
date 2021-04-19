using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            if (Proposta != null)
            {
                return Ok(Mapper.Map<PropostaDto>(Proposta));
            }

            return NotFound();
        }

        [HttpGet("Pdf/{form}")]
        public ActionResult GetDemandaPdf([FromRoute] Guid propostaId, string form,
            [FromServices] GestorDbContext context,
            [FromServices] DemandaService demandaService)
        {
            var id = context.Set<Captacao>()
                .Where(c => c.Id == Proposta.CaptacaoId)
                .Select(c => c.DemandaId)
                .FirstOrDefault();

            var info = context.Set<DemandaFormValues>()
                .FirstOrDefault(values => values.DemandaId == id && values.FormKey == form);

            if (info != null)
            {
                var filename = demandaService.GetDemandaFormPdfFilename(id, form, info.Revisao.ToString());

                if (System.IO.File.Exists(filename))
                {
                    var name = string.Format("demanda-{0}-{1}-rev-{2}.pdf", id, form, info.Revisao);
                    var response = PhysicalFile(filename, "application/pdf", name);
                    if (Request.Query["dl"] == "1")
                    {
                        response.FileDownloadName = name;
                    }

                    return response;
                }
            }

            return NotFound();
        }
    }
}