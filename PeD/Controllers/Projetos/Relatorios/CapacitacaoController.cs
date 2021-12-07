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
    public class CapacitacaoController : ProjetoNodeBaseController<Capacitacao, CapacitacaoRequest, CapacitacaoDto>
    {
        public CapacitacaoController(IService<Capacitacao> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService, GestorDbContext context) : base(
            service, mapper,
            authorizationService, projetoService, context)
        {
        }

        protected override IQueryable<Capacitacao> Includes(IQueryable<Capacitacao> queryable)
        {
            return queryable.Include(c => c.Recurso);
        }

        [HttpPost("{id:int}/Arquivos/Origem")]
        public async Task<ActionResult> UploadRelatorio(int id, List<IFormFile> file,
            [FromServices] ArquivoService arquivoService)
        {
            if (!await HasAccess(true))
                return Forbid();
            var capacitacao = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id && r.Id == id))
                .FirstOrDefault();
            if (capacitacao is null || file.Count == 0 || file.Count > 1)
                return BadRequest();
            var fileupload = await arquivoService.SaveFile(file.FirstOrDefault());
            capacitacao.ArquivoTrabalhoOrigemId = fileupload.Id;
            Service.Put(capacitacao);
            return Ok();
        }

        [HttpGet("{id:int}/Arquivos/Origem")]
        public async Task<ActionResult> GetRelatorioPdf(int id)
        {
            if (!await HasAccess())
                return Forbid();
            
            var capacitacao = Service
                .Filter(q =>
                    q.Include(e => e.ArquivoTrabalhoOrigem).Where(r => r.ProjetoId == Projeto.Id && r.Id == id))
                .FirstOrDefault();
            if (capacitacao is null || capacitacao.ArquivoTrabalhoOrigem is null)
                return NotFound();
            return PhysicalFile(capacitacao.ArquivoTrabalhoOrigem.Path, capacitacao.ArquivoTrabalhoOrigem.ContentType,
                capacitacao.ArquivoTrabalhoOrigem.FileName);
        }

        protected override void BeforePut(Capacitacao actual, Capacitacao @new)
        {
            @new.ArquivoTrabalhoOrigemId = actual.ArquivoTrabalhoOrigemId;
        }
    }
}