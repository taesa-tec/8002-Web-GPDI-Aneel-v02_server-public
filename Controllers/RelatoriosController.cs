using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.IO;
using Newtonsoft.Json;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RelatoriosController : ControllerBase
    {
        private RelatorioEmpresaService _relatorioEmpresaService;
        private RelatorioEtapaService _relatorioEtapaService;

        public RelatoriosController(
            RelatorioEmpresaService relatorioEmpresaService,
            RelatorioEtapaService relatorioEtapaService)
        {
            _relatorioEmpresaService = relatorioEmpresaService;
            _relatorioEtapaService = relatorioEtapaService;
        }

        [HttpGet("{projetoId}/ExtratoEmpresas")]
        public RelatorioEmpresa Get(int projetoId)
        {
            return _relatorioEmpresaService.ExtratoFinanceiro(projetoId);
        }
        [HttpGet("{projetoId}/ExtratoEtapas")]
        public RelatorioEtapa GetA(int projetoId)
        {
            return _relatorioEtapaService.ExtratoFinanceiro(projetoId);
        }
        
        [HttpGet("{projetoId}/ExtratoEmpresas/exportar")]
        public FileResult Download(int projetoId)  
        {  
            var relatorio = _relatorioEmpresaService.ExportarRelatorio(projetoId);
            if (relatorio==null)
                return null;

            var mr = new MemoryStream();
            var tw = new StreamWriter(mr);
            tw.Write(JsonConvert.SerializeObject(relatorio));
            tw.Flush();
         
            return new FileContentResult(mr.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet)
                { 
                    FileDownloadName = "relatorio.json"
                };
            //return File(mr, System.Net.Mime.MediaTypeNames.Application.Octet, "relatorio.json");
        }
    }
}