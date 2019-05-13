using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RelatoriosController : ControllerBase {
        private RelatorioEmpresaService _relatorioEmpresaService;
        private RelatorioEtapaService _relatorioEtapaService;
        private RelatorioAtividadeService _relatorioAtividadeService;

        public RelatoriosController(
            RelatorioEmpresaService relatorioEmpresaService,
            RelatorioEtapaService relatorioEtapaService,
            RelatorioAtividadeService relatorioAtividadeService) {
            _relatorioEmpresaService = relatorioEmpresaService;
            _relatorioEtapaService = relatorioEtapaService;
            _relatorioAtividadeService = relatorioAtividadeService;
        }

        [HttpGet("{projetoId}/ExtratoEmpresas")]
        public OrcamentoEmpresas Get(int projetoId) {
            return _relatorioEmpresaService.orcamentoEmpresas(projetoId);
        }

        [HttpGet("{projetoId}/ExtratoEtapas")]
        public RelatorioEtapa GetA(int projetoId) {
            return _relatorioEtapaService.ExtratoFinanceiro(projetoId);
        }

        [HttpGet("{projetoId}/ExtratoREFP")]
        public ExtratoEmpresas GetB(int projetoId) {
            return _relatorioEmpresaService.extratoEmpresas(projetoId);
        }

        [HttpGet("{projetoId}/ExtratoAtividades")]
        public RelatorioAtividade GetC(int projetoId) {
            return _relatorioAtividadeService.ExtratoFinanceiro(projetoId);
        }

        [HttpGet("{projetoId}/ExtratoEmpresas/exportar_old")]
        public FileResult Download(int projetoId) {
            var data = _relatorioEmpresaService.FormatRelatorioCsv(_relatorioEmpresaService.orcamentoEmpresas(projetoId));
            MemoryStream relatorio = _relatorioEmpresaService.ExportarRelatorio(data, "RelatorioEmpresa");
            if (relatorio == null)
                return null;

            return new FileContentResult(relatorio.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet) {
                FileDownloadName = "relatorio.csv"
            };
        }

        [HttpGet("{projetoId}/ExtratoEmpresas/exportar")]
        public ActionResult DownloadXLS(int projetoId) {
            MemoryStream stream = new MemoryStream();
            var workbook = this._relatorioEmpresaService.gerarXLSOrcamento(projetoId);
            workbook.SaveAs(stream);
            stream.Position = 0;
            Response.Headers.Add("Content-Disposition", "inline; filename=\"projeto-"+projetoId + "-relatorio.xlsx\"");
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //[HttpGet("{projetoId}/ExtratoREFP/exportar")]
        //public FileResult DownloadA(int projetoId) {
        //    var data = _relatorioEmpresaService.FormatRelatorioCsv(_relatorioEmpresaService.ExtratoREFP2(projetoId));
        //    MemoryStream relatorio = _relatorioEmpresaService.ExportarRelatorio(data, "RelatorioREFP");
        //    if (relatorio == null)
        //        return null;

        //    return new FileContentResult(relatorio.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet) {
        //        FileDownloadName = "relatorioRefp.csv"
        //    };
        //}
    }
}