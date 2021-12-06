using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PropriedadeIntelectualController : ProjetoNodeBaseController<PropriedadeIntelectual,
        PropriedadeIntelectualRequest, PropriedadeIntelectualDto>
    {

        public PropriedadeIntelectualController(IService<PropriedadeIntelectual> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService, GestorDbContext context) : base(
            service, mapper,
            authorizationService, projetoService, context)
        {
        }

        protected override IQueryable<PropriedadeIntelectual> Includes(IQueryable<PropriedadeIntelectual> queryable)
        {
            return queryable
                .Include(p => p.Depositantes)
                .ThenInclude(d => d.Empresa)
                .Include(d => d.Inventores)
                .ThenInclude(i => i.Recurso);
        }

        protected override void BeforePut(PropriedadeIntelectual actual, PropriedadeIntelectual @new)
        {
            var depositantes = Context.Set<PropriedadeIntelectualDepositante>()
                .Where(p => p.PropriedadeId == actual.Id).ToList();
            var invetores = Context.Set<PropriedadeIntelectualInventor>()
                .Where(p => p.PropriedadeId == actual.Id).ToList();
            Context.RemoveRange(depositantes);
            Context.RemoveRange(invetores);
            Context.SaveChanges();
            Context.AddRange(@new.Depositantes);
            Context.AddRange(@new.Inventores);
            Context.SaveChanges();
        }
    }
}