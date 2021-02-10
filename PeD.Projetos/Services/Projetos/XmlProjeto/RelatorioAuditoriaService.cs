using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Xmls;
using PeD.Data;

namespace PeD.Services.Projetos.XmlProjeto {
    public class XmlRelatorioAuditoriaService : IXmlService<XmlRelatorioAuditoria> {
        private GestorDbContext _context;
        public XmlRelatorioAuditoriaService(GestorDbContext context) {
            _context = context;
        }
        public Resultado ValidaXml(int ProjetoId) {
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Include("Empresas.Estado")
                    .Include("Etapas")
                    .Include("AlocacoesRh.RecursoHumano")
                    .Include("AlocacoesRm.RecursoMaterial")
                    .Include("Empresas.CatalogEmpresa")
                    .Include("RelatorioFinal.Uploads")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();


            if (projeto.RelatorioFinal == null) {
                resultado.Inconsistencias.Add("O projeto não tem Relatório Final Cadastrado");
            }
            else if(projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAuditoria").FirstOrDefault() == null) {
                resultado.Inconsistencias.Add("O projeto não tem Arquivo PDF do Relatório Final cadastrado");
            }

            return resultado;
        }
        public XmlRelatorioAuditoria GerarXml(int ProjetoId, string Versao, string UserId) {
            XmlRelatorioAuditoria relatorio = new XmlRelatorioAuditoria();
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Include("Empresas.Estado")
                    .Include("Etapas")
                    .Include("AlocacoesRh.RecursoHumano")
                    .Include("AlocacoesRm.RecursoMaterial")
                    .Include("Empresas.CatalogEmpresa")
                    .Include("RelatorioFinal.Uploads")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();

            var registros = _context.RegistrosFinanceiros
                            .Include("RecursoHumano")
                            .Include("RecursoMaterial")
                            .Where(p => p.ProjetoId == ProjetoId)
                            .Where(p => p.StatusValor == "Aprovado")
                            .ToList();

            int?[] rhIds = registros.Where(r => r.RecursoHumano != null).Select(r => r.RecursoHumanoId).ToArray();
            int?[] rmIds = registros.Where(r => r.RecursoMaterial != null).Select(r => r.RecursoMaterialId).ToArray();

            decimal? TotalRh = registros.Where(r => r.QtdHrs != null && r.RecursoHumano != null).Sum(r => r.QtdHrs * r.RecursoHumano.ValorHora);
            decimal? TotalRm = registros.Where(r => r.QtdItens != null && r.RecursoMaterial != null).Sum(r => r.QtdItens * r.RecursoMaterial.ValorUnitario);


            // Validar o Relatório Final pode não ter sido criado
            relatorio.PD_RelAuditoriaPED = new PD_RelAuditoriaPED {
                CodProjeto = projeto.Codigo,
                ArquivoPDF = projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAuditoria").FirstOrDefault().NomeArquivo,
                CustoTotal = (TotalRh + TotalRm).ToString()
            };

            var EmpresasFinanciadoras = projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                .ToList();

            // RecursoEmpresa
            var ListRecursoEmpresa = new List<RA_RecursoEmpresa>();
            foreach (Empresa empresa in EmpresasFinanciadoras) {
                var DestRecursosExec = new List<RA_DestRecursosExec>();
                // RH
                foreach (var rh in projeto.AlocacoesRh
                    .Where(p => p.Empresa.ClassificacaoValor == "Executora")
                    .Where(p => p.Empresa == empresa)
                    .Where(p => rhIds.Contains(p.RecursoHumano.Id))
                    .GroupBy(p => p.Empresa)
                    .ToList()) {
                    decimal custo = 0;

                    var CustoCatContabilExec = new List<CustoCatContabilExec>();

                    foreach (var rh0 in rh) {
                        custo += rh0.RecursoHumano.ValorHora * (rh0.HrsMes1 + rh0.HrsMes2 + rh0.HrsMes3
                            + rh0.HrsMes4 + rh0.HrsMes5 + rh0.HrsMes6);

                    }
                    CustoCatContabilExec.Add(new CustoCatContabilExec {
                        CatContabil = "RH",
                        CustoExec = custo.ToString()
                    });
                    DestRecursosExec.Add(new RA_DestRecursosExec {
                        CNPJExec = rh.First().Empresa.Cnpj,
                        CustoCatContabil = CustoCatContabilExec
                    });
                }
                // RM
                foreach (var rm in projeto.AlocacoesRm
                    .Where(p => p.EmpresaRecebedora.ClassificacaoValor == "Executora")
                    .Where(p => p.EmpresaFinanciadora == empresa)
                    .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                    .GroupBy(p => p.EmpresaRecebedora)
                    .ToList()) {
                    var CustoCatContabilExec = new List<CustoCatContabilExec>();
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil)) {
                        decimal custo = 0;
                        foreach (var rm1 in rm0) {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        CustoCatContabilExec.Add(new CustoCatContabilExec {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                            CustoExec = custo.ToString()
                        });
                    }
                    DestRecursosExec.Add(new RA_DestRecursosExec {
                        CNPJExec = rm.First().EmpresaRecebedora.Cnpj,
                        CustoCatContabil = CustoCatContabilExec
                    });
                }

                var DestRecursosEmp = new List<RA_DestRecursosEmp>();
                // RH
                foreach (var rh in projeto.AlocacoesRh
                    .Where(p => p.Empresa.ClassificacaoValor != "Executora")
                    .Where(p => p.Empresa == empresa)
                    .Where(p => rhIds.Contains(p.RecursoHumano.Id))
                    .GroupBy(p => p.Empresa)
                    .ToList()) {
                    decimal custo = 0;

                    var CustoCatContabilEmp = new List<CustoCatContabilEmp>();

                    foreach (var rh0 in rh) {
                        custo += rh0.RecursoHumano.ValorHora * (rh0.HrsMes1 + rh0.HrsMes2 + rh0.HrsMes3
                            + rh0.HrsMes4 + rh0.HrsMes5 + rh0.HrsMes6);

                    }
                    CustoCatContabilEmp.Add(new CustoCatContabilEmp {
                        CatContabil = "RH",
                        CustoEmp = custo.ToString()
                    });
                    DestRecursosEmp.Add(new RA_DestRecursosEmp {
                        CustoCatContabil = CustoCatContabilEmp
                    });
                }
                // RM
                foreach (var rm in projeto.AlocacoesRm
                        .Where(p => p.EmpresaRecebedora == empresa)
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList()) {
                    var CustoCatContabilEmp = new List<CustoCatContabilEmp>();
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil)) {
                        decimal custo = 0;
                        foreach (var rm1 in rm0) {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        CustoCatContabilEmp.Add(new CustoCatContabilEmp {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                            CustoEmp = custo.ToString()
                        });
                    }
                    DestRecursosEmp.Add(new RA_DestRecursosEmp {
                        CustoCatContabil = CustoCatContabilEmp
                    });
                }

