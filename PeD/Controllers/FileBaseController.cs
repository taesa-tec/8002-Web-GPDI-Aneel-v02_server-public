using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MimeDetective;
using PeD.Core.Exceptions;
using PeD.Core.Models;
using PeD.Data;

namespace PeD.Controllers
{
    public abstract class FileBaseController<T> : Controller where T : class, IFileUpload, new()
    {
        protected GestorDbContext context;
        protected IConfiguration Configuration;
        protected readonly string StoragePath;
        private ContentInspector _contentInspector;
        public string[] AllowedFiles { get; set; }

        protected string ActualPath
        {
            get
            {
                var folderName = string.Format("{0}/{1}/{2}", DateTime.Today.Year, DateTime.Today.Month,
                    DateTime.Today.Day);
                return Path.Combine(StoragePath, folderName);
            }
        }

        public FileBaseController(GestorDbContext context, IConfiguration configuration,
            ContentInspector contentInspector)
        {
            this.context = context;
            Configuration = configuration;
            _contentInspector = contentInspector;
            StoragePath = Configuration.GetValue<string>("StoragePath");
            AllowedFiles = Configuration.GetSection("AllowedExtensionFiles").Get<string[]>();
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
            var files = Request.Form.Files.ToList();

            if (!files.All(file => AllowedFiles.Any(ext => file.FileName.EndsWith($".{ext}"))))
            {
                throw new FileNotAllowedException();
            }

            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var result = _contentInspector.Inspect(ContentReader.Default.ReadFromStream(stream));
                var mimeType = result.ByMimeType();
                if (mimeType.IsEmpty)
                {
                    throw new FileNotAllowedException();
                }
            }


            if (!files.All(file => AllowedFiles.Any(ext => file.FileName.EndsWith($".{ext}"))))
            {
                throw new FileNotAllowedException();
            }

            // long size = files.Sum(f => f.Length);

            var fileUploads = new List<T>();

            var start = DateTime.Now.Ticks.ToString();

            uint o = 0;

            CreateActualPath();

            foreach (var file in files)
            {
                var filename = Path.Combine(ActualPath, string.Format("{0}-{1}", start, o));


                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        var _t = FromFormFile(file, filename);
                        if (func != null)
                        {
                            _t = func(_t);
                            _t.UserId = this.UserId();
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