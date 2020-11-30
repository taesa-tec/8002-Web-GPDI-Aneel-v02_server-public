using System.Collections.Generic;
using APIGestor.Dtos.Demandas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Models.Demandas;
using APIGestor.Models.Demandas.Forms;
using APIGestor.Services.Demandas;
using APIGestor.Services.Sistema;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using TaesaCore.Controllers;

namespace APIGestor.Controllers.Demandas
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    public partial class DemandaController : ControllerServiceBase<Demanda>
    {
        SistemaService sistemaService;
        protected DemandaService DemandaService { get; }

        protected List<FieldList> _forms = new List<FieldList>()
        {
            new EspecificacaoTecnicaForm()
        };

        public DemandaController(DemandaService demandaService, IMapper mapper, SistemaService sistemaService) : base(
            demandaService, mapper)
        {
            DemandaService = demandaService;
            this.sistemaService = sistemaService;
        }


        [HttpGet("{etapa:alpha}")]
        public List<DemandaDto> GetByEtapa(DemandaEtapa demandaEtapa)
        {
            return Mapper.Map<List<DemandaDto>>(DemandaService.GetByEtapa(demandaEtapa, this.userId()));
        }

        [HttpGet("Reprovadas")]
        public List<DemandaDto> GetDemandasReprovadas()
        {
            return Mapper.Map<List<DemandaDto>>(DemandaService.GetDemandasReprovadas(this.userId()));
        }

        [HttpGet("Aprovadas")]
        public List<DemandaDto> GetDemandasAprovadas()
        {
            return Mapper.Map<List<DemandaDto>>(DemandaService.GetDemandasAprovadas(this.userId()));
        }

        [HttpGet("EmElaboracao")]
        public List<DemandaDto> GetDemandasEmElaboracao()
        {
            return Mapper.Map<List<DemandaDto>>(DemandaService.GetDemandasEmElaboracao(this.userId()));
        }

        [HttpGet("Captacao")]
        public List<DemandaDto> GetDemandasCaptacao()
        {
            return Mapper.Map<List<DemandaDto>>(DemandaService.GetDemandasCaptacao(this.userId()));
        }
    }
}