using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PeD.Authorizations;
using PeD.Core.ApiModels.Demandas;
using PeD.Core.Exceptions;
using PeD.Core.Models.Demandas;
using PeD.Data;

namespace PeD.Controllers.Demandas
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    [Authorize(Policy = Policies.IsColaborador)]
    public class DemandaFileController : FileBaseController<DemandaFile>
    {
        private IMapper _mapper;
        private ILogger<DemandaFileController> _logger;

        public DemandaFileController(GestorDbContext context, IConfiguration configuration, IMapper mapper,
            ILogger<DemandaFileController> logger) : base(
            context,
            configuration)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [HttpDelete("{id}/Files/{fileId}")]
        public IActionResult RemoveFile(int id, int fileId)
        {
            var file = context.DemandaFiles.FirstOrDefault(df => df.DemandaId == id && df.Id == fileId);
            if (file == null) return NotFound();
            try
            {
                context.DemandaFiles.Remove(file);
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

        [HttpGet("{id}/Files")]
        public ActionResult<List<DemandaFileDto>> GetFiles(int id)
        {
            var files = context
                .DemandaFiles
                .Where(file => file.DemandaId == id)
                .ToList();
            return _mapper.Map<List<DemandaFileDto>>(files);
        }

        [HttpPost("{id}/Files")]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<List<DemandaFileDto>>> Upload(int id)
        {
            var files = await base.Upload(file =>
            {
                file.DemandaId = id;
                return file;
            });
            return _mapper.Map<List<DemandaFileDto>>(files);
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
                UserId = this.UserId(),
                CreatedAt = DateTime.Now
            };
        }
    }
}