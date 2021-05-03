using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Propostas 2.4")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas")]
    public class PropostasController : ControllerServiceBase<Proposta>
    {
        private new PropostaService Service;
        public readonly IAuthorizationService AuthorizationService;

        public PropostasController(PropostaService service, IMapper mapper, IAuthorizationService authorizationService)
            : base(service, mapper)
        {
            Service = service;
            AuthorizationService = authorizationService;
        }


        #region Get Proposta

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PropostaDto>> GetProposta(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);

            if (authorizationResult.Succeeded)
            {
                return Mapper.Map<PropostaDto>(proposta);
            }

            return Forbid();
        }

        #endregion

        #region Empresas Relacionadas a proposta

        [HttpGet("{id:guid}/Empresas")]
        public async Task<ActionResult> GetPropostaEmpresas(Guid id,
            [FromServices] IService<CoExecutor> coexecutoresService,
            [FromServices] IService<Empresa> empresasService,
            [FromServices] IService<Fornecedor> fornecedorService
        )
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);

            if (authorizationResult.Succeeded)
            {
                var empresa = empresasService.Filter(q => q.OrderBy(e => e.Id).Take(1)).FirstOrDefault();
                var fornecedor = fornecedorService.Filter(q =>
                    q.Where(e => e.ResponsavelId == proposta.ResponsavelId)).FirstOrDefault();
                var coExecutores = coexecutoresService.Filter(q =>
                    q.Where(e => e.PropostaId == proposta.Id));
                var response = new
                {
                    empresa,
                    fornecedor,
                    coExecutores = Mapper.Map<List<CoExecutorDto>>(coExecutores)
                };
                return Ok(response);
            }

            return Forbid();
        }

        #endregion


        [HttpGet("{id:guid}/Documento")]
        public async Task<ActionResult> PropostaDoc(Guid id)
        {
            var tempproposta = Service.GetProposta(id);
            if (tempproposta == null)
            {
                return NotFound();
            }

            var authorizationResult = await this.Authorize(tempproposta);
            if (!authorizationResult.Succeeded)
                return Forbid();

            var relatorio = Service.GetRelatorio(tempproposta.Id);
            if (relatorio != null)
            {
                return Ok(new
                {
                    relatorio.Content,
                    relatorio.Validacao
                });
            }

            return NotFound();
        }

        #region Downloads

        [HttpGet("{id:guid}/Download/PlanoTrabalho")]
        public async Task<ActionResult> PropostaDocDownload(Guid id)
        {
            var tempproposta = Service.GetProposta(id);
            if (tempproposta == null || tempproposta.Captacao?.Status < Captacao.CaptacaoStatus.Fornecedor)
            {
                return NotFound();
            }

            var authorizationResult = await this.Authorize(tempproposta);
            if (!authorizationResult.Succeeded)
                return Forbid();
            var relatorio = Service.GetRelatorioPdf(tempproposta.Id);
            if (relatorio != null)
            {
                return PhysicalFile(relatorio.Path, "application/pdf", relatorio.FileName);
            }

            return NotFound();
        }

        [HttpGet("{id:guid}/Download/Contrato")]
        public async Task<ActionResult> PropostaContratoDownload(Guid id)
        {
            var tempproposta = Service.GetProposta(id);
            if (tempproposta == null || tempproposta.Captacao?.Status < Captacao.CaptacaoStatus.Fornecedor)
            {
                return NotFound();
            }

            var authorizationResult = await this.Authorize(tempproposta);
            if (!authorizationResult.Succeeded)
                return Forbid();

            var result = await this.Authorize(tempproposta);
            if (result.Succeeded)
            {
                var contrato = Service.GetContratoPdf(tempproposta.Id);
                if (contrato != null)
                {
                    return PhysicalFile(contrato.Path, "application/octet-stream", contrato.FileName);
                }

                return NotFound();
            }

            return Forbid();
        }

        #endregion

        #region Fornecedor

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpGet("EmAberto")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            var propostas = Service.GetPropostasPorResponsavel(this.UserId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpGet("Encerradas")]
        public ActionResult<List<PropostaDto>> GetPropostasEncerradas()
        {
            var propostas = Service.GetPropostasEncerradas(this.UserId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut("{id:guid}/Rejeitar")]
        public async Task<ActionResult> RejeitarCaptacao(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var result = await this.Authorize(proposta);
            if (!result.Succeeded)
                return Forbid();

            if (proposta.Participacao == StatusParticipacao.Pendente)
            {
                proposta.Participacao = StatusParticipacao.Rejeitado;
                proposta.DataResposta = DateTime.Now;
                Service.Put(proposta);
                Service.SendEmailFinalizado(proposta).Wait();
                return Ok();
            }

            return StatusCode(428);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut("{id:guid}/Participar")]
        public async Task<ActionResult> ParticiparCaptacao(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var result = await this.Authorize(proposta);
            if (!result.Succeeded)
                return Forbid();

            if (proposta.Participacao == StatusParticipacao.Pendente)
            {
                proposta.Participacao = StatusParticipacao.Aceito;
                proposta.DataParticipacao = DateTime.Now;
                Service.Put(proposta);
                return Ok();
            }

            return StatusCode(428);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut("{id:guid}/Duracao")]
        public async Task<ActionResult> PropostaDuracao(Guid id, [FromServices] IService<Etapa> etapaService,
            [FromBody] short meses)
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);
            if (!authorizationResult.Succeeded)
                return Forbid();

            if (proposta.Participacao == StatusParticipacao.Aceito)
            {
                var etapas = etapaService.Filter(q => q.Where(e => e.PropostaId == proposta.Id));
                var max = etapas.Any() ? etapas.Select(e => e.Meses.Max()).Max() : 0;
                if (meses < max)
                    return Problem("Há etapas em meses superiores à duração desejada",
                        title: "Alteração não permitida", statusCode: StatusCodes.Status428PreconditionRequired);
                proposta.Duracao = meses;
                proposta.DataAlteracao = DateTime.Now;
                Service.Put(proposta);
                return Ok();
            }

            return Problem("Participação não aceita", title: "Alteração não permitida",
                statusCode: StatusCodes.Status409Conflict);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut("{id:guid}/Finalizar")]
        public async Task<ActionResult> Finalizar(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);
            if (!authorizationResult.Succeeded)
                return Forbid();

            if (proposta.Participacao == StatusParticipacao.Aceito)
            {
                await Service.FinalizarProposta(proposta);
                return Ok();
            }

            return Problem("A participação nesse projeto foi recusada!");
        }

        #endregion
    }
}