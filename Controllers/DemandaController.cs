using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using APIGestor.Models.Demandas;
using APIGestor.Business;
using APIGestor.Models.Demandas.Forms;
using iText.Html2pdf;
using System.IO;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;

namespace APIGestor.Controllers
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    public class DemandaController : Controller
    {

        AppService appService;
        private IHostingEnvironment _hostingEnvironment;
        protected List<FieldList> _forms = new List<FieldList>(){
                new EspecificacaoTecnicaForm()
        };

        [AllowAnonymous]
        [HttpGet("Forms")]
        public IEnumerable<object> Forms()
        {
            return _forms.Select(f => new
            {
                f.Key,
                f.Title
            });
        }
        [AllowAnonymous]
        [HttpGet("Forms/{key}")]
        public FieldList Form(string key)
        {
            return _forms.Find(f => f.Key == key);
        }
        [AllowAnonymous]
        [HttpGet("Forms/{key}/Values")]
        public object FormValues(string key)
        {
            return appService.getOption<object>(string.Format("form-value-{0}", key));
        }

        [HttpPost("Forms/{key}/Values")]
        public ActionResult SetFormValues(string key, [FromBody] object data)
        {
            appService.setOption(string.Format("form-value-{0}", key), data);
            return Ok();
        }
        protected DemandaService Service { get; }
        public DemandaController(DemandaService DemandaService, AppService appService, IHostingEnvironment hostingEnvironment)
        {
            this.Service = DemandaService;
            this.appService = appService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{etapa}")]
        public List<Demanda> GetByEtapa(Etapa etapa)
        {
            return this.Service.GetByEtapa(etapa);
        }

        [HttpGet("Reprovadas")]
        public List<Demanda> GetDemandasReprovadas()
        {
            return this.Service.GetDemandasReprovadas();
        }
        [HttpGet("Aprovadas")]
        public List<Demanda> GetDemandasAprovadas()
        {
            return this.Service.GetDemandasAprovadas();
        }

        [HttpGet("EmElaboracao")]
        public List<Demanda> GetDemandasEmElaboracao()
        {
            return this.Service.GetDemandasEmElaboracao();
        }

        [HttpGet("Captacao")]
        public List<Demanda> GetDemandasCaptacao()
        {
            return this.Service.GetDemandasCaptacao();
        }

        [HttpPost("Criar")]
        public ActionResult CriarDemanda([FromBody]string titulo)
        {
            Service.CriarDemanda(titulo, this.userId());
            return Ok();
        }

        [HttpPut("{id}/Captacao")]
        public ActionResult EnviarCaptacao(int id)
        {
            Service.EnviarCaptacao(id);
            return Ok();
        }

        [HttpPut("{id}/Status")]
        public ActionResult AlterarStatusDemanda(int id, APIGestor.Models.Demandas.EtapaStatus status)
        {
            return Ok();
        }

        [HttpPut("{id}/Aprovar")]
        public ActionResult AprovarDemanda(int id)
        {
            return Ok();
        }
        [HttpPut("{id}/Reprovar")]
        public ActionResult ReprovarDemanda(int id)
        {
            return Ok();
        }
        [HttpGet("{id}/Form/{form}")]
        public ActionResult<object> GetDemandaFormValue(int id, string form)
        {
            var data = Service.GetDemandaFormData(id, form);
            if (data != null)
            {
                return data;
            }
            return default(object);
        }
        [AllowAnonymous]
        [HttpGet("{id}/Form/{form}/Pdf")]
        public ActionResult<object> GetDemandaPDF(int id, string form)
        {
            var filename = Service.SaveDemandaFormPdf(id, form);
            return PhysicalFile(filename, "application/pdf");

        }
        [AllowAnonymous]
        [HttpGet("{id}/Form/{form}/Debug")]
        public ActionResult<object> GetDemandaTeste(int id, string form)
        {
            var doc = Service.RenderDocument(id, form);
            if (doc != null)
            {
                return doc;
            }
            return NotFound();

        }

        [HttpPut("{id}/Form/{form}")]
        public ActionResult<object> SalvarDemandaFormValue(int id, string form, [FromBody] object data)
        {
            if (Service.DemandaExist(id))
            {
                Service.SalvarDemandaFormData(id, form, data);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
    }
}