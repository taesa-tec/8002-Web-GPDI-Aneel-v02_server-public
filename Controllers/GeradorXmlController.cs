using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class GeradorXmlController : ControllerBase
    {
        private GeradorXmlService _service;

        public GeradorXmlController(GeradorXmlService service)
        {
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
        public Resultado Get(int projetoId, XmlTipo xmlTipo, string versao)
        {
            Resultado r = new Resultado();
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

                return _service.GerarXml(projetoId, xmlTipo, versao, userId);
            }
            catch (System.Exception e)
            {
                r.Inconsistencias.Add(e.Message);
                return r;
            }
            
        }

        [HttpGet("{projetoId}/Xml/{xmlTipo}/ValidaDados")]
        public Resultado GetA(int projetoId, XmlTipo xmlTipo)
        {
           return _service.ValidaDados(projetoId, xmlTipo);
        }

        [HttpGet("{projetoId}/ObterXmls")]
        public IEnumerable<Upload> GetB(int projetoId)
        {
            return _service.ObterXmls(projetoId);
        }
    }
}