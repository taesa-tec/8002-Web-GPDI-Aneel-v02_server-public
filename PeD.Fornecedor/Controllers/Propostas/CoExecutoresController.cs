using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.FornecedoresDtos;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Fornecedor.Controllers.Propostas
{
    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{propostaId:int}/[controller]")]
    public class CoExecutoresController : ControllerCrudBase<CoExecutor, CoExecutorDto>
    {
        public CoExecutoresController(IService<CoExecutor> service, IMapper mapper) : base(service, mapper)
        {
        }

        public override ActionResult<List<CoExecutorDto>> Get()
        {
            int propostaId = int.Parse(RouteData.Values["propostaId"].ToString() ?? string.Empty);
            //Service.Filter(query=>query.Where(c=>))
            return base.Get();
        }
    }
}