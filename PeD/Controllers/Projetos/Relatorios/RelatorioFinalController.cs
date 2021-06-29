using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos.Resultados;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Requests.Projetos.Resultados;
using PeD.Services;
using PeD.Services.Projetos;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos.Relatorios
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Projetos/{projetoId:int}/Relatorio/[controller]")]
    public class
        RelatorioFinalController : ProjetoNodeBaseController<RelatorioFinal>
    {
        public RelatorioFinalController(IService<RelatorioFinal> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService) : base(service, mapper,
            authorizationService, projetoService)
        {
        }

        [HttpGet]
        public ActionResult<RelatorioFinalDto> Get()
        {
            var relatorio = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id)).FirstOrDefault();
            return Ok(Mapper.Map<RelatorioFinalDto>(relatorio ?? new RelatorioFinal()));
        }

        [HttpPost]
        [HttpPut]
        public ActionResult Edit(RelatorioFinalRequest request)
        {
            var prevRelatorio = Service.Filter(q => q.AsNoTracking().Where(r => r.ProjetoId == Projeto.Id))
                .FirstOrDefault();
            var relatorio = Mapper.Map<RelatorioFinal>(request);
            if (prevRelatorio is null)
                Service.Post(relatorio);
            else
            {
                relatorio.Id = prevRelatorio.Id;
                Service.Put(relatorio);
            }

            return Ok();
        }

        [HttpPost("Arquivos/Relatorio")]
        public ActionResult UploadRelatorio(List<IFormFile> file, [FromServices] ArquivoService arquivoService)
        {
            var relatorio = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id)).FirstOrDefault();
            if (relatorio is null || file.Count == 0 || file.Count > 1)
                return BadRequest();
            var fileupload = arquivoService.SaveFile(file.FirstOrDefault());
            relatorio.RelatorioArquivoId = fileupload.Id;
            Service.Put(relatorio);
            return Ok();
        }

        [HttpPost("Arquivos/RelatorioAuditoria")]
        public ActionResult UploadRelatorioAuditoria(List<IFormFile> file, [FromServices] ArquivoService arquivoService)
        {
            var relatorio = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id)).FirstOrDefault();
            if (relatorio is null || file.Count == 0 || file.Count > 1)
                return BadRequest();
            var fileupload = arquivoService.SaveFile(file.FirstOrDefault());
            relatorio.AuditoriaRelatorioArquivoId = fileupload.Id;
            Service.Put(relatorio);
            return Ok();
        }

        [HttpGet("Arquivos/Relatorio")]
        public ActionResult GetRelatorioPdf()
        {
            var relatorio = Service
                .Filter(q => q.Include(e => e.RelatorioArquivo).Where(r => r.ProjetoId == Projeto.Id)).FirstOrDefault();
            if (relatorio is null || relatorio.RelatorioArquivo is null)
                return NotFound();
            return PhysicalFile(relatorio.RelatorioArquivo.Path, relatorio.RelatorioArquivo.ContentType,
                relatorio.RelatorioArquivo.FileName);
        }

        [HttpGet("Arquivos/RelatorioAuditoria")]
        public ActionResult GetRelatorioAuditoriaPdf()
        {
            var relatorio = Service
                .Filter(q => q.Include(e => e.AuditoriaRelatorioArquivo).Where(r => r.ProjetoId == Projeto.Id))
                .FirstOrDefault();
            if (relatorio is null || relatorio.AuditoriaRelatorioArquivo is null)
                return NotFound();
            return PhysicalFile(relatorio.AuditoriaRelatorioArquivo.Path,
                relatorio.AuditoriaRelatorioArquivo.ContentType,
                relatorio.AuditoriaRelatorioArquivo.FileName);
        }
    }
}