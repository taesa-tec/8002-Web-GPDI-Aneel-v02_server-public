using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using PeD.Core.ApiModels.FornecedoresDtos;
using PeD.Core.Extensions;
using PeD.Core.Models;
using PeD.Core.Models.Captacao;
using PeD.Fornecedor.Services;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Fornecedor.Controllers.Propostas
{
    [SwaggerTag("Proposta Fornecedor")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
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
            var propostas = _captacaoService.GetPropostasPorResponsavel(this.UserId());
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