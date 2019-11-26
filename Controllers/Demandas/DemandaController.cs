using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using APIGestor.Models.Demandas;
using APIGestor.Business;
using APIGestor.Business.Demandas;
using APIGestor.Models.Demandas.Forms;
using iText.Html2pdf;
using System.IO;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using APIGestor.Business.Sistema;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Controllers.Demandas
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    public partial class DemandaController : Controller
    {
        SistemaService sistemaService;
        protected DemandaService Service { get; }
        protected DemandaLogService LogService { get; }
        private IHostingEnvironment _hostingEnvironment;
        protected List<FieldList> _forms = new List<FieldList>(){
                new EspecificacaoTecnicaForm()
        };
        public DemandaController(DemandaService DemandaService, SistemaService sistemaService, IHostingEnvironment hostingEnvironment)
        {
            this.Service = DemandaService;
            
            this.sistemaService = sistemaService;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet("{etapa:alpha}")]
        public List<Demanda> GetByEtapa(Etapa etapa)
        {
            return this.Service.GetByEtapa(etapa, this.userId());
        }

        [HttpGet("Reprovadas")]
        public List<Demanda> GetDemandasReprovadas()
        {
            return this.Service.GetDemandasReprovadas(this.userId());
        }
        [HttpGet("Aprovadas")]
        public List<Demanda> GetDemandasAprovadas()
        {
            return this.Service.GetDemandasAprovadas(this.userId());
        }

        [HttpGet("EmElaboracao")]
        public List<Demanda> GetDemandasEmElaboracao()
        {
            return this.Service.GetDemandasEmElaboracao(this.userId());
        }

        [HttpGet("Captacao")]
        public List<Demanda> GetDemandasCaptacao()
        {
            return this.Service.GetDemandasCaptacao(this.userId());
        }


    }
}