using System.Linq;
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
            return queryable.Include(r => r.Etapa).ThenInclude(e => e.Produto);
        }
    }
}