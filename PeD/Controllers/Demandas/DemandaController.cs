using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using PeD.Authorizations;
using PeD.Core.ApiModels.Demandas;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Demandas.Forms;
using PeD.Data;
using PeD.Services.Demandas;
using PeD.Services.Sistema;
using TaesaCore.Controllers;

namespace PeD.Controllers.Demandas
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    [Authorize(Policy = Policies.IsColaborador)]
    public partial class DemandaController : ControllerServiceBase<Demanda>
    {
        SistemaService sistemaService;
        private GestorDbContext context;
        protected DemandaService DemandaService { get; }

        protected List<FieldList> _forms = new List<FieldList>()
        {
            new EspecificacaoTecnicaForm()
        };

        public DemandaController(DemandaService demandaService, IMapper mapper, SistemaService sistemaService,
            GestorDbContext context) : base(
            demandaService, mapper)
        {
            DemandaService = demandaService;
            this.sistemaService = sistemaService;
            this.context = context;
        }


        [HttpGet("{etapa:alpha}")]
        public List<DemandaDto> GetByEtapa(DemandaEtapa demandaEtapa)
        {
            return Mapper.Map<List<DemandaDto>>(DemandaService.GetByEtapa(demandaEtapa,
                this.IsAdmin() ? null : this.UserId()
            ));
        }

        [HttpGet("Reprovadas")]
        public List<DemandaDto> GetDemandasReprovadas()
        {
            return Mapper.Map<List<DemandaDto>>(
                DemandaService.GetDemandasReprovadas(this.IsAdmin() ? null : this.UserId()));
        }

        [HttpGet("Aprovadas")]
        public List<DemandaDto> GetDemandasAprovadas()
        {
            return Mapper.Map<List<DemandaDto>>(
                DemandaService.GetDemandasAprovadas(this.IsAdmin() ? null : this.UserId()));
        }

        [HttpGet("EmElaboracao")]
        public List<DemandaDto> GetDemandasEmElaboracao()
        {
            return Mapper.Map<List<DemandaDto>>(
                DemandaService.GetDemandasEmElaboracao(this.IsAdmin() ? null : this.UserId()));
        }

        [HttpGet("Captacao")]
        public List<DemandaDto> GetDemandasCaptacao()
        {
            return Mapper.Map<List<DemandaDto>>(
                DemandaService.GetDemandasCaptacao(this.IsAdmin() ? null : this.UserId()));
        }
    }
}