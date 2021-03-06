using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos.Resultados;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Requests.Projetos.Resultados;
using PeD.Data;
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
            IAuthorizationService authorizationService, ProjetoService projetoService, GestorDbContext context) : base(
            service, mapper,
            authorizationService, projetoService, context)
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
        public async Task<ActionResult> Edit(RelatorioFinalRequest request,
            [FromServices] ArquivoService arquivoService)
        {
            // Verifica se o usuário tem acesos ao projeto e se os arquivos selecionados foram enviados pelo usuário
            if (!await HasAccess(true) ||
                (request.AuditoriaRelatorioArquivoId.HasValue && !arquivoService.IsUserFiles(this.UserId(),
                    request.AuditoriaRelatorioArquivoId.Value)) ||
                (request.RelatorioArquivoId.HasValue && !arquivoService.IsUserFiles(this.UserId(),
                    request.RelatorioArquivoId.Value))
               )
                return Forbid();
            var prevRelatorio = Service.Filter(q => q.AsNoTracking().Where(r => r.ProjetoId == Projeto.Id))
                .FirstOrDefault();
            var relatorio = Mapper.Map<RelatorioFinal>(request);
            relatorio.ProjetoId = Projeto.Id;
            if (prevRelatorio is null)
            {
                if (!request.AuditoriaRelatorioArquivoId.HasValue || !request.RelatorioArquivoId.HasValue)
                    return BadRequest();
                Service.Post(relatorio);
            }
            else
            {
                relatorio.Id = prevRelatorio.Id;
                relatorio.RelatorioArquivoId = request.RelatorioArquivoId ?? prevRelatorio.RelatorioArquivoId;
                relatorio.AuditoriaRelatorioArquivoId =
                    request.AuditoriaRelatorioArquivoId ?? prevRelatorio.AuditoriaRelatorioArquivoId;
                Service.Put(relatorio);
            }

            return Ok();
        }

        [HttpPost("Arquivos/Relatorio")]
        public async Task<ActionResult> UploadRelatorio(List<IFormFile> file,
            [FromServices] ArquivoService arquivoService)
        {
            var relatorio = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id)).FirstOrDefault();
            if (relatorio is null || file.Count == 0 || file.Count > 1)
                return BadRequest();

            if (!file[0].FileName.ToLower().EndsWith(".pdf", true, null))
            {
                return BadRequest("É necessário enviar um arquivo pdf");
            }

            var fileupload = await arquivoService.SaveFile(file.FirstOrDefault());
            relatorio.RelatorioArquivoId = fileupload.Id;
            Service.Put(relatorio);
            return Ok();
        }

        [HttpPost("Arquivos/RelatorioAuditoria")]
        public async Task<ActionResult> UploadRelatorioAuditoria(List<IFormFile> file,
            [FromServices] ArquivoService arquivoService)
        {
            var relatorio = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id)).FirstOrDefault();
            if (relatorio is null || file.Count == 0 || file.Count > 1)
                return BadRequest();
            if (!file[0].FileName.ToLower().EndsWith(".pdf", true, null))
            {
                return BadRequest("É necessário enviar um arquivo pdf");
            }

            var fileupload = await arquivoService.SaveFile(file.FirstOrDefault());
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