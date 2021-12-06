using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Projetos.Resultados;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Requests.Projetos.Resultados;
using PeD.Data;
using PeD.Services.Projetos;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos.Relatorios
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Projetos/{projetoId:int}/Relatorio/[controller]")]
    public class IndicadorEconomicoController : ProjetoNodeBaseController<IndicadorEconomico, IndicadorEconomicoRequest,
        IndicadorEconomicoDto>
    {
        public IndicadorEconomicoController(IService<IndicadorEconomico> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService,  GestorDbContext context) : base(service, mapper,
            authorizationService, projetoService, context)
        {
        }
    }
}