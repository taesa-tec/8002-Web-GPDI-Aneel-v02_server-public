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
        [HttpGet("{projetoId}/XmlProjetoPed/{versao}")]
        public Resultado Get(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.GerarXmlProjetoPed(projetoId, versao, userId);
        }

        [HttpGet("{projetoId}/XmlInteresseExecucao/{versao}")]
        public Resultado GetA(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.GerarXmlInteresseExec(projetoId, versao, userId);
        }
        
        [HttpGet("{projetoId}/XmlInicioExecucao/{versao}")]
        public Resultado GetB(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.GerarXmlInicioExec(projetoId, versao, userId);
        }
        [HttpGet("{projetoId}/XmlProrrogacao/{versao}")]
        public Resultado GetC(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.GerarXmlProrrogacao(projetoId, versao, userId);
        }
        [HttpGet("{projetoId}/XmlRelatorioFinal/{versao}")]
        public Resultado GetD(int projetoId, string versao)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.GerarXmlRelatorioFinal(projetoId, versao, userId);
        }

        [HttpGet("{projetoId}/ObterXmls")]
        public IEnumerable<Upload> GetA(int projetoId)
        {
            return _service.ObterXmls(projetoId);
        }


        [HttpGet("{projetoId}/XmlProjetoPed/ValidaDados")]
        public Resultado Get(int projetoId)
        {
            Projeto projeto = _service.obterProjeto(projetoId);
            if (projeto==null){
                var resultado = new Resultado();
                resultado.Inconsistencias.Add("Projeto não localizado");
                return resultado;
            }else{
                return _service.ValidaXmlProjetoPed(projeto);
            }
        }
    }
}