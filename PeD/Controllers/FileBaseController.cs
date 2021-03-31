using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PeD.Core.Models;
using PeD.Data;

namespace PeD.Controllers
{
    public abstract class FileBaseController<T> : Controller where T : class, IFileUpload, new()
    {
        protected GestorDbContext context;
        protected IConfiguration Configuration;
        protected readonly string StoragePath;

        protected string ActualPath
        {
            get
            {
                string folderName = String.Format("{0}/{1}/{2}", DateTime.Today.Year, DateTime.Today.Month,
                    DateTime.Today.Day);
                return Path.Combine(StoragePath, folderName);
            }
        }

        public FileBaseController(GestorDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
            StoragePath = Configuration.GetValue<string>("StoragePath");
        }

        protected void CreateActualPath()
        {
            if (!Directory.Exists(ActualPath))
            {
                Console.WriteLine(StoragePath);
                Directory.CreateDirectory(ActualPath);
            }
        }

        [NonAction]
        public virtual async Task<List<T>> Upload(Func<T, T> func = null)
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

            context.Set<T>().AddRange(fileUploads);
            // context.Files.AddRange(fileUploads);
            await context.SaveChangesAsync();
            return fileUploads;
        }

        protected abstract T FromFormFile(IFormFile file, string filename);
    }
}