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

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class CoExecutoresController : ControllerCrudBase<CoExecutor, CoExecutorDto>
    {
        public CoExecutoresController(IService<CoExecutor> service, IMapper mapper) : base(service, mapper)
        {
        }

        public override ActionResult<List<CoExecutorDto>> Get()
        {
            int captacaoId = int.Parse(RouteData.Values["captacaoId"].ToString() ?? string.Empty);
            //Service.Filter(query=>query.Where(c=>))
            return base.Get();
        }
    }
}