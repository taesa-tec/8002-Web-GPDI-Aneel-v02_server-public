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
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/RecursosMateriais/Alocacao")]
    public class
        AlocacaoRecursosMateriaisController : PropostaNodeBaseController<RecursoMaterial.AlocacaoRm,
            AlocacaoRecursoMaterialRequest,
            AlocacaoRecursoMaterialDto>
    {
        public AlocacaoRecursosMateriaisController(IService<RecursoMaterial.AlocacaoRm> service, IMapper mapper,
            PropostaService propostaService) : base(service, mapper, propostaService)
        {
        }

        protected override IQueryable<RecursoMaterial.AlocacaoRm> Includes(IQueryable<RecursoMaterial.AlocacaoRm> queryable)
        {
            return queryable
                    .Include(r => r.Recurso)
                    .ThenInclude(r => r.CategoriaContabil)
                    .Include(r => r.Etapa)
                    .Include(r => r.EmpresaFinanciadora)
                    .Include(r => r.EmpresaRecebedora)
                ;
        }
    }
}