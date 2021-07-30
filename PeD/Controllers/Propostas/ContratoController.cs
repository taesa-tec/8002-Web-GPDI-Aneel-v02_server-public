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
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Extensions;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class ContratoController : PropostaNodeBaseController<PropostaContrato>
    {
        private GestorDbContext _context;

        public ContratoController(IService<PropostaContrato> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper,
                authorizationService, propostaService)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<ActionResult<PropostaContratoDto>> Get([FromRoute] Guid propostaId)
        {
            if (!await HasAccess())
                return Forbid();
            var contrato = PropostaService.GetContrato(propostaId);
            contrato = PropostaService.GetContratoFull(contrato.PropostaId);
            if (Proposta.ResponsavelId != this.UserId())
            {
                contrato.Rascunho = string.Empty;
            }

            return Ok(Mapper.Map<PropostaContratoDto>(contrato));
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost("")]
        public async Task<ActionResult> Post([FromRoute] Guid propostaId,
            [FromBody] ContratoRequest request,
            [FromServices] IService<ContratoComentario> service)
        {
            if (!await HasAccess(true))
                return Forbid();

            if (Proposta.Captacao.Status == Captacao.CaptacaoStatus.Refinamento &&
                (Proposta.ContratoAprovacao != StatusAprovacao.Alteracao ||
                 string.IsNullOrWhiteSpace(request.Alteracao) && !request.Draft))
            {
                return BadRequest();
            }

            ContratoComentario comentario = null;

            var contratoProposta = PropostaService.GetContrato(propostaId);
            var hash = contratoProposta.Conteudo?.ToMD5() ?? "";
            var hasChanges = !hash.Equals(request.Conteudo.ToMD5());

            contratoProposta.Finalizado = contratoProposta.Finalizado || !request.Draft;
            if (request.Draft)
            {
                contratoProposta.Rascunho = request.Conteudo;
            }
            else
            {
                contratoProposta.Rascunho = null;
                contratoProposta.Conteudo = request.Conteudo;
            }

            if (contratoProposta.Id != 0)
            {
                Service.Put(contratoProposta);
            }
            else
            {
                var parent = contratoProposta.Parent;
                contratoProposta.Parent = null;
                Service.Post(contratoProposta);
                contratoProposta.Parent = parent;
            }

            // Só criar revisão se tiver alguma alteração de texto relevante
            if (hasChanges && !request.Draft)
            {
                var revisao = contratoProposta.ToRevisao();
                revisao.UserId = this.UserId();
                _context.Add(revisao);
                _context.SaveChanges();
            }

            if (!request.Draft && (hasChanges || contratoProposta.FileId == null))
            {
                var file = PropostaService.SaveContratoPdf(contratoProposta);
                contratoProposta.File = file;
                contratoProposta.FileId = file.Id;
                Service.Put(contratoProposta);
            }

            if (Proposta.Captacao.Status == Captacao.CaptacaoStatus.Refinamento && !request.Draft)
            {
                Proposta.ContratoAprovacao = StatusAprovacao.Pendente;
                comentario = new ContratoComentario()
                {
                    AuthorId = this.UserId(),
                    PropostaId = Proposta.Id,
                    Mensagem = request.Alteracao,
                    CreatedAt = DateTime.Now
                };
                service.Post(comentario);
                PropostaService.Put(Proposta);
                PropostaService.SendEmailNovoContrato(Proposta).Wait();
            }

            PropostaService.UpdatePropostaDataAlteracao(contratoProposta.PropostaId);

            return Ok(new {Id = contratoProposta.Id, Comentario = Mapper.Map<ComentarioDto>(comentario)});
        }

        [HttpGet("Revisoes")]
        public async Task<ActionResult<List<ContratoRevisaoListItemDto>>> GetRevisoes()
        {
            if (!await HasAccess())
                return Forbid();
            var revisoes = PropostaService.GetContratoRevisoes(Proposta.Id);
            return Mapper.Map<List<ContratoRevisaoListItemDto>>(revisoes);
        }

        [HttpGet("Revisoes/{id}")]
        public async Task<ActionResult<ContratoRevisaoDto>> GetRevisao([FromRoute] int id)
        {
            if (!await HasAccess())
                return Forbid();
            var revisao = PropostaService.GetContratoRevisao(Proposta.Id, id);
            return Mapper.Map<ContratoRevisaoDto>(revisao);
        }

        [HttpGet("Revisoes/{id}/Diff")]
        public async Task<ActionResult<string>> GetRevisaoDiff([FromRoute] Guid propostaId, [FromRoute] int id,
            [FromServices] IViewRenderService viewRenderService)
        {
            if (!await HasAccess())
                return Forbid();
            var contrato = PropostaService.GetContrato(propostaId);
            if (contrato is null)
                return NotFound();
            var revisao = PropostaService.GetContratoRevisao(contrato.PropostaId, id);

            var diff = DiffService.Html(contrato.Conteudo,
                revisao.Conteudo);
            var render = await viewRenderService.RenderToStringAsync("Pdf/Diff", diff);
            return render;
        }


        [HttpGet("Comentarios")]
        public async Task<ActionResult> Comentarios([FromServices] IService<ContratoComentario> service)
        {
            if (!await HasAccess())
                return Forbid();

            var comentarios = service.Filter(q => q
                .Include(c => c.Author)
                .Include(c => c.Files)
                .ThenInclude(f => f.File)
                .Where(c => c.PropostaId == Proposta.Id)
                .OrderByDescending(c => c.CreatedAt));
            return Ok(Mapper.Map<List<ComentarioDto>>(comentarios));
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("SolicitarAlteracao")]
        public async Task<ActionResult> SolicitarAlteracao(ComentarioRequest request,
            [FromServices] IService<ContratoComentario> service)
        {
            if (!await HasAccess())
                return Forbid();
            Proposta.ContratoAprovacao = StatusAprovacao.Alteracao;
            var comentario = new ContratoComentario()
            {
                AuthorId = this.UserId(),
                PropostaId = Proposta.Id,
                Mensagem = request.Mensagem,
                CreatedAt = DateTime.Now
            };
            service.Post(comentario);
            PropostaService.Put(Proposta);
            await PropostaService.SendEmailContratoAlteracao(Proposta);
            return Ok(Mapper.Map<ComentarioDto>(comentario));
        }

        [HttpPost("Comentario/{id}/Arquivo")]
        public async Task<ActionResult> AnexoComentario(int id, List<IFormFile> file,
            [FromServices] ArquivoService arquivoService)
        {
            var arquivos = await arquivoService.SaveFiles(file);
            var entities = arquivos.ToList().Select(a => new ContratoComentarioFile()
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
            var file = _context.Set<ContratoComentarioFile>()
                .Include(c => c.File)
                .Include(c => c.Comentario)
                .FirstOrDefault(c =>
                    c.ComentarioId == id && c.FileId == fileId && c.Comentario.PropostaId == Proposta.Id);
            if (file is null)
                return NotFound();

            return PhysicalFile(file.File.Path, file.File.ContentType, file.File.FileName);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("Aprovar")]
        public async Task<ActionResult> Aprovar()
        {
            if (!await HasAccess() || Proposta.ContratoAprovacao == StatusAprovacao.Aprovado)
                return Forbid();

            Proposta.ContratoAprovacao = StatusAprovacao.Aprovado;
            PropostaService.Put(Proposta);
            await PropostaService.ConcluirRefinamento(Proposta);

            return Ok();
        }
    }
}