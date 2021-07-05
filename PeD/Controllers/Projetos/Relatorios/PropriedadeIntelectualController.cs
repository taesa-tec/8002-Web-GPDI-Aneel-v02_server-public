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
        private GestorDbContext _context;

        public PropriedadeIntelectualController(IService<PropriedadeIntelectual> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService, GestorDbContext context) : base(
            service, mapper,
            authorizationService, projetoService)
        {
            _context = context;
        }

        protected override IQueryable<PropriedadeIntelectual> Includes(IQueryable<PropriedadeIntelectual> queryable)
        {
            return queryable
                .Include(p => p.Depositantes)
                .ThenInclude(d => d.Empresa)
                .Include(p => p.Depositantes)
                .ThenInclude(d => d.CoExecutor)
                .Include(d => d.Inventores)
                .ThenInclude(i => i.Recurso);
        }

        protected override void BeforePut(PropriedadeIntelectual actual, PropriedadeIntelectual @new)
        {
            var depositantes = _context.Set<PropriedadeIntelectualDepositante>()
                .Where(p => p.PropriedadeId == actual.Id).ToList();
            var invetores = _context.Set<PropriedadeIntelectualInventor>()
                .Where(p => p.PropriedadeId == actual.Id).ToList();
            _context.RemoveRange(depositantes);
            _context.RemoveRange(invetores);
            _context.SaveChanges();
            _context.AddRange(@new.Depositantes);
            _context.AddRange(@new.Inventores);
            _context.SaveChanges();
        }
    }
}