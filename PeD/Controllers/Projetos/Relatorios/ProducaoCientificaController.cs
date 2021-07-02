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
    public class ProducaoCientificaController : ProjetoNodeBaseController<ProducaoCientifica, ProducaoCientificaRequest,
        ProducaoCientificaDto>
    {
        public ProducaoCientificaController(IService<ProducaoCientifica> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService) : base(service, mapper,
            authorizationService, projetoService)
        {
        }

        [HttpPost("{id:int}/Arquivos/Origem")]
        public ActionResult UploadRelatorio(int id, List<IFormFile> file, [FromServices] ArquivoService arquivoService)
        {
            var producaoCientifica = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id && r.Id == id))
                .FirstOrDefault();
            if (producaoCientifica is null || file.Count == 0 || file.Count > 1)
                return BadRequest();
            var fileupload = arquivoService.SaveFile(file.FirstOrDefault());
            producaoCientifica.ArquivoTrabalhoOrigemId = fileupload.Id;
            Service.Put(producaoCientifica);
            return Ok();
        }

        [HttpGet("{id:int}/Arquivos/Origem")]
        public ActionResult GetRelatorioPdf(int id)
        {
            var producaoCientifica = Service
                .Filter(q =>
                    q.Include(e => e.ArquivoTrabalhoOrigem).Where(r => r.ProjetoId == Projeto.Id && r.Id == id))
                .FirstOrDefault();
            if (producaoCientifica is null || producaoCientifica.ArquivoTrabalhoOrigem is null)
                return NotFound();
            return PhysicalFile(producaoCientifica.ArquivoTrabalhoOrigem.Path,
                producaoCientifica.ArquivoTrabalhoOrigem.ContentType,
                producaoCientifica.ArquivoTrabalhoOrigem.FileName);
        }

        protected override void BeforePut(ProducaoCientifica actual, ProducaoCientifica @new)
        {
            @new.ArquivoTrabalhoOrigemId = actual.ArquivoTrabalhoOrigemId;
        }
    }
}