using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models.Captacoes;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;

namespace PeD.Controllers.Captacoes
{
    [SwaggerTag("Proposta Fornecedor")]
    [Route("api/Captacoes/Fornecedor")]
    [ApiController]
    [Authorize("Bearer")]
    public class FornecedorController : ControllerServiceBase<Captacao>
    {
        private CaptacaoService service;

        public FornecedorController(CaptacaoService service, IMapper mapper, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(service, mapper)
        {
            urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            this.service = service;
        }

        [HttpGet("")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            var propostas = service.GetPropostasPorResponsavel(this.UserId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("{id}")]
        public ActionResult<PropostaDto> GetProposta(int id)
        {
            var proposta = service.GetProposta(id);
            return Mapper.Map<PropostaDto>(proposta);
        }

        [HttpGet("{id}/Detalhes")]
        public ActionResult<PropostaDto> GetPropostaDetalhes(int id)
        {
            var proposta = service.GetProposta(id);
            return Mapper.Map<PropostaDto>(proposta);
        }
    }
}