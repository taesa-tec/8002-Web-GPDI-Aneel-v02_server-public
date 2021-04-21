using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class PlanoTrabalhoController : PropostaNodeBaseController<PlanoTrabalho>
    {
        public PlanoTrabalhoController(IService<PlanoTrabalho> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
        }

        [HttpGet("")]
        public async Task<ActionResult<PlanoTrabalhoDto>> Get()
        {
            if (Proposta == null)
                return NotFound();

            if (!await HasAccess())
                return Forbid();
            var plano = Service.Filter(q => q
                            .Include(p => p.Proposta)
                            .ThenInclude(p => p.Arquivos)
                            .ThenInclude(a => a.Arquivo)
                            .Where(p => p.PropostaId == Proposta.Id)).FirstOrDefault() ??
                        new PlanoTrabalho();
            return Mapper.Map<PlanoTrabalhoDto>(plano);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost("")]
        public async Task<ActionResult> Post([FromBody] PlanoTrabalhoRequest request)
        {
            if (Proposta == null)
                return NotFound();
            if (!await HasAccess())
                return Forbid();


            var planoPrev = Service.Filter(q => q.AsNoTracking().Where(p => p.PropostaId == Proposta.Id))
                .FirstOrDefault();
            var plano = Mapper.Map<PlanoTrabalho>(request);
            plano.PropostaId = Proposta.Id;

            if (planoPrev != null)
            {
                plano.Id = planoPrev.Id;
                Service.Put(plano);
            }
            else
            {
                Service.Post(plano);
            }

            PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);

            return Ok();
        }
    }
}