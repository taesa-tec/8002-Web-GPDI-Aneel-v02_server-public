using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using PeD.Data;
using PeD.Models;

namespace PeD.Controllers
{
    [Route("api/File/")]
    [ApiController]
    [Authorize("Bearer")]
    public class FileController : FileBaseController<FileUpload>
    {
        public FileController(GestorDbContext context, IWebHostEnvironment hostingEnvironment) : base(context, hostingEnvironment)
        {
        }


        [HttpGet]
        public ActionResult<List<FileUpload>> GetFiles()
        {

            return context
            .Files
            .Where(file => file.UserId == this.userId())
            .ToList();
        }

        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<FileUpload>>> Upload()
        {
            return await base.Upload();
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
                UserId = this.userId(),
                CreatedAt = DateTime.Now
            };
        }
    }
}