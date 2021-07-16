using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Models.Projetos.Xml.Auditoria;
using PeD.Data;

namespace PeD.Services.Projetos.Xml
{
    public class RelatorioAuditoriaService
    {
        private GestorDbContext _context;

        public RelatorioAuditoriaService(GestorDbContext context)
        {
            _context = context;
        }

        public Auditoria Auditoria(int projetoId)
        {
            return new Auditoria()
            {
                PD_RelAuditoriaPED = PD_RelAuditoriaPED(projetoId)
            };
        }

        protected PD_RelAuditoriaPED PD_RelAuditoriaPED(int projetoId)
        {
            var projeto = _context.Set<Projeto>().FirstOrDefault(p => p.Id == projetoId) ??
                          throw new Exception($"Projeto não encontrado!");

            var relatorio = _context.Set<RelatorioFinal>()
                .Include(r => r.AuditoriaRelatorioArquivo)
                .FirstOrDefault(r => r.ProjetoId == projetoId);
            var registros = _context.Set<RegistroFinanceiroInfo>()
                .Where(rf => rf.ProjetoId == projetoId && rf.Status == StatusRegistro.Aprovado).ToList();
            var custoTotal = registros.Sum(rf => rf.Custo);

            return new PD_RelAuditoriaPED()
            {
                CodProjeto = projeto.Codigo,
                CustoTotal = custoTotal,
                ArquivoPDF = relatorio?.AuditoriaRelatorioArquivo.FileName ??
                             throw new Exception("Arquivo pdf de auditoria não encontrado!"),
                RecursoEmpresa = RecursoEmpresas(registros),
                RecursoParceira = RecursoParceiras(registros)
            };
        }

        protected List<RecursoEmpresa> RecursoEmpresas(List<RegistroFinanceiroInfo> registros)
        {
            var financiadores = registros.Where(r => r.FinanciadoraFuncao != Funcao.Executora ).GroupBy(r => r.FinanciadoraId);
            return financiadores.Select(f => new RecursoEmpresa()
            {
                CodEmpresa = f.First().FinanciadoraCodigo,
                DestRecursosExec = DestRecursosExec(f),
                DestRecursosEmp = DestRecursosEmp(f)
            }).ToList();
        }

        protected List<RecursoParceira> RecursoParceiras(List<RegistroFinanceiroInfo> registros)
        {
            var parceiras = registros.Where(r => r.FinanciadoraFuncao == Funcao.Executora)
                .GroupBy(r => r.FinanciadoraId);

            return parceiras.Select(rf => new RecursoParceira()
            {
                CNPJParc = rf.First().CNPJParc,
                DestRecursosExec = DestRecursosExec(rf)
            }).ToList();
        }

        protected List<DestRecursosExec> DestRecursosExec(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return registros
                .GroupBy(rf => rf.CNPJExec)
                .Select(dre => new DestRecursosExec()
                {
                    CNPJExec = dre.Key,
                    CustoCatContabil = CustoCatContabil(dre)
                })
                .ToList();
        }

        protected DestRecursosEmp DestRecursosEmp(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return new DestRecursosEmp()
            {
                CustoCatContabil = CustoCatContabil(registros)
            };
        }

        protected List<CustoCatContabil> CustoCatContabil(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return registros.GroupBy(rf => rf.CategoriaContabilCodigo)
                .Select(rf => new CustoCatContabil()
                {
                    CatContabil = rf.Key,
                    CustoExec = rf.Sum(c => c.Custo)
                })
                .ToList();
        }
    }
}