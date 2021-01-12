using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGestor.Data;
using APIGestor.Dtos.Captacao;
using APIGestor.Dtos.Captacao.Fornecedor;
using APIGestor.Models.Captacao;
using APIGestor.Requests.Captacao;
using APIGestor.Services.Captacoes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace APIGestor.Controllers.Captacoes
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
    }
}