using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGestor.Data;
using APIGestor.Models.Captacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIGestor.Controllers.Captacoes
{
    [SwaggerTag("Captacao")]
    [Route("api/Captacoes")]
    [ApiController]
    [Authorize("Bearer")]
    public class CaptacaoArquivoController : FileBaseController<CaptacaoArquivo>
    {
        public CaptacaoArquivoController(GestorDbContext context, IWebHostEnvironment hostingEnvironment) : base(
            context, hostingEnvironment)
        {
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
                    file.Name,
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
                Name = file.FileName,
                ContentType = file.ContentType,
                Path = filename,
                Size = file.Length,
                UserId = this.userId(),
                CreatedAt = DateTime.Now
            };
        }
    }
}