using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using iText.Html2pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta Fornecedor")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas")]
    public class PropostasController : ControllerServiceBase<Proposta>
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

        [HttpGet("Encerradas")]
        public ActionResult<List<PropostaDto>> GetPropostasEncerradas()
        {
            var propostas = Service.GetPropostasPorResponsavel(this.UserId(), Captacao.CaptacaoStatus.Encerrada);
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("{id}")]
        public ActionResult<PropostaDto> GetProposta(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            return Mapper.Map<PropostaDto>(proposta);
        }


        [HttpGet("{id}/Empresas")]
        public ActionResult GetPropostaEmpresas(int id,
            [FromServices] IService<CoExecutor> coexecutoresService,
            [FromServices] IService<Empresa> empresasService,
            [FromServices] IService<Fornecedor> fornecedorService
        )
        {
            var currentUser = this.UserId();
            var proposta = Service.GetPropostaPorResponsavel(id, currentUser);
            var empresa = empresasService.Filter(q => q.OrderBy(e => e.Id).Take(1)).FirstOrDefault();
            var fornecedor = fornecedorService.Filter(q =>
                q.Where(e => e.ResponsavelId == currentUser)).FirstOrDefault();
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

        [HttpPut("{id}/Rejeitar")]
        public ActionResult RejeitarCaptacao(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
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

        [HttpPut("{id}/Participar")]
        public ActionResult ParticiparCaptacao(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            if (proposta.Participacao == StatusParticipacao.Pendente)
            {
                proposta.Participacao = StatusParticipacao.Aceito;
                proposta.DataParticipacao = DateTime.Now;
                Service.Put(proposta);
                return Ok();
            }

            return StatusCode(428);
        }


        [HttpPut("{id}/Duracao")]
        public ActionResult PropostaDuracao(int id, [FromServices] IService<Etapa> etapaService, [FromBody] short meses)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
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

        [HttpGet("{id}/Documento")]
        public ActionResult PropostaDoc(int id)
        {
            var tempproposta = Service.GetPropostaPorResponsavel(id, this.UserId());
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

        [HttpGet("{id}/Download/PlanoTrabalho")]
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

        [HttpGet("{id}/Download/Contrato")]
        public ActionResult PropostaContratoDownload(int id)
        {
            var tempproposta = Service.GetPropostaPorResponsavel(id, this.UserId(), Captacao.CaptacaoStatus.Fornecedor,
                Captacao.CaptacaoStatus.Encerrada);
            if (tempproposta == null)
            {
                return NotFound();
            }

            var contrato = Service.GetContratoPdf(tempproposta.Id);
            if (contrato != null)
            {
                return PhysicalFile(contrato.Path, "application/octet-stream", contrato.FileName);
            }

            return NotFound();
        }

        [HttpPut("{id}/Finalizar")]
        public async Task<ActionResult> Finalizar(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            if (proposta.Participacao == StatusParticipacao.Aceito)
            {
                await Service.FinalizarProposta(proposta);
                return Ok();
            }

            return Problem("A participação nesse projeto foi recusada!");
        }
    }
}