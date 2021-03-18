using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta Fornecedor")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class ArquivosController : FileBaseController<FileUpload>
    {
        private PropostaService _propostaService;

        public ArquivosController(GestorDbContext context, IConfiguration configuration,
            PropostaService propostaService) : base(context, configuration)
        {
            _propostaService = propostaService;
        }

        [HttpGet]
        public ActionResult<List<FileUpload>> GetFiles([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta == null)
                return NotFound();

            return context
                .Set<PropostaArquivo>()
                .Include(p => p.Arquivo)
                .Where(p => p.PropostaId == proposta.Id)
                .Select(p => p.Arquivo)
                .ToList();
        }

        [HttpGet("{arquivoId}")]
        public ActionResult GetFile([FromRoute] int captacaoId, [FromRoute] int arquivoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta == null)
                return NotFound();

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

        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<FileUpload>>> Upload([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta == null)
                return NotFound();
            var files = await base.Upload();
            var propostaFiles = files.Select(f => new PropostaArquivo() {ArquivoId = f.Id, PropostaId = proposta.Id});
            context.Set<PropostaArquivo>().AddRange(propostaFiles);
            context.SaveChanges();
            return files;
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveFile([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta == null)
                return NotFound();

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
                    file.Arquivo.Name,
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
                Name = file.FileName,
                ContentType = file.ContentType,
                Path = filename,
                Size = file.Length,
                UserId = this.UserId(),
                CreatedAt = DateTime.Now
            };
        }
    }
}