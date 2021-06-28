using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Projetos.Resultados;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Requests.Projetos.Resultados;
using PeD.Services.Projetos;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos.Relatorios
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Projetos/{projetoId:int}/[controller]")]
    public class
        SocioambientalController : ProjetoNodeBaseController<Socioambiental, SocioambientalRequest, SocioambientalDto>
    {
        public SocioambientalController(IService<Socioambiental> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService) : base(service, mapper,
            authorizationService, projetoService)
        {
        }
    }
}