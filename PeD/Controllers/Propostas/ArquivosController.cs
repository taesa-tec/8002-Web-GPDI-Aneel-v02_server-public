using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeD.Authorizations;
using PeD.Core.ApiModels;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta Fornecedor")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class ArquivosController : FileBaseController<FileUpload>
    {
        private IMapper _mapper;
        private PropostaService _propostaService;
        private IAuthorizationService authorizationService;

        public ArquivosController(GestorDbContext context, IConfiguration configuration,
            PropostaService propostaService, IAuthorizationService authorizationService, IMapper mapper) : base(context,
            configuration)
        {
            _propostaService = propostaService;
            this.authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<FileUploadDto>>> GetFiles([FromRoute] Guid propostaId)
        {
            var proposta = _propostaService.GetProposta(propostaId);
            if (proposta == null)
                return NotFound();
            var authResult = await authorizationService.AuthorizeAsync(User, proposta, PropostaAuthorizations.Access);
            if (!authResult.Succeeded)
                return Forbid();

            var files = context
                .Set<PropostaArquivo>()
                .Include(p => p.Arquivo)
                .Where(p => p.PropostaId == proposta.Id)
                .Select(p => p.Arquivo)
                .ToList();
            return _mapper.Map<List<FileUploadDto>>(files);
        }

        [AllowAnonymous]
        [HttpGet("{arquivoId:int}")]
        public async Task<ActionResult> GetFile([FromRoute] Guid propostaId, [FromRoute] int arquivoId)
        {
            var proposta = _propostaService.GetProposta(propostaId);
            if (proposta == null)
                return NotFound();
            var authResult = await authorizationService.AuthorizeAsync(User, proposta, PropostaAuthorizations.Access);
            if (!authResult.Succeeded)
                return Forbid();

            var arquivo = context
                .Set<PropostaArquivo>()
                .Include(p => p.Arquivo)
                .Where(p => p.PropostaId == proposta.Id && p.ArquivoId == arquivoId)
                .Select(p => p.Arquivo)
                .FirstOrDefault();
            if (arquivo == null)
                return NotFound();
            return PhysicalFile(arquivo.Path, arquivo.ContentType, arquivo.FileName);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<FileUploadDto>>> Upload([FromRoute] Guid propostaId)
        {
            var proposta = _propostaService.GetProposta(propostaId);
            if (proposta == null)
                return NotFound();
            var authResult = await authorizationService.AuthorizeAsync(User, proposta, PropostaAuthorizations.Access);
            if (!authResult.Succeeded)
                return Forbid();

            var files = await base.Upload();
            var propostaFiles = files.Select(f => new PropostaArquivo() { ArquivoId = f.Id, PropostaId = proposta.Id });
            context.Set<PropostaArquivo>().AddRange(propostaFiles);
            context.SaveChanges();
            return _mapper.Map<List<FileUploadDto>>(files);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFile([FromRoute] Guid propostaId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetProposta(propostaId);
            if (proposta == null)
                return NotFound();
            var authResult = await authorizationService.AuthorizeAsync(User, proposta, PropostaAuthorizations.Access);
            if (!authResult.Succeeded)
                return Forbid();

            var propostaFiles = context.Set<PropostaArquivo>();
            var file = propostaFiles
                .Include(pa => pa.Arquivo)
                .FirstOrDefault(pa => pa.PropostaId == proposta.Id && pa.ArquivoId == id);
            if (file == null) return NotFound();
            try
            {
                propostaFiles.Remove(file);
                context.Remove(file.Arquivo);
                context.SaveChanges();
                System.IO.File.Delete(file.Arquivo.Path);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    e.Message,
                    e.StackTrace,
                    file.Arquivo.FileName,
                    file.Arquivo.Path
                });
            }

            return Ok();
        }

        protected override FileUpload FromFormFile(IFormFile file, string filename)
        {
            return new FileUpload()
            {
                FileName = file.FileName,
                Name = file.Name,
                ContentType = file.ContentType,
                Path = filename,
                Size = file.Length,
                UserId = this.UserId(),
                CreatedAt = DateTime.Now
            };
        }
    }
}