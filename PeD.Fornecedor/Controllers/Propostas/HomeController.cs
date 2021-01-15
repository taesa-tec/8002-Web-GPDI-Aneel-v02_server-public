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
        private new PropostaService Service;

        public PropostasController(PropostaService service,
            IMapper mapper, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(service, mapper)
        {
            Service = service;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HttpGet("")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            var propostas = Service.GetPropostasPorResponsavel(this.UserId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("{id}")]
        public ActionResult<PropostaDto> GetProposta(int id)
        {
            var proposta = Service.GetProposta(id);
            return Mapper.Map<PropostaDto>(proposta);
        }
    }
}