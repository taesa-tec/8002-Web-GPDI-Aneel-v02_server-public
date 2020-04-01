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
    public partial class DemandaController
    {
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
            return sistemaService.GetOption<object>(string.Format("form-value-{0}", key));
        }

        [HttpPost("Forms/{key}/Values")]
        public ActionResult SetFormValues(string key, [FromBody] object data)
        {
            sistemaService.SetOption(string.Format("form-value-{0}", key), data);
            return Ok();
        }
    }
}