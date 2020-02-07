using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using APIGestor.Models.Demandas;
using APIGestor.Security;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.FileProviders;
using APIGestor.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace APIGestor.Controllers
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    public class DemandaFileController : FileBaseController<DemandaFile>
    {
        public DemandaFileController(GestorDbContext context, IHostingEnvironment hostingEnvironment) : base(context,
            hostingEnvironment)
        {
        }

        [HttpDelete("{id}/Files/{fileId}")]
        public IActionResult RemoveFile(int id, int fileId)
        {
            var file = context.DemandaFiles.FirstOrDefault(df => df.DemandaId == id && df.Id == fileId);
            if (file == null) return NotFound();

            context.DemandaFiles.Remove(file);
            System.IO.File.Delete(file.Path);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}/Files")]
        public ActionResult<List<DemandaFile>> GetFiles(int id)
        {
            return context
                .DemandaFiles
                .Where(file => file.DemandaId == id)
                .ToList();
        }

        [HttpPost("{id}/Files")]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<DemandaFile>>> Upload(int id)
        {
            return await base.Upload(file =>
            {
                file.DemandaId = id;
                return file;
            });
        }

        protected override DemandaFile FromFormFile(IFormFile file, string filename)
        {
            return new DemandaFile()
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