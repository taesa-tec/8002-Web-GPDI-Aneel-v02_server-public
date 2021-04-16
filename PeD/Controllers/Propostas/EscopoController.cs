using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class EscopoController : ControllerServiceBase<Escopo>
    {
        private PropostaService _propostaService;

        public EscopoController(IService<Escopo> service, IMapper mapper, PropostaService propostaService) : base(
            service, mapper)
        {
            _propostaService = propostaService;
        }

        [HttpGet]
        public IActionResult Get([FromRoute] int captacaoId, [FromServices] IService<Meta> serviceMeta)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var escopo = Service.Filter(q => q.Where(e => e.PropostaId == proposta.Id)).FirstOrDefault() ??
                         new Escopo();
            var metas = serviceMeta.Filter(q => q.Where(e => e.PropostaId == proposta.Id)).ToList();
            var response = Mapper.Map<PropostaEscopoDto>(escopo);
            response.Metas = Mapper.Map<List<PropostaEscopoDto.MetaDto>>(metas);
            return Ok(response);
        }
        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public IActionResult Post([FromRoute] int captacaoId, [FromBody] PropostaEscopoDto escopoDto,
            [FromServices] GestorDbContext context)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var metaContext = context.Set<Meta>();

            var escopoPrev = Service.Filter(q => q.Where(e => e.PropostaId == proposta.Id)).FirstOrDefault();
            if (escopoPrev == null)
            {
                var escopo = Mapper.Map<Escopo>(escopoDto);
                escopo.PropostaId = proposta.Id;
                Service.Post(escopo);
            }
            else
            {
                var escopo = Mapper.Map(escopoDto, escopoPrev);
                Service.Put(escopo);
            }

            var metasId = metaContext.Where(m => m.PropostaId == proposta.Id).Select(m => m.Id).ToList();
            var metas = Mapper.Map<List<Meta>>(escopoDto.Metas);
            var metasIdsRequest = metas
                .Where(m => metasId.Contains(m.Id))
                .Select(m => m.Id)
                .ToList();

            var excluidos = metaContext.Where(m => m.PropostaId == proposta.Id && !metasIdsRequest.Contains(m.Id))
                .ToList();
            metaContext.RemoveRange(excluidos);
            metas.ForEach(m =>
            {
                m.PropostaId = proposta.Id;
                if (m.Id > 0 && metasId.Contains(m.Id))
                {
                    metaContext.Update(m);
                }
                else
                {
                    metaContext.Add(m);
                }
            });
            context.SaveChanges();

            return Ok();
        }
    }
}