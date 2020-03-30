using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Hosting;

namespace APIGestor.Business {
    public class XmlProjetoGestaoService : IXmlService<XmlProjetoGestao> {
        private GestorDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        public XmlProjetoGestaoService( GestorDbContext context, IWebHostEnvironment hostingEnvironment ) {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public Resultado ValidaXml( int ProjetoId ) {
            Projeto projeto = obterProjeto(ProjetoId);
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            if(projeto.Etapas.Count() == 0)
                resultado.Inconsistencias.Add("Etapas do projeto não preenchida");
            if(projeto.Atividades.DedicacaoHorario == null)
                resultado.Inconsistencias.Add("Atividades do projeto não preenchidas");
            if(projeto.RecursosHumanos.Where(p => p.GerenteProjeto == true).Count() == 0)
                resultado.Inconsistencias.Add("O projeto não tem nenhum gerente cadastrado");

            return resultado;
        }
        public Projeto obterProjeto( int Id ) {
            return _context.Projetos
                         .Include("CatalogEmpresa")
                         .Include("Etapas")
                         .Include("Atividades")
                         .Include("Empresas.Estado")
                         .Include("Empresas.CatalogEmpresa")
                         .Include("RecursosHumanos")
                         .Include("AlocacoesRh.RecursoHumano")
                         .Include("AlocacoesRm.RecursoMaterial.Atividade")
                         .Include("AlocacoesRm.RecursoMaterial.CategoriaContabilGestao")
                         .Include("RelatorioFinal.Uploads")
                         .Where(p => p.Id == Id)
                         .FirstOrDefault();
        }

        protected Dictionary<string, List<AlocacaoRm>> groupAlocacoesRmByCategory( List<AlocacaoRm> AlocacoesRm ) {
            var group = (from alocacao in AlocacoesRm
                         group alocacao by alocacao.RecursoMaterial.CategoriaContabilValor into cat
                         select new { categoria = cat.Key, alocacoes = cat.ToList() });

            return group.ToDictionary(item => item.categoria, item => item.alocacoes);
        }

        protected void getCustoEmpresa( Projeto projeto, Empresa empresa, out List<GstCustoCatContabil> CustoCatContabilEmp ) {

            CustoCatContabilEmp = new List<GstCustoCatContabil>();

            #region Custo com a própria empresa
            var custoRHEmpresa = projeto.AlocacoesRh.Where(al => al.EmpresaId == empresa.Id && al.RecursoHumano.EmpresaId == empresa.Id).Sum(al => al.HrsTotais * al.RecursoHumano.ValorHora);

            var custoRMEmpresa = projeto.AlocacoesRm.Where(al => al.EmpresaFinanciadoraId == empresa.Id)
                .GroupBy(al => al.RecursoMaterial.CategoriaContabilGestao.Valor)
                .Select(a => new GstCustoCatContabil {
                    CategoriaContabil = a.Key,
                    CustoEmpresa = a.Sum(b => b.Qtd * b.RecursoMaterial.ValorUnitario).ToString()
                }).Where(cc => decimal.Parse(cc.CustoEmpresa) > 0).ToList();

            if(custoRHEmpresa > 0)
                CustoCatContabilEmp.Add(new GstCustoCatContabil { CategoriaContabil = "RH", CustoEmpresa = custoRHEmpresa.ToString() });

            CustoCatContabilEmp.AddRange(custoRMEmpresa);
            #endregion


        }

        public XmlProjetoGestao GerarXml( int ProjetoId, string Versao, string UserId ) {
            XmlProjetoGestao relatorio = new XmlProjetoGestao();
            Projeto projeto = obterProjeto(ProjetoId);

            var EmpresasFinanciadoras = projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                .ToList();

            // EMPRESAS
            var EmpresaList = new List<GstEmpresa>();


            // CustosContabeis
            var GstCustoCatContabilList = new List<GstCustoCatContabil>();


            foreach(Empresa empresa in EmpresasFinanciadoras) {
                var equipeList = new List<GstEquipeGestao>();
                foreach(RecursoHumano rh in projeto.RecursosHumanos
                    .Where(p => p.CPF != null)
                    .Where(p => p.Empresa == empresa)
                    .ToList()) {
                    equipeList.Add(new GstEquipeGestao {
                        NomeMbEqEmp = rh.NomeCompleto,
                        CpfMbEqEmp = rh.CPF
                    });
                }

                var GstCustoCatContabil = new List<GstCustoCatContabil>();

                this.getCustoEmpresa(projeto, empresa, out GstCustoCatContabil);

                EmpresaList.Add(new GstEmpresa {
                    TipoEmpresa = empresa.ClassificacaoValor,
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    Equipe = new GstEquipe {
                        EquipeGestao = equipeList
                    },
                    CustosContabeis = new GstCustosContabeis {
                        CustoCatContabil = GstCustoCatContabil
                    }
                });
            }





            // Atividades
            var AtividadesList = new List<GstAtividade>();
            //RH
            if(projeto.AlocacoesRh.Count > 0) {
                decimal custo = projeto.AlocacoesRh.Sum(a => {
                    return a.HrsTotais * a.RecursoHumano.ValorHora;
                });
                AtividadesList.Add(new GstAtividade {
                    TipoAtividade = "HH",
                    DescAtividade = projeto.Atividades.DedicacaoHorario,
                    CustoAtividade = custo.ToString()
                });
            }
            // RM
            foreach(var rm in projeto.AlocacoesRm
                .GroupBy(p => p.RecursoMaterial.Atividade.Valor)
                .ToList()) {
                decimal custo = 0;
                foreach(var rm1 in rm) {
                    custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                }
                string _descAtividade = null;
                switch(rm.First().RecursoMaterial.Atividade.Valor) {
                    case "HH":
                        _descAtividade = projeto.Atividades.DedicacaoHorario;
                        break;
                    case "EC":
                        _descAtividade = projeto.Atividades.ParticipacaoMembros;
                        break;
                    case "FG":
                        _descAtividade = projeto.Atividades.DesenvFerramenta;
                        break;
                    case "PP":
                        _descAtividade = projeto.Atividades.ProspTecnologica;
                        break;
                    case "RP":
                        _descAtividade = projeto.Atividades.DivulgacaoResultados;
                        break;
                    case "AP":
                        _descAtividade = projeto.Atividades.ParticipacaoTecnicos;
                        break;
                    case "BA":
                        _descAtividade = projeto.Atividades.BuscaAnterioridade;
                        break;
                    case "CA":
                        _descAtividade = projeto.Atividades.ContratacaoAuditoria;
                        break;
                    case "AC":
                        _descAtividade = projeto.Atividades.ApoioCitenel;
                        break;
                }
                AtividadesList.Add(new GstAtividade {
                    TipoAtividade = rm.First().RecursoMaterial.Atividade.Valor,
                    DescAtividade = _descAtividade,
                    CustoAtividade = custo.ToString()
                });
            }

            // PD_ProjetoGestao
            var gerente = projeto.RecursosHumanos.Where(p => p.GerenteProjeto == true);
            relatorio.PD_ProjetoGestao = new PD_ProjetoGestao {
                Empresas = new GstEmpresas {
                    Empresa = EmpresaList
                },
                Atividades = new GstAtividades {
                    Atividade = AtividadesList
                },
                Duracao = 24,
                CpfGerente = gerente.Count() > 0 ? gerente.First().CPF : ""
            };

            return relatorio;
        }
    }
}
