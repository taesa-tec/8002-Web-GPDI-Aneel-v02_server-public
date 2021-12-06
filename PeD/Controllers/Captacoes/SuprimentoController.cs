using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeD.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeD.Authorizations;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Exceptions.Captacoes;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Captacao;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;

namespace PeD.Controllers.Captacoes
{
    [SwaggerTag("Captacao")]
    [Route("api/Captacoes/Suprimento")]
    [ApiController]
    [Authorize("Bearer")]
    [Authorize(Policy = Policies.IsSuprimento)]
    public class SuprimentoController : ControllerServiceBase<Captacao>
    {
        private IUrlHelper _urlHelper;
        private CaptacaoService service;
        private ILogger<SuprimentoController> _logger;

        public SuprimentoController(CaptacaoService service, IMapper mapper, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor, GestorDbContext context, ILogger<SuprimentoController> logger)
            : base(service, mapper)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            this.service = service;
            _logger = logger;
        }

        [HttpGet("")]
        public ActionResult GetCaptacoes()
        {
            var captacoes = service.GetCaptacoesPorSuprimento(this.UserId());
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("Pendentes")]
        public ActionResult GetCaptacoesPendentes()
        {
            var captacoes = service.GetCaptacoesPorSuprimento(this.UserId(), Captacao.CaptacaoStatus.Elaboracao);
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("Abertas")]
        public ActionResult GetCaptacoesEmElaboracao()
        {
            var captacoes = service.GetCaptacoesPorSuprimentoAberta(this.UserId());
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("Finalizadas")]
        public ActionResult GetCaptacoesFinalizadas()
        {
            var captacoes = service.GetCaptacoesPorSuprimentoFinalizada(this.UserId());
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("Canceladas")]
        public ActionResult GetCaptacoesCanceladas()
        {
            var captacoes = service.GetCaptacoesPorSuprimentoCanceladas(this.UserId());
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("{id:int}")]
        public ActionResult<CaptacaoDetalhesDto> GetCaptacao(int id)
        {
            var captacao = Service.Filter(q => q
                .Include(c => c.Arquivos)
                .Include(c => c.FornecedoresSugeridos)
                .ThenInclude(fs => fs.Fornecedor)
                .Include(c => c.FornecedoresConvidados)
                .ThenInclude(fs => fs.Fornecedor)
                .Where(c => c.Status != Captacao.CaptacaoStatus.Pendente &&
                            c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();
            if (captacao == null)
            {
                return NotFound();
            }

            var detalhes = Mapper.Map<CaptacaoDetalhesDto>(captacao);
            detalhes.EspecificacaoTecnicaUrl = _urlHelper.Link("DemandaPdf",
                new { id = captacao.DemandaId, form = "especificacao-tecnica" });

            return Ok(detalhes);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> ConfigurarCaptacao(int id, ConfiguracaoRequest request)
        {
            try
            {
                if (service.UserSuprimento(id) == this.UserId())
                {
                    await service.ConfigurarCaptacao(id, request.Termino, request.Consideracoes, request.Arquivos,
                        request.Fornecedores, request.ContratoId);
                    await service.EnviarParaFornecedores(id);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (CaptacaoException e)
            {
                // CaptacaoException não exibe mensagem do funcionamento interno
                return Problem(e.Message, statusCode: StatusCodes.Status409Conflict);
            }
            catch (Exception)
            {
                return Problem("Erro interno do servidor");
            }

            return Ok();
        }

        [HttpPut("{id:int}/Estender")]
        public async Task<ActionResult> EstenderCaptacao(int id, ConfiguracaoRequest request)
        {
            try
            {
                if (service.UserSuprimento(id) == this.UserId())
                {
                    await service.EstenderCaptacao(id, request.Termino);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (CaptacaoException e)
            {
                return Problem(e.Message, null, StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                _logger.LogError("Erro ao estender captação:{Error}", e.Message);
                return Problem("Erro interno do servidor");
            }

            return Ok();
        }

        [HttpDelete("{id:int}/Cancelar")]
        public async Task<ActionResult> CancelarCaptacao(int id)
        {
            try
            {
                if (service.UserSuprimento(id) == this.UserId())
                {
                    await service.CancelarCaptacao(id);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Problem(e.Message);
            }

            return Ok();
        }

        [HttpGet("{id:int}/Propostas")]
        public ActionResult<List<PropostaDto>> GetPropostas(int id)
        {
            if (service.UserSuprimento(id) == this.UserId())
            {
                var propostas = service.GetPropostasPorCaptacao(id);
                return Mapper.Map<List<PropostaDto>>(propostas);
            }

            return Forbid();
        }

        [HttpGet("{id:int}/Propostas/Status/{status}")]
        public ActionResult<List<PropostaDto>> GetPropostas(int id, StatusParticipacao status)
        {
            if (service.UserSuprimento(id) == this.UserId())
            {
                var propostas = service.GetPropostasPorCaptacao(id, status);
                return Mapper.Map<List<PropostaDto>>(propostas);
            }

            return Forbid();
        }

        [HttpGet("{id:int}/Propostas/EmAberto")]
        public ActionResult<List<PropostaDto>> GetPropostasEmAberto(int id)
        {
            if (service.UserSuprimento(id) == this.UserId())
            {
                var propostas = service.GetPropostasEmAberto(id);
                return Mapper.Map<List<PropostaDto>>(propostas);
            }

            return Forbid();
        }

        [HttpGet("{id:int}/Propostas/Recebidas")]
        public ActionResult<List<PropostaDto>> GetPropostasRecebidas(int id)
        {
            if (service.UserSuprimento(id) == this.UserId())
            {
                var propostas = service.GetPropostasRecebidas(id);
                return Mapper.Map<List<PropostaDto>>(propostas);
            }

            return Forbid();
        }

        [HttpGet("{id:int}/Propostas/Negadas")]
        public ActionResult<List<PropostaDto>> GetPropostasNegadas(int id)
        {
            if (service.UserSuprimento(id) == this.UserId())
            {
                var propostas = service.GetPropostasPorCaptacao(id, StatusParticipacao.Rejeitado);
                return Mapper.Map<List<PropostaDto>>(propostas);
            }

            return Forbid();
        }

        [HttpGet("{id:int}/Propostas/{propostaId:int}/Detalhes")]
        public ActionResult<PropostaDto> GetPropostaDetalhes(int id, int propostaId)
        {
            if (service.UserSuprimento(id) == this.UserId())
            {
                var captacao = service.Get(id);
                if (captacao is { IsPropostasOpen: true })
                {
                    var propostas = service.GetProposta(propostaId);
                    return Mapper.Map<PropostaDto>(propostas);
                }

                return NotFound();
            }

            return Forbid();
        }

        [HttpGet("{id:int}/Propostas/{propostaId:int}/PlanoTrabalho")]
        [HttpGet("{id:int}/Propostas/{propostaId:int}/PlanoTrabalho.pdf")]
        public ActionResult PropostaDocDownload(int id, int propostaId, [FromServices] PropostaService serviceProposta)
        {
            if (service.UserSuprimento(id) != this.UserId())
            {
                return Forbid();
            }

            var captacao = service.Get(id);
            if (captacao is { IsPropostasOpen: true })
            {
                var relatorio = serviceProposta.GetRelatorio(propostaId);
                if (relatorio != null)
                {
                    return PhysicalFile(relatorio.File.Path, relatorio.File.ContentType, relatorio.File.FileName);
                }
            }

            return NotFound();
        }

        [HttpGet("{id:int}/Propostas/{propostaId:int}/Contrato")]
        [HttpGet("{id:int}/Propostas/{propostaId:int}/Contrato.pdf")]
        public ActionResult PropostaContratoDownload(int id, int propostaId,
            [FromServices] PropostaService serviceProposta)
        {
            if (service.UserSuprimento(id) != this.UserId())
            {
                return Forbid();
            }

            var captacao = service.Get(id);
            if (captacao != null && captacao.Status >= Captacao.CaptacaoStatus.Encerrada &&
                captacao.Termino < DateTime.Now)
            {
                var contrato = serviceProposta.GetContratoPdf(propostaId);
                if (contrato != null)
                {
                    return PhysicalFile(contrato.Path, "application/octet-stream", contrato.FileName);
                }
            }

            return NotFound();
        }
    }
}