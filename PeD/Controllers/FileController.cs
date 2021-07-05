using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PeD.Core.Models;
using PeD.Data;

namespace PeD.Controllers
{
    [Route("api/File/")]
    [ApiController]
    [Authorize("Bearer")]
    public class FileController : FileBaseController<FileUpload>
    {
        public FileController(GestorDbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }


        [HttpGet]
        public ActionResult<List<FileUpload>> GetFiles()
        {
            return context
                .Files
                .Where(file => file.UserId == this.UserId())
                .ToList();
        }

        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<FileUpload>>> Upload()
        {
            return await base.Upload();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var file = context
                .Files
                .FirstOrDefault(f => f.UserId == this.UserId() && f.Id == id);
            if (file is null)
            {
                return NotFound();
            }

            context.Remove(file);
            context.SaveChanges();
            System.IO.File.Delete(file.Path);
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