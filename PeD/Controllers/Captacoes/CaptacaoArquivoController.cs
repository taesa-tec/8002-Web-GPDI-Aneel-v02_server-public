using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PeD.Core.Models.Captacoes;
using PeD.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Captacoes
{
    [SwaggerTag("Captacao")]
    [Route("api/Captacoes")]
    [ApiController]
    [Authorize("Bearer")]
    public class CaptacaoArquivoController : FileBaseController<CaptacaoArquivo>
    {
        public CaptacaoArquivoController(GestorDbContext context, IConfiguration configuration) : base(
            context, configuration)
        {
        }

        [HttpGet("{id}/Arquivos/{fileId}", Name = "DownloadCaptacaoFile")]
        public IActionResult DownloadFile(int id, int fileId)
        {
            var file = context.Set<CaptacaoArquivo>().FirstOrDefault(df => df.CaptacaoId == id && df.Id == fileId);
            if (file == null) return NotFound();
            if (System.IO.File.Exists(file.Path))
            {
                var response = PhysicalFile(file.Path, file.ContentType, file.FileName);
                if (Request.Query["dl"] == "1")
                {
                    response.FileDownloadName = file.FileName;
                }

                return response;
            }

            return NotFound();
        }

        [HttpDelete("{id}/Arquivos/{fileId}")]
        public IActionResult RemoveFile(int id, int fileId)
        {
            var file = context.Set<CaptacaoArquivo>().FirstOrDefault(df => df.CaptacaoId == id && df.Id == fileId);
            if (file == null) return NotFound();
            try
            {
                context.Set<CaptacaoArquivo>().Remove(file);
                context.SaveChanges();
                System.IO.File.Delete(file.Path);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    e.Message,
                    e.StackTrace,
                    file.FileName,
                    file.Path
                });
            }

            return Ok();
        }

        [HttpGet("{id}/Arquivos")]
        public ActionResult<List<CaptacaoArquivo>> GetFiles(int id)
        {
            return context
                .Set<CaptacaoArquivo>()
                .Where(file => file.CaptacaoId == id)
                .ToList();
        }

        [HttpPost("{id}/Arquivos")]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<CaptacaoArquivo>>> Upload(int id)
        {
            return await base.Upload(file =>
            {
                file.CaptacaoId = id;
                return file;
            });
        }


        protected override CaptacaoArquivo FromFormFile(IFormFile file, string filename)
        {
            return new CaptacaoArquivo()
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