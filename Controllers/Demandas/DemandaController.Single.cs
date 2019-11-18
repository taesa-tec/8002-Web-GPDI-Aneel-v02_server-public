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
using APIGestor.Exceptions.Demandas;

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

        [HttpHead("{id:int}")]
        public ActionResult HasAccess(int id)
        {
            if (Service.DemandaExist(id))
            {
                if (Service.UserCanAccess(id, this.userId()))
                    return Ok();
                else
                    return Forbid();
            }
            return NotFound();
        }

        [HttpGet("{id:int}")]
        public ActionResult<Demanda> GetById(int id)
        {
            if (Service.UserCanAccess(id, this.userId()))
                return Service.GetById(id);
            return NotFound();
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
        [HttpPut("{id}/Revisor")]
        public ActionResult<Demanda> SetRevisor(int id, [FromBody] JObject data)
        {

            var revisorId = data.Value<string>("revisorId");
            if (String.IsNullOrWhiteSpace(revisorId))
            {
                return BadRequest("Rivisor não informado!");
            }
            if (!Service.DemandaExist(id))
            {
                return NotFound();
            }
            if (sistemaService.GetEquipePeD().Coordenador == this.userId())
            {
                try
                {
                    Service.ProximaEtapa(id, this.userId(), revisorId);
                }
                catch (DemandaException exception)
                {
                    return BadRequest(exception);
                }
                catch (System.Exception)
                {
                    throw;
                }
                return GetById(id);
            }
            return Forbid();
        }

        [HttpPut("{id}/ProximaEtapa")]
        public ActionResult<Demanda> AlterarStatusDemanda(int id, [FromBody] JObject data)
        {
            try
            {
                var comentario = data.Value<string>("comentario");
                Service.ProximaEtapa(id, this.userId());
                if (!String.IsNullOrWhiteSpace(comentario))
                {
                    Service.AddComentario(id, comentario, this.userId());
                }
            }
            catch (DemandaException exception)
            {
                return BadRequest(exception);
            }
            catch (System.Exception)
            {
                throw;
            }
            return GetById(id);
        }

        [HttpPut("{id:int}/Reiniciar")]
        public ActionResult<Demanda> Reiniciar(int id, [FromBody] JObject data)
        {
            if (!Service.DemandaExist(id))
                return NotFound();

            var motivo = data.Value<string>("motivo");

            try
            {
                if (String.IsNullOrWhiteSpace(motivo))
                {
                    motivo = "Motivo não informado";
                }
                Service.ReprovarReiniciar(id, this.userId());
                Service.AddComentario(id, motivo, this.userId());


            }
            catch (DemandaException exception)
            {
                return BadRequest(exception);
            }
            catch (System.Exception)
            {

                throw;
            }
            return GetById(id);
        }
        [HttpPut("{id:int}/ReprovarPermanente")]
        public ActionResult<Demanda> Finalizar(int id, [FromBody] JObject data)
        {
            if (!Service.DemandaExist(id))
                return NotFound();

            var motivo = data.Value<string>("motivo");



            try
            {
                Service.ReprovarPermanente(id, this.userId());
                Service.AddComentario(id, motivo, this.userId());
            }
            catch (DemandaException exception)
            {
                return BadRequest(exception);
            }
            catch (System.Exception)
            {

                throw;
            }
            return CreatedAtAction(nameof(GetById), new { id });
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

                var name = String.Format("demanda-{0}-{1}.pdf", id, form);
                var response = PhysicalFile(filename, "application/pdf", name);
                if (Request.Query["dl"] == "1")
                {
                    response.FileDownloadName = name;
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