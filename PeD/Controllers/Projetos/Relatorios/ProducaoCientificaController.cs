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
    public class ProducaoCientificaController : ProjetoNodeBaseController<ProducaoCientifica, ProducaoCientificaRequest,
        ProducaoCientificaDto>
    {
        public ProducaoCientificaController(IService<ProducaoCientifica> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService, GestorDbContext context) : base(
            service, mapper,
            authorizationService, projetoService, context)
        {
        }

        [HttpPost("{id:int}/Arquivos/Origem")]
        public async Task<ActionResult> UploadRelatorio(int id, List<IFormFile> file,
            [FromServices] ArquivoService arquivoService)
        {
            if (!await HasAccess(true))
                return Forbid();
            var producaoCientifica = Service.Filter(q => q.Where(r => r.ProjetoId == Projeto.Id && r.Id == id))
                .FirstOrDefault();
            if (producaoCientifica is null || file.Count == 0 || file.Count > 1)
                return BadRequest();
            var fileupload = await arquivoService.SaveFile(file.FirstOrDefault());
            producaoCientifica.ArquivoTrabalhoOrigemId = fileupload.Id;
            Service.Put(producaoCientifica);
            return Ok();
        }

        [HttpGet("{id:int}/Arquivos/Origem")]
        public async Task<ActionResult> GetRelatorioPdf(int id)
        {
            if (!await HasAccess())
                return Forbid();
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

        protected override bool Validate(ProducaoCientificaRequest request, out object error)
        {
            if (!base.Validate(request, out error)) return false;

            if (!request.ArquivoTrabalhoOrigemId.HasValue && // Se não foi enviado a id do arquivo e é um entrada nova
                request.Id == 0 ||
                !(request.ArquivoTrabalhoOrigemId
                      .HasValue && // Ou se o arquivo foi enviado mas não foi o usuário atual que o enviou
                  Context.Files.Any(f => f.UserId == this.UserId() && f.Id == request.ArquivoTrabalhoOrigemId)))
            {
                error = "Arquivo não enviado ou inválido";
                return false;
            }

            return true;
        }

        protected override void BeforePut(ProducaoCientifica actual, ProducaoCientifica @new)
        {
            @new.ArquivoTrabalhoOrigemId ??= actual.ArquivoTrabalhoOrigemId;
        }
    }
}