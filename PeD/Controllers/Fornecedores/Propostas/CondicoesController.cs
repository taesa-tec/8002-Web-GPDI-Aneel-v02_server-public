using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class CondicoesController : ControllerBase
    {
        private IService<Clausula> Service;
        private IMapper Mapper;

        public CondicoesController(IService<Clausula> service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        [SwaggerOperation("Lista das clausulas")]
        [HttpGet("/api/Fornecedor/Clausulas")]
        public ActionResult<List<Clausula>> Index()
        {
            var clausulas = Service.Get();
            return Ok(clausulas);
        }

        [SwaggerOperation("Aceitação dos termos das clausulas")]
        [HttpPost]
        public ActionResult AceitarClausula([FromServices] PropostaService propostaService, [FromRoute] int captacaoId,
            CondicoesRequest request)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta != null)
            {
                if (request.ClausulasAceita)
                {
                    proposta.DataClausulasAceitas = DateTime.Now;
                    propostaService.Put(proposta);
                }
                else
                {
                    proposta.Participacao = StatusParticipacao.Rejeitado;
                    proposta.DataResposta = DateTime.Now;
                    propostaService.Put(proposta);
                    propostaService.SendEmailFinalizado(proposta).Wait();
                }


                return Ok(Mapper.Map<PropostaDto>(proposta));
            }

            return NotFound();
        }
    }
}