using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeD.Authorizations;
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
    [SwaggerTag("Proposta Fornecedor")]
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

        [HttpGet("")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            IEnumerable<Proposta> propostas;
            if (User.IsInRole(Roles.Administrador) || User.IsInRole(Roles.User))
            {
                propostas = Service.Get();
            }
            else
            {
                propostas = Service.GetPropostasPorResponsavel(this.UserId());
            }

            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("Encerradas")]
        public ActionResult<List<PropostaDto>> GetPropostasEncerradas()
        {
            var propostas = Service.GetPropostasEncerradarFornecedor(this.UserEmpresaId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PropostaDto>> GetProposta(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);

            if (authorizationResult.Succeeded)
            {
                return Mapper.Map<PropostaDto>(proposta);
            }
            return Unauthorized();
        }


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

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut("{id:guid}/Rejeitar")]
        public async Task<ActionResult> RejeitarCaptacao(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var result = await this.Authorize(proposta);
            if (result.Succeeded)
            {
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

            return Forbid();
        }

        [HttpPut("{id:guid}/Participar")]
        public ActionResult ParticiparCaptacao(Guid id)
        {
            var proposta = Service.GetProposta(id);
            if (proposta.Participacao == StatusParticipacao.Pendente)
            {
                proposta.Participacao = StatusParticipacao.Aceito;
                proposta.DataParticipacao = DateTime.Now;
                Service.Put(proposta);
                return Ok();
            }

            return StatusCode(428);
        }


        [HttpPut("{id:guid}/Duracao")]
        public ActionResult PropostaDuracao(int id, [FromServices] IService<Etapa> etapaService, [FromBody] short meses)
        {
            var proposta = Service.GetProposta(id);
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

            return StatusCode(428);
        }

        [HttpGet("{id:guid}/Documento")]
        public ActionResult PropostaDoc(Guid id)
        {
            var tempproposta = Service.GetProposta(id);
            if (tempproposta == null)
            {
                return NotFound();
            }

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

        [HttpGet("{id:guid}/Download/PlanoTrabalho")]
        public ActionResult PropostaDocDownload(int id)
        {
            var tempproposta = Service.GetPropostaPorResponsavel(id, this.UserId(), Captacao.CaptacaoStatus.Fornecedor,
                Captacao.CaptacaoStatus.Encerrada);
            if (tempproposta == null)
            {
                return NotFound();
            }

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
            if (tempproposta == null || (tempproposta.Captacao?.Status != Captacao.CaptacaoStatus.Encerrada &&
                                         tempproposta.Captacao?.Status != Captacao.CaptacaoStatus.Encerrada))
            {
                return NotFound();
            }

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

        [HttpPut("{id:guid}/Finalizar")]
        public async Task<ActionResult> Finalizar(Guid id)
        {
            var proposta = Service.GetProposta(id);
            var result = await this.Authorize(proposta);
            if (result.Succeeded)
            {
                if (proposta.Participacao == StatusParticipacao.Aceito)
                {
                    await Service.FinalizarProposta(proposta);
                    return Ok();
                }

                return Problem("A participação nesse projeto foi recusada!");
            }

            return Forbid();
        }
    }
}