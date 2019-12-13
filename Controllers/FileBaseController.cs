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
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Controllers
{
    public abstract class FileBaseController<T> : Controller where T : FileUpload, new()
    {
        protected GestorDbContext context;
        protected IHostingEnvironment hostingEnvironment;

        protected string ActualPath
        {
            get
            {
                string folderName = String.Format("uploads/{0}/{1}/{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                string webRootPath = hostingEnvironment.WebRootPath;
                return Path.Combine(webRootPath, folderName);
            }
        }
        public FileBaseController(GestorDbContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        protected void CreateActualPath()
        {
            if (!Directory.Exists(ActualPath))
            {
                Directory.CreateDirectory(ActualPath);
            }
        }


        public virtual async Task<ActionResult<List<T>>> Upload(Func<T, T> func = null)
        {
            List<IFormFile> files = Request.Form.Files.ToList();

            long size = files.Sum(f => f.Length);

            var fileUploads = new List<T>();

            var start = DateTime.Now.Ticks.ToString();

            uint o = 0;

            CreateActualPath();

            foreach (var file in files)
            {
                var filename = Path.Combine(ActualPath, String.Format("{0}-{1}", start, o));


                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        var _t = FromFormFile(file, filename);
                        if (func != null)
                        {
                            _t = func(_t);
                        }
                        fileUploads.Add(_t);

                        o++;
                    }
                }
            }
            context.AddRange(fileUploads);
            // context.Files.AddRange(fileUploads);
            await context.SaveChangesAsync();
            return fileUploads;
        }
        protected abstract T FromFormFile(IFormFile file, string filename);


    }
}