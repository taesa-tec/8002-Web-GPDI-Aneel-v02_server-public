using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
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
    [Route("api/File/")]
    [ApiController]
    [Authorize("Bearer")]
    public class FileController : Controller
    {
        GestorDbContext context;
        IHostingEnvironment hostingEnvironment;

        protected string ActualPath
        {
            get
            {
                string folderName = String.Format("uploads/{0}/{1}/{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                string webRootPath = hostingEnvironment.WebRootPath;
                return Path.Combine(webRootPath, folderName);
            }
        }
        public FileController(GestorDbContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        private void CreateActualPath()
        {
            if (!Directory.Exists(ActualPath))
            {
                Directory.CreateDirectory(ActualPath);
            }
        }

        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<FileUpload>>> Upload()
        {
            List<IFormFile> files = Request.Form.Files.ToList();
            long size = files.Sum(f => f.Length);

            var fileUploads = new List<FileUpload>();
            var start = DateTime.Now.Ticks.ToString();
            uint o = 0;
            foreach (var file in files)
            {
                var filename = Path.Combine(ActualPath, String.Format("{0}-{1}", start, o));
                CreateActualPath();
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        fileUploads.Add(new FileUpload()
                        {
                            FileName = file.FileName,
                            Name = file.FileName,
                            ContentType = file.ContentType,
                            Path = filename,
                            Size = file.Length,
                            UserId = this.userId(),
                            CreatedAt = DateTime.Now
                        });
                        o++;
                    }
                }
            }
            context.Files.AddRange(fileUploads);
            await context.SaveChangesAsync();
            return fileUploads;
        }

        [HttpGet]
        public ActionResult<List<FileUpload>> GetFiles()
        {
            return context.Files.Where(file => file.UserId == this.userId()).ToList();
        }
    }
}