using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

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
    }
}