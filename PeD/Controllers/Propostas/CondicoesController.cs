using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class CondicoesController : PropostaNodeBaseController<Clausula>
    {
        public CondicoesController(IService<Clausula> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
        }

        [SwaggerOperation("Lista das clausulas")]
        [HttpGet("/api/Clausulas")]
        public ActionResult<List<Clausula>> Index()
        {
            var clausulas = Service.Get();
            return Ok(clausulas);
        }

        [SwaggerOperation("Aceitação dos termos das clausulas")]
        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public async Task<ActionResult> AceitarClausula([FromServices] PropostaService propostaService,
            [FromRoute] int captacaoId,
            CondicoesRequest request)
        {
            if (!await HasAccess())
                return Forbid();

            if (Proposta != null)
            {
                if (request.ClausulasAceita)
                {
                    Proposta.DataClausulasAceitas = DateTime.Now;
                    propostaService.Put(Proposta);
                }
                else
                {
                    Proposta.Participacao = StatusParticipacao.Rejeitado;
                    Proposta.DataResposta = DateTime.Now;
                    propostaService.Put(Proposta);
                    propostaService.SendEmailFinalizado(Proposta).Wait();
                }


                return Ok(Mapper.Map<PropostaDto>(Proposta));
            }

            return NotFound();
        }
    }
}