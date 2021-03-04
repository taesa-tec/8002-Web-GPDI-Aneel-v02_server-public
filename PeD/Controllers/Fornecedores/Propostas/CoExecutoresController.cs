using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Extensions;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class CoExecutoresController : ControllerBase
    {
        private PropostaService propostaService;
        private CoExecutorService Service;
        private IMapper mapper;

        public CoExecutoresController(CoExecutorService service, IMapper mapper, PropostaService propostaService)
        {
            Service = service;
            this.mapper = mapper;
            this.propostaService = propostaService;
        }

        [HttpGet]
        public ActionResult<List<CoExecutorDto>> Get([FromRoute] int captacaoId)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var ces = Service.GetPorProposta(proposta.Id);
            return mapper.Map<List<CoExecutorDto>>(ces);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] CoExecutorRequest request)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());

            var coExecutor = new CoExecutor()
            {
                PropostaId = proposta.Id,
                RazaoSocial = request.RazaoSocial,
                UF = request.UF,
                CNPJ = request.CNPJ,
                Funcao = request.Funcao
            };
            Service.Save(coExecutor);
            return Ok();
        }

        [HttpPut]
        public ActionResult Put([FromRoute] int captacaoId, [FromBody] CoExecutorRequest request)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var coExecutor = Service.Get(request.Id);
            if (coExecutor.PropostaId == proposta.Id)
            {
                coExecutor.Id = request.Id;
                coExecutor.PropostaId = proposta.Id;
                coExecutor.RazaoSocial = request.RazaoSocial;
                coExecutor.UF = request.UF;
                coExecutor.CNPJ = request.CNPJ;
                coExecutor.Funcao = request.Funcao;
                Service.Save(coExecutor);
                return Ok();
            }

            return Forbid();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var coExecutor = Service.Get(id);
            if (coExecutor == null)
            {
                return NotFound();
            }

            if (coExecutor.PropostaId == proposta.Id)
            {
                Service.Delete(id);
                return Ok();
            }

            return Forbid();
        }
    }
}