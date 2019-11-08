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

namespace APIGestor.Controllers.Demandas
{
    public partial class DemandaController : Controller
    {
        [HttpPost("Criar")]
        public ActionResult CriarDemanda([FromBody]string titulo)
        {
            Service.CriarDemanda(titulo, this.userId());
            return Ok();
        }

        [HttpGet("{id:int}")]
        public Demanda GetById(int id)
        {
            return this.Service.GetById(id);
        }

        [HttpPut("{id}/Captacao")]
        public ActionResult EnviarCaptacao(int id)
        {
            Service.EnviarCaptacao(id);
            return Ok();
        }
        [HttpPut("{id}/EquipeValidacao")]
        public ActionResult SetEquipeValidacao(int id, [FromBody] JObject data)
        {
            var superiorDireto = data.Value<string>("superiorDireto");
            if (String.IsNullOrWhiteSpace(superiorDireto))
            {
                return BadRequest("Superior Direto não informado!");
            }
            if (!Service.DemandaExist(id))
            {
                return NotFound();
            }

            Service.SetSuperiorDireto(id, superiorDireto);

            return Ok();
        }
        [HttpGet("{id}/EquipeValidacao")]
        public ActionResult<object> GetEquipeValidacao(int id)
        {
            return new
            {
                superiorDireto = Service.GetSuperiorDireto(id)
            };

        }

        [HttpPut("{id}/ProximaEtapa")]
        public ActionResult AlterarStatusDemanda(int id)
        {
            try
            {
                Service.ProximaEtapa(id, this.userId());
            }
            catch (System.Exception)
            {

                throw;
            }
            return Ok();
        }

        [HttpPut("{id:int}/Aprovar")]
        public ActionResult AprovarDemanda(int id)
        {
            return Ok();
        }
        [HttpPut("{id:int}/Reprovar")]
        public ActionResult ReprovarDemanda(int id)
        {
            return Ok();
        }
        [HttpGet("{id:int}/File/")]
        public ActionResult<object> GetDemandaFiles(int id)
        {
            return Service.GetDemandaFiles(id);
        }
        [HttpGet("{id:int}/File/{file_id:int}")]
        public ActionResult<object> GetDemandaFile(int id, int file_id)
        {
            var file = Service.GetDemandaFile(id, file_id);
            if (file != null && System.IO.File.Exists(file.Path))
            {
                return PhysicalFile(file.Path, file.ContentType, file.Name);
            }
            return NotFound();
        }

        [HttpGet("{id:int}/Form/{form}")]
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
        public ActionResult<object> SalvarDemandaFormValue(int id, string form, [FromBody] JObject data)
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



        [HttpGet("{id:int}/Form/{form}/Pdf")]
        public ActionResult<object> GetDemandaPDF(int id, string form)
        {
            var filename = Service.GetDemandaFormPdfFilename(id, form);
            if (System.IO.File.Exists(filename))
            {

                var response = PhysicalFile(filename, "application/pdf");
                if (Request.Query["dl"] == "1")
                {
                    response.FileDownloadName = String.Format("demanda-{0}-{1}.pdf", id, form);
                }
                return response;
            }
            else
            {
                return NotFound(filename);
            }

        }
        [HttpGet("{id:int}/Form/{form}/Debug")]
        public ActionResult<object> GetDemandaTeste(int id, string form)
        {
            var doc = Service.GetDemandaFormHTML(id, form);
            if (doc != null)
            {
                return Content(doc, "text/html");
            }
            return NotFound();

        }


    }
}