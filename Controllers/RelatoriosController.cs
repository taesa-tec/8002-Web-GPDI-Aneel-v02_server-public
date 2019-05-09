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
        private RelatorioAtividadeService _relatorioAtividadeService;

        public RelatoriosController(
            RelatorioEmpresaService relatorioEmpresaService,
            RelatorioEtapaService relatorioEtapaService,
            RelatorioAtividadeService relatorioAtividadeService)
        {
            _relatorioEmpresaService = relatorioEmpresaService;
            _relatorioEtapaService = relatorioEtapaService;
            _relatorioAtividadeService = relatorioAtividadeService;
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
        [HttpGet("{projetoId}/ExtratoREFP")]
        public RelatorioEmpresa GetB(int projetoId)
        {
            return _relatorioEmpresaService.ExtratoREFP2(projetoId);
        }
        [HttpGet("{projetoId}/ExtratoAtividades")]
        public RelatorioAtividade GetC(int projetoId)
        {
            return _relatorioAtividadeService.ExtratoFinanceiro(projetoId);
        }
        
        [HttpGet("{projetoId}/ExtratoEmpresas/exportar")]
        public FileResult Download(int projetoId)  
        {  
             var data = _relatorioEmpresaService.FormatRelatorioCsv(_relatorioEmpresaService.ExtratoFinanceiro(projetoId));
             MemoryStream relatorio = _relatorioEmpresaService.ExportarRelatorio(data, "RelatorioEmpresa");
            if (relatorio==null)
                return null;
         
            return new FileContentResult(relatorio.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet)
                { 
                    FileDownloadName = "relatorio.csv"
                };
        }
        [HttpGet("{projetoId}/ExtratoREFP/exportar")]
        public FileResult DownloadA(int projetoId)  
        {  
             var data = _relatorioEmpresaService.FormatRelatorioCsv(_relatorioEmpresaService.ExtratoREFP2(projetoId));
             MemoryStream relatorio = _relatorioEmpresaService.ExportarRelatorio(data, "RelatorioREFP");
            if (relatorio==null)
                return null;
         
            return new FileContentResult(relatorio.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet)
                { 
                    FileDownloadName = "relatorioRefp.csv"
                };
        }
    }
}