                ListRecursoEmpresa.Add(new RA_RecursoEmpresa {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    DestRecursosExc = DestRecursosExec,
                    DestRecursosEmp = DestRecursosEmp
                });
            }
            relatorio.PD_RelAuditoriaPED.RecursoEmpresa = ListRecursoEmpresa;

            //RecursoParceira
            var ListRecursoParceira = new List<RA_RecursoParceira>();
            foreach (Empresa empresa in projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Parceira")
                .ToList()) {
                var DestRecursosExec = new List<RA_DestRecursosExec>();

                //RH
                foreach (var rh in projeto.AlocacoesRh
                        .Where(p => p.Empresa == empresa)
                        .Where(p => p.Empresa.ClassificacaoValor == "Executora")
                        .Where(p => rhIds.Contains(p.RecursoHumano.Id))
                        .GroupBy(p => p.Empresa)
                        .ToList()) {
                    decimal custo = 0;

                    var CustoCatContabilExec = new List<CustoCatContabilExec>();

                    foreach (var rh0 in rh) {
                        custo += rh0.RecursoHumano.ValorHora * (rh0.HrsMes1 + rh0.HrsMes2 + rh0.HrsMes3
                            + rh0.HrsMes4 + rh0.HrsMes5 + rh0.HrsMes6);

                    }
                    CustoCatContabilExec.Add(new CustoCatContabilExec {
                        CatContabil = "RH",
                        CustoExec = custo.ToString()
                    });
                    DestRecursosExec.Add(new RA_DestRecursosExec {
                        CNPJExec = rh.First().Empresa.Cnpj,
                        CustoCatContabil = CustoCatContabilExec
                    });
                }
                // RM
                foreach (var rm in projeto.AlocacoesRm
                    .Where(p => p.EmpresaRecebedora.ClassificacaoValor == "Executora")
                    .Where(p => p.EmpresaFinanciadora == empresa)
                    .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                    .GroupBy(p => p.EmpresaRecebedora)
                    .ToList()) {
                    var CustoCatContabilExec = new List<CustoCatContabilExec>();

                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil)) {
                        decimal custo = 0;
                        foreach (var rm1 in rm0) {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        CustoCatContabilExec.Add(new CustoCatContabilExec {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                            CustoExec = custo.ToString()
                        });
                    }
                    DestRecursosExec.Add(new RA_DestRecursosExec {
                        CNPJExec = rm.First().EmpresaRecebedora.Cnpj,
                        CustoCatContabil = CustoCatContabilExec
                    });
                }

                ListRecursoParceira.Add(new RA_RecursoParceira {
                    CNPJParc = empresa.Cnpj,
                    DestRecursosExec = DestRecursosExec
                });
            }
            relatorio.PD_RelAuditoriaPED.RecursoParceira = ListRecursoParceira;
            return relatorio;
        }
    }
}
