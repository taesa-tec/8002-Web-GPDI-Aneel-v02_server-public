using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models.Projetos;
using PeD.Services.Projetos.Relatorios;

namespace PeD.Controllers.Projetos {
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
        public RelatorioAtividades GetC(int projetoId) {
            return _relatorioAtividadeService.ExtratoFinanceiro(projetoId);

            // return _relatorioAtividadeService.ExtratoAtividades(projetoId);
        }

        [HttpGet("{projetoId}/ExtratoEmpresas/exportar")]
        public ActionResult DownloadOrcamento(int projetoId) {
            MemoryStream stream = new MemoryStream();
            var workbook = this._relatorioEmpresaService.gerarXLSOrcamento(projetoId);
            workbook.SaveAs(stream);
            stream.Position = 0;
            Response.Headers.Add("Content-Disposition", "inline; filename=\"projeto-" + projetoId + "-relatorio.xlsx\"");
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpGet("{projetoId}/ExtratoREFP/exportar")]
        public ActionResult DownloadExtrato(int projetoId) {
            MemoryStream stream = new MemoryStream();
            var workbook = this._relatorioEmpresaService.gerarXLSExtrato(projetoId);
            workbook.SaveAs(stream);
            stream.Position = 0;
            Response.Headers.Add("Content-Disposition", "inline; filename=\"projeto-" + projetoId + "-relatorio.xlsx\"");
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}