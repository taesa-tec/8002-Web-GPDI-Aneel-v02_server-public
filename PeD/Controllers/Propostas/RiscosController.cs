using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Propostas;
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
    public class RiscosController : PropostaNodeBaseController<Risco, RiscoRequest, PropostaRiscoDto>
    {
        public RiscosController(IService<Risco> service, IMapper mapper, IAuthorizationService authorizationService,
            PropostaService propostaService) : base(service, mapper, authorizationService, propostaService)
        {
        }
    }
}