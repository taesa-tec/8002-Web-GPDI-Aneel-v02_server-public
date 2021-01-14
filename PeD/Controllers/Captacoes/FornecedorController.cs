using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeD.Data;
using PeD.Dtos.Captacao;
using PeD.Requests.Captacao;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using PeD.Dtos.FornecedoresDtos;
using PeD.Models.Captacao;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Captacoes
{
    [SwaggerTag("Proposta Fornecedor")]
    [Route("api/Captacoes/Fornecedor")]
    [ApiController]
    [Authorize("Bearer")]
    public class FornecedorController : ControllerServiceBase<Captacao>
    {
        private IUrlHelper _urlHelper;
        private CaptacaoService service;

        public FornecedorController(CaptacaoService service, IMapper mapper, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(service, mapper)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            this.service = service;
        }

        [HttpGet("")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            var propostas = service.GetPropostasPorResponsavel(this.userId());
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