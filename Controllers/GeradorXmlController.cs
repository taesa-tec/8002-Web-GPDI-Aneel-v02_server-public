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

        [HttpGet("{projetoId}/XmlProjetoPed/{versao}")]
        public ContentResult Get(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return new ContentResult
            {
                Content = _service.GerarXmlProjetoPed(projetoId, versao, userId).ToString(),
                ContentType = "text/xml",
                StatusCode = 200
            };
        }

        [HttpGet("{projetoId}/XmlInteresseExecucao/{versao}")]
        public ContentResult GetA(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return new ContentResult
            {
                Content = _service.GerarXmlInteresseExec(projetoId, versao, userId).ToString(),
                ContentType = "text/xml",
                StatusCode = 200
            };
        }
        
        [HttpGet("{projetoId}/XmlInicioExecucao/{versao}")]
        public ContentResult GetB(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return new ContentResult
            {
                Content = _service.GerarXmlInicioExec(projetoId, versao, userId).ToString(),
                ContentType = "text/xml",
                StatusCode = 200
            };
        }

        [HttpGet("{projetoId}/ObterXmls")]
        public IEnumerable<Upload> GetA(int projetoId)
        {
            return _service.ObterXmls(projetoId);
        }
        // [HttpGet("{projetoId}/XmlInteresse/{versao}")]
        // public ActionResult<Tema> Get(int projetoId, string versao)
        // {
        //     return _service.GerarXmlInteresse(projetoId, versao);
        // }

    }
}