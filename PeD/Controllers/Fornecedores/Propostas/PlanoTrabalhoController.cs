using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class PlanoTrabalhoController : Controller
    {
        private GestorDbContext _context;
        private PropostaService _propostaService;
        private IService<PlanoTrabalho> Service;
        private IMapper _mapper;

        public PlanoTrabalhoController(GestorDbContext context, PropostaService propostaService,
            IService<PlanoTrabalho> service, IMapper mapper)
        {
            _context = context;
            _propostaService = propostaService;
            Service = service;
            _mapper = mapper;
        }

        [HttpGet("")]
        public ActionResult<PlanoTrabalhoDto> Get([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta == null)
                return NotFound();
            var plano = Service.Filter(q => q
                            .Include(p => p.Proposta)
                            .ThenInclude(p => p.Arquivos)
                            .ThenInclude(a => a.Arquivo)
                            .Where(p => p.PropostaId == proposta.Id)).FirstOrDefault() ??
                        new PlanoTrabalho();
            return _mapper.Map<PlanoTrabalhoDto>(plano);
        }

        [HttpPost("")]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] PlanoTrabalhoRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            if (proposta == null)
                return NotFound();
            var planoPrev = Service.Filter(q => q.AsNoTracking().Where(p => p.PropostaId == proposta.Id))
                .FirstOrDefault();
            var plano = _mapper.Map<PlanoTrabalho>(request);
            plano.PropostaId = proposta.Id;

            if (planoPrev != null)
            {
                plano.Id = planoPrev.Id;
                Service.Put(plano);
            }
            else
            {
                Service.Post(plano);
            }

            _propostaService.UpdatePropostaDataAlteracao(proposta.Id);

            return Ok();
        }
    }
}