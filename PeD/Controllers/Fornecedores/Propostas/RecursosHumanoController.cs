using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class
        RecursosHumanoController : PropostaNodeBaseController<RecursoHumano, RecursoHumanoRequest, RecursoHumanoDto>
    {
        public RecursosHumanoController(IService<RecursoHumano> service, IMapper mapper,
            PropostaService propostaService) : base(service, mapper, propostaService)
        {
        }

        protected override IQueryable<RecursoHumano> Includes(IQueryable<RecursoHumano> queryable)
        {
            return queryable
                    .Include(r => r.Empresa)
                    .Include(r => r.CoExecutor)
                ;
        }
    }
}