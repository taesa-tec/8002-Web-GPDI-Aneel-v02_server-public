using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Authorizations;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using Empresa = PeD.Core.Models.Propostas.Empresa;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Propostas 2.4")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas")]
    public class PropostasController : ControllerServiceBase<Proposta>
    {
        private new PropostaService Service;
        private GestorDbContext _context;
        public readonly IAuthorizationService AuthorizationService;

        public PropostasController(PropostaService service, IMapper mapper, IAuthorizationService authorizationService,
            GestorDbContext context)
            : base(service, mapper)
        {
            Service = service;
            AuthorizationService = authorizationService;
            _context = context;
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

            return NotFound();
        }

        #endregion

        #region Empresas Relacionadas a proposta

        [HttpGet("{id:guid}/EmpresasOld")]
        public async Task<ActionResult> GetPropostaEmpresas(Guid id,
            [FromServices] IService<Empresa> empresasService,
            [FromServices] IService<Fornecedor> fornecedorService
        )
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);

            if (authorizationResult.Succeeded)
            {
                var empresas = empresasService.Filter(q =>
                    q.Where(e => e.PropostaId == proposta.Id));
                return Ok(Mapper.Map<List<EmpresaDto>>(empresas));
            }

            return Forbid();
        }

        #endregion


        [HttpGet("{id:guid}/Documento")]
        public async Task<ActionResult> PropostaDoc(Guid id, [FromServices] GestorDbContext context)
        {
            var tempproposta = Service.GetProposta(id);
            if (tempproposta == null)
            {
                return NotFound();
            }

            context.Entry(tempproposta).State = EntityState.Detached;

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
        public async Task<ActionResult> PropostaDuracao(Guid id,
            [FromServices] IService<Etapa> etapaService,
            [FromServices] IService<Meta> metaService,
            [FromBody] short meses)
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);
            if (!authorizationResult.Succeeded)
                return Forbid();

            if (proposta.Participacao == StatusParticipacao.Aceito ||
                proposta.Participacao == StatusParticipacao.Concluido)
            {
                var metas = metaService.Filter(q => q.Where(e => e.PropostaId == proposta.Id));
                if (metas.Any() && meses < metas.Max(m => m.Meses))
                    return Problem("Há metas em meses superiores à duração desejada",
                        title: "Alteração não permitida", statusCode: StatusCodes.Status428PreconditionRequired);

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
        public async Task<ActionResult> Finalizar(Guid id, [FromBody] ComentarioRequest request,
            [FromServices] IService<PlanoComentario> service)
        {
            var proposta = Service.GetProposta(id);
            var authorizationResult = await this.Authorize(proposta);
            if (!authorizationResult.Succeeded)
                return Forbid();

            if (proposta.Participacao == StatusParticipacao.Aceito ||
                proposta.Participacao == StatusParticipacao.Concluido)
            {
                PlanoComentario comentario = null;
                await Service.FinalizarProposta(proposta);
                if (proposta.Captacao.Status == Captacao.CaptacaoStatus.Refinamento && request?.Mensagem != null &&
                    proposta.PlanoTrabalhoAprovacao == StatusAprovacao.Alteracao)
                {
                    proposta.PlanoTrabalhoAprovacao = StatusAprovacao.Pendente;
                    comentario = new PlanoComentario()
                    {
                        AuthorId = this.UserId(),
                        PropostaId = proposta.Id,
                        Mensagem = request.Mensagem,
                        CreatedAt = DateTime.Now
                    };
                    service.Post(comentario);
                    Service.Put(proposta);
                    Service.SendEmailNovoPlano(proposta).Wait();
                }


                return Ok(Mapper.Map<ComentarioDto>(comentario));
            }

            return Problem("A participação nesse projeto foi recusada!");
        }

        #endregion

        protected async Task<bool> HasAccess(Proposta proposta)
        {
            var authorizationResult = await this.Authorize(proposta);
            return authorizationResult.Succeeded;
        }

        [HttpGet("{guid:guid}/Comentarios")]
        public async Task<ActionResult> Comentarios(Guid guid, [FromServices] IService<PlanoComentario> service)
        {
            var proposta = Service.GetProposta(guid);
            if (!await HasAccess(proposta))
                return Forbid();

            var comentarios = service.Filter(q => q
                .Include(c => c.Author)
                .Include(c => c.Files)
                .ThenInclude(f => f.File)
                .Where(c => c.PropostaId == proposta.Id)
                .OrderByDescending(c => c.CreatedAt));
            return Ok(Mapper.Map<List<ComentarioDto>>(comentarios));
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{guid:guid}/SolicitarAlteracao")]
        public async Task<ActionResult> SolicitarAlteracao(Guid guid, ComentarioRequest request,
            [FromServices] IService<PlanoComentario> service)
        {
            var proposta = Service.GetProposta(guid);
            if (!await HasAccess(proposta) || proposta.Captacao.Status != Captacao.CaptacaoStatus.Refinamento)
                return Forbid();
            if (string.IsNullOrWhiteSpace(request.Mensagem))
                return BadRequest();
            proposta.PlanoTrabalhoAprovacao = StatusAprovacao.Alteracao;
            var comentario = new PlanoComentario()
            {
                AuthorId = this.UserId(),
                PropostaId = proposta.Id,
                Mensagem = request.Mensagem,
                CreatedAt = DateTime.Now
            };
            service.Post(comentario);
            Service.Put(proposta);
            await Service.SendEmailPlanoTrabalhoAlteracao(proposta);
            return Ok(Mapper.Map<ComentarioDto>(comentario));
        }

        [HttpPost("Comentario/{id}/Arquivo")]
        public async Task<ActionResult> AnexoComentario(int id, List<IFormFile> file,
            [FromServices] ArquivoService arquivoService)
        {
            var arquivos = await arquivoService.SaveFiles(file, this.UserId());
            var entities = arquivos.ToList().Select(a => new PlanoComentarioFile()
            {
                ComentarioId = id,
                FileId = a.Id
            });
            _context.AddRange(entities);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Comentario/{id}/Arquivo/{fileId}")]
        public ActionResult DownloadFile(int id, int fileId)
        {
            var file = _context.Set<PlanoComentarioFile>()
                .Include(c => c.File)
                .Include(c => c.Comentario)
                .FirstOrDefault(c =>
                    c.ComentarioId == id && c.FileId == fileId);
            if (file is null)
                return NotFound();

            return PhysicalFile(file.File.Path, file.File.ContentType, file.File.FileName);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{guid:guid}/Aprovar")]
        public async Task<ActionResult> Aprovar(Guid guid)
        {
            var proposta = Service.GetProposta(guid);
            if (!await HasAccess(proposta) || proposta.Captacao.Status != Captacao.CaptacaoStatus.Refinamento ||
                proposta.PlanoTrabalhoAprovacao == StatusAprovacao.Aprovado)
                return Forbid();

            proposta.PlanoTrabalhoAprovacao = StatusAprovacao.Aprovado;
            Service.Put(proposta);

            await Service.ConcluirRefinamento(proposta);
            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{guid:guid}/CancelarRefinamento")]
        public async Task<ActionResult> CancelarRefinamento(Guid guid)
        {
            var proposta = Service.GetProposta(guid);
            if (!await HasAccess(proposta) || proposta.Captacao.Status != Captacao.CaptacaoStatus.Refinamento)
                return Forbid();
            proposta.Participacao = StatusParticipacao.Cancelado;
            Service.Put(proposta);
            await Service.SendEmailRefinamentoCancelado(proposta);
            return Ok();
        }
    }
}