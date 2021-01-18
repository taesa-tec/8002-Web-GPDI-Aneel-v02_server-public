using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using PeD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Xmls;
using PeD.Services.Projetos;

namespace PeD.Controllers.Projetos {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class GeradorXmlController : ControllerBase {
        private GeradorXmlService _service;

        public GeradorXmlController( GeradorXmlService service ) {
            _service = service;
        }

        // [HttpGet("{projetoId}/XmlInteresseExecucao/{versao}")]
        // public ContentResult GetA(int projetoId, string versao)
        // {
        //     var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        //     return new ContentResult
        //     {
        //         Content = _service.GerarXmlInteresseExec(projetoId, versao, userId).ToString(),
        //         ContentType = "text/xml",
        //         StatusCode = 200
        //     };
        // }
        [HttpGet("{projetoId}/Xml/{xmlTipo}/{versao}")]
        public ActionResult<Resultado> Get( int projetoId, XmlTipo xmlTipo, string versao ) {
            Resultado r = new Resultado();
            try {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

                return _service.GerarXml(projetoId, xmlTipo, versao, userId);
            }
            catch(System.Exception e) {
                // Substituir depois só por uma mensagem de erro genérica
                r.Inconsistencias.Add(e.Message);
                r.Inconsistencias.Add(e.StackTrace);
                return r;
            }

        }

        [HttpGet("{projetoId}/Xml/{xmlTipo}/ValidaDados")]
        public ActionResult<Resultado> GetA( int projetoId, XmlTipo xmlTipo ) {
            return _service.ValidaDados(projetoId, xmlTipo);
        }

        [HttpGet("{projetoId}/ObterXmls")]
        public IEnumerable<Upload> GetB( int projetoId ) {
            return _service.ObterXmls(projetoId);
        }
    }
}