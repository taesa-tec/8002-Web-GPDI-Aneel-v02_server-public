using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos.Resultados;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Requests.Projetos.Resultados;
using PeD.Services.Projetos;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos.Relatorios
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Projetos/{projetoId:int}/Relatorio/[controller]")]
    public class
        RelatorioEtapaController : ProjetoNodeBaseController<RelatorioEtapa, RelatorioEtapaRequest, RelatorioEtapaDto>
    {
        public RelatorioEtapaController(IService<RelatorioEtapa> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService) : base(service, mapper,
            authorizationService, projetoService)
        {
        }

        protected override IQueryable<RelatorioEtapa> Includes(IQueryable<RelatorioEtapa> queryable)
        {
            return queryable.Include(r => r.Projeto).Include(r => r.Etapa).ThenInclude(e => e.Produto)
                .OrderBy(e => e.Etapa.Ordem);
        }

#pragma warning disable 1998
        public override async Task<IActionResult> Post(RelatorioEtapaRequest request)
#pragma warning restore 1998
        {
            var etapa = Service.Get(request.Id);
            if (etapa is null || etapa.ProjetoId != Projeto.Id)
                return NotFound();

            etapa.AtividadesRealizadas = request.AtividadesRealizadas;
            Service.Put(etapa);
            return Ok(Mapper.Map<RelatorioEtapaDto>(etapa));
        }

        public override async Task<IActionResult> Put(RelatorioEtapaRequest request)
        {
            return await Post(request);
        }

#pragma warning disable 1998
        public override async Task<IActionResult> Delete(int id)
#pragma warning restore 1998
        {
            return Forbid();
        }
    }
}