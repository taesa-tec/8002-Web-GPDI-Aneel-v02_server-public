using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;
using Empresa = PeD.Core.Models.Propostas.Empresa;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/RecursosHumanos/Alocacao")]
    public class
        AlocacaoRecursosHumanosController : PropostaNodeBaseController<AlocacaoRh,
            AlocacaoRecursoHumanoRequest,
            AlocacaoRecursoHumanoDto>
    {
        private GestorDbContext _context;

        public AlocacaoRecursosHumanosController(IService<AlocacaoRh> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper,
                authorizationService, propostaService)
        {
            _context = context;
        }

        protected override IQueryable<AlocacaoRh> Includes(IQueryable<AlocacaoRh> queryable)
        {
            return queryable
                    .Include(r => r.HorasMeses)
                    .Include(r => r.Recurso)
                    .Include(r => r.Etapa)
                    .Include(r => r.EmpresaFinanciadora)
                ;
        }

        protected override ActionResult Validate(AlocacaoRecursoHumanoRequest request)
        {
            if (!_context.Set<Etapa>().Any(e => e.PropostaId == Proposta.Id && e.Id == request.EtapaId))
            {
                return ValidationProblem("Etapa não encontrada");
            }

            var recurso = _context.Set<RecursoHumano>().Include(r => r.Empresa)
                .FirstOrDefault(r => r.Id == request.RecursoId && r.PropostaId == Proposta.Id);
            var empresa = _context.Set<Empresa>()
                .FirstOrDefault(e => e.Id == request.EmpresaFinanciadoraId && e.PropostaId == Proposta.Id);
            if (recurso != null && recurso.Empresa.Funcao == Funcao.Cooperada &&
                empresa is { Funcao: Funcao.Executora })
                return ValidationProblem(
                    "Não é permitido um co-executor/executor destinar recursos a uma empresa Proponente/Cooperada");

            if (!_context.Set<Empresa>().Any(e => e.Id == request.EmpresaFinanciadoraId && e.PropostaId == Proposta.Id))
                return ValidationProblem("Empresa Inválida");
            if (empresa is { Funcao: Funcao.Cooperada } && request.HoraMeses.Any(kv => kv.Value > 160) ||
                empresa is { Funcao: Funcao.Executora } && request.HoraMeses.Any(kv => kv.Value > 172))
                return ValidationProblem("Quantidade de horas limite por mês não permitida");
            return null;
        }

        protected override void BeforePut(AlocacaoRh actual, AlocacaoRh @new)
        {
            var horas = _context.Set<AlocacaoRhHorasMes>().Where(a => a.AlocacaoRhId == actual.Id).ToList();
            _context.RemoveRange(horas);
            _context.AddRange(@new.HorasMeses);
        }
    }
}