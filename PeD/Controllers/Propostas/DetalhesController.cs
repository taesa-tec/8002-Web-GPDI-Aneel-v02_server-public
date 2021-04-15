using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Demandas;
using PeD.Data;
using PeD.Services.Captacoes;
using PeD.Services.Demandas;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{captacaoId:int}/[controller]")]
    public class DetalhesController : Controller
    {
        private IMapper Mapper;
        private PropostaService Service;

        public DetalhesController(PropostaService service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Index([FromRoute] int captacaoId)
        {
            var proposta = Service.GetProposta(captacaoId);
            return Ok(Mapper.Map<PropostaDto>(proposta));
        }

        [HttpGet("Pdf/{form}")]
        public ActionResult GetDemandaPdf([FromRoute] int captacaoId, string form,
            [FromServices] GestorDbContext context,
            [FromServices] DemandaService demandaService)
        {
            var id = context.Set<Captacao>()
                .Where(c => c.Id == captacaoId)
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