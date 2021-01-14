using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using PeD.Dtos.FornecedoresDtos;
using PeD.Models.Captacao;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedor
{
    [SwaggerTag("Proposta Fornecedor")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Fornecedor/Propostas")]
    public class PropostasController : ControllerServiceBase<PropostaFornecedor>
    {
        private IUrlHelper _urlHelper;
        private CaptacaoService _captacaoService;


        public PropostasController(CaptacaoService captacaoService, IService<PropostaFornecedor> service,
            IMapper mapper, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(service, mapper)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            this._captacaoService = captacaoService;
        }

        [HttpGet("")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            var propostas = _captacaoService.GetPropostasPorResponsavel(this.userId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("{id}")]
        public ActionResult<PropostaDto> GetProposta(int id)
        {
            var proposta = _captacaoService.GetProposta(id);
            return Mapper.Map<PropostaDto>(proposta);
        }

        [HttpGet("{id}/Detalhes")]
        public ActionResult<PropostaDto> GetPropostaDetalhes(int id)
        {
            var proposta = _captacaoService.GetProposta(id);
            return Mapper.Map<PropostaDto>(proposta);
        }
    }
}