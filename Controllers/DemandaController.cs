using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using APIGestor.Models.Demandas;
using APIGestor.Business;
using APIGestor.Models.Demandas.Forms;

namespace APIGestor.Controllers
{
    [Route("api/Demandas/")]
    [ApiController]
    [Authorize("Bearer")]
    public class DemandaController : Controller
    {

        AppService appService;
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
        public DemandaController(DemandaService DemandaService, AppService appService)
        {
            this.Service = DemandaService;
            this.appService = appService;
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