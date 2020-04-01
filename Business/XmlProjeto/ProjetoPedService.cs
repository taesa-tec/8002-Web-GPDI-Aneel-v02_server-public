using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace APIGestor.Business {
    public class XmlProjetoPedService : IXmlService<ProjetoPed> {
        private GestorDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        public XmlProjetoPedService( GestorDbContext context, IWebHostEnvironment hostingEnvironment ) {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public Resultado ValidaXml( int ProjetoId ) {
            Projeto projeto = obterProjeto(ProjetoId);
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            if(projeto.Tema == null || projeto.Produtos.Count() <= 0)
                resultado.Inconsistencias.Add("Tema e/ou produto não cadastrados");
            if(projeto.AvaliacaoInicial == null)
                resultado.Inconsistencias.Add("AvaliacaoInicial do projeto não preenchida");
            if(projeto.Etapas.Count() == 0)
                resultado.Inconsistencias.Add("Etapas do projeto não preenchida");
            if(projeto.CatalogSegmento == null)
                resultado.Inconsistencias.Add("Segmento do projeto não preenchida");
            if(projeto.Tema == null)
                resultado.Inconsistencias.Add("Tema do projeto não definido");
            if(projeto.Motivacao == null)
                resultado.Inconsistencias.Add("Motivacao do projeto não preenchida");
            if(projeto.Originalidade == null)
                resultado.Inconsistencias.Add("Originalidade do projeto não preenchida");
            if(projeto.Aplicabilidade == null)
                resultado.Inconsistencias.Add("Aplicabilidade do projeto não preenchida");
            if(projeto.Relevancia == null)
                resultado.Inconsistencias.Add("Relevancia do projeto não preenchida");
            if(projeto.Razoabilidade == null)
                resultado.Inconsistencias.Add("Razoabilidade do projeto não preenchida");
            if(projeto.Pesquisas == null)
                resultado.Inconsistencias.Add("Pesquisas do projeto não preenchida");
            return resultado;
        }
        public Projeto obterProjeto( int Id ) {
            return _context.Projetos
                         .Include("CatalogEmpresa")
                         .Include("CatalogSegmento")
                         .Include("Tema.CatalogTema")
                         .Include("Tema.SubTemas.CatalogSubTema")
                         .Include("Etapas")
                         .Include("Produtos.CatalogProdutoFaseCadeia")
                         .Include("Empresas.Estado")
                         .Include("Empresas.CatalogEmpresa")
                         .Include("RecursosHumanos")
                         // .Include(p => p.RecursosMateriais)
                         .Include("AlocacoesRh.RecursoHumano")
                         .Include("AlocacoesRm.RecursoMaterial")
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

        protected void getCustoEmpresa( Projeto projeto, Empresa empresa, List<Empresa> EmpresasExecutoras, out List<DestRecursosExec> DestRecursosExec, out DestRecursosEmp DestRecursosEmp ) {

            DestRecursosExec = new List<DestRecursosExec>(); //Da empresa financiadora para empresa executora
            DestRecursosEmp = new DestRecursosEmp(); // Da empresa pra ela própria

            #region Custo Com Empresas Executora
            var custoRHEmpresaExec =
                from al in projeto.AlocacoesRh
                where al.RecursoHumano.Empresa.ClassificacaoValor == "Executora" && al.EmpresaId == empresa.Id
                group al by al.RecursoHumano.EmpresaId into _empresa
                select new {
                    custo = _empresa.Sum(_al => _al.HrsTotais * _al.RecursoHumano.ValorHora),
                    empresa = _empresa.First().RecursoHumano.Empresa,
                    alocacoes = _empresa.ToList()
                };

            var custoRMEmpresaExec =
                from al in projeto.AlocacoesRm
                where al.EmpresaFinanciadoraId == empresa.Id
                group al by al.EmpresaRecebedoraId into _empresa
                select new {
                    custo = _empresa.Sum(_al => _al.Qtd * _al.RecursoMaterial.ValorUnitario),
                    empresa = _empresa.First().EmpresaRecebedora,
                    alocacoes = _empresa.ToList()
                };

            foreach(Empresa executora in EmpresasExecutoras) {

                var CustoCatContabilExec = new List<CustoCatContabilExec>();

                var custosRH = custoRHEmpresaExec.FirstOrDefault(ex => ex.empresa.Id == executora.Id);
                var custosRM = custoRMEmpresaExec.FirstOrDefault(ex => ex.empresa.Id == executora.Id);

                if(custosRH != null) {
                    CustoCatContabilExec.Add(new CustoCatContabilExec {
                        CatContabil = "RH",
                        CustoExec = custosRH.custo.ToString()
                    });
                }

                if(custosRM != null) {

                    var custoCatContabilExec =
                        from r in this.groupAlocacoesRmByCategory(custosRM.alocacoes)
                        select new CustoCatContabilExec {
                            CatContabil = r.Key,
                            CustoExec = r.Value.Sum(a => a.Qtd * a.RecursoMaterial.ValorUnitario).ToString()
                        };

                    CustoCatContabilExec.AddRange(custoCatContabilExec);
                }

                if(CustoCatContabilExec.Count > 0) {
                    DestRecursosExec.Add(new DestRecursosExec {
                        CNPJExec = executora.Cnpj,
                        CustoCatContabilExec = CustoCatContabilExec
                    });
                }


            }
            #endregion

            #region Custo com a própria empresa
            var custoRHEmpresa = projeto.AlocacoesRh.Where(al => al.EmpresaId == empresa.Id && al.RecursoHumano.EmpresaId == empresa.Id).Sum(al => al.HrsTotais * al.RecursoHumano.ValorHora);

            var custoRMEmpresa = projeto.AlocacoesRm.Where(al => al.EmpresaRecebedoraId == empresa.Id)
                .GroupBy(al => al.RecursoMaterial.CategoriaContabilValor)
                .Select(a => new CustoCatContabilEmp {
                    CatContabil = a.Key,
                    CustoEmp = a.Sum(b => b.Qtd * b.RecursoMaterial.ValorUnitario).ToString()
                }).ToList();

            DestRecursosEmp.CustoCatContabilEmp = new List<CustoCatContabilEmp>();

            DestRecursosEmp.CustoCatContabilEmp.Add(new CustoCatContabilEmp { CatContabil = "RH", CustoEmp = custoRHEmpresa.ToString() });

            DestRecursosEmp.CustoCatContabilEmp.AddRange(custoRMEmpresa);
            #endregion


        }

        public ProjetoPed GerarXml( int ProjetoId, string Versao, string UserId ) {

            ProjetoPed ProjetoPed = new ProjetoPed();
            Projeto projeto = obterProjeto(ProjetoId);

            var PedEmpresaList = new List<PedEmpresa>();
            var PedExecutoraList = new List<PedExecutora>();
            var recursosHumanos = projeto.RecursosHumanos.ToList();
            var EmpresasFinanciadoras = projeto.Empresas.Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente").ToList();
            var EmpresasExecutoras = projeto.Empresas.Where(p => p.ClassificacaoValor == "Executora").ToList();
            var EmpresasParceiras = projeto.Empresas.Where(p => p.ClassificacaoValor == "Parceira").ToList();

            ProjetoPed.PD_Recursos = new PD_Recursos {
                RecursoEmpresa = new List<RecursoEmpresa>(),
                RecursoParceira = new List<RecursoParceira>()
            };


            #region PD_ProjetoBase

            var SubtemasList = new List<PedSubTema>();

            foreach(TemaSubTema subTema in projeto.Tema.SubTemas) {
                SubtemasList.Add(new PedSubTema {
                    CodSubtema = subTema.CatalogSubTema.Valor,
                    OutroSubtema = subTema.OutroDesc
                });
            }

            ProjetoPed.PD_ProjetoBase = new PD_ProjetoBase {
                AvIniANEEL = projeto.AvaliacaoInicial.ToString(),
                Titulo = projeto.TituloDesc,
                Duracao = projeto.Etapas.Sum(p => p.Duracao),
                Segmento = projeto.CatalogSegmento.Valor,
                CodTema = projeto.Tema.CatalogTema.Valor,
                OutroTema = projeto.Tema.OutroDesc,
                Subtemas = new PedSubTemas {
                    Subtema = SubtemasList
                },
                Motivacao = projeto.Motivacao,
                Originalidade = projeto.Originalidade,
                Aplicabilidade = projeto.Aplicabilidade,
                Relevancia = projeto.Relevancia,
                RazoabCustos = projeto.Razoabilidade,
                PesqCorrelata = projeto.Pesquisas
            };

            Produto Produto = projeto.Produtos.Where(p => p.Classificacao == (ProdutoClassificacao)(1)).FirstOrDefault();

            if(Produto != null) {
                ProjetoPed.PD_ProjetoBase.FaseInovacao = Produto.CatalogProdutoFaseCadeia.Valor;
                ProjetoPed.PD_ProjetoBase.TipoProduto = Produto.TipoValor;
                ProjetoPed.PD_ProjetoBase.DescricaoProduto = Produto.Desc;
            }
            #endregion

            foreach(Empresa empresa in EmpresasFinanciadoras) {

                var DestRecursosExec = new List<DestRecursosExec>(); //Da empresa financiadora para empresa executora
                var DestRecursosEmp = new DestRecursosEmp(); // Da empresa pra ela própria

                #region Equipe Empresa
                var equipeList =
                    from rh in recursosHumanos
                    where rh.CPF != null && rh.EmpresaId == empresa.Id
                    select new EquipeEmpresa {
                        NomeMbEqEmp = rh.NomeCompleto,
                        CpfMbEqEmp = rh.CPF,
                        TitulacaoMbEqEmp = rh.TitulacaoValor,
                        FuncaoMbEqEmp = rh.FuncaoValor
                    };
                PedEmpresaList.Add(new PedEmpresa {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    TipoEmpresa = empresa.ClassificacaoValor,
                    Equipe = new Equipe {
                        EquipeEmpresa = equipeList.ToList()
                    }
                });

                #endregion

                this.getCustoEmpresa(projeto, empresa, EmpresasExecutoras, out DestRecursosExec, out DestRecursosEmp);

                ProjetoPed.PD_Recursos.RecursoEmpresa.Add(new RecursoEmpresa {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    DestRecursosExec = DestRecursosExec,
                    DestRecursosEmp = DestRecursosEmp,
                });



            }
            foreach(Empresa empresa in EmpresasParceiras) {

                var DestRecursosExec = new List<DestRecursosExec>(); //Da empresa financiadora para empresa executora
                var DestRecursosEmp = new DestRecursosEmp(); // Da empresa pra ela própria

                this.getCustoEmpresa(projeto, empresa, EmpresasExecutoras, out DestRecursosExec, out DestRecursosEmp);

                ProjetoPed.PD_Recursos.RecursoParceira.Add(new RecursoParceira {
                    CNPJParc = empresa.Cnpj,
                    DestRecursosExec = DestRecursosExec
                });

            }

            foreach(Empresa empresa in EmpresasExecutoras) {

                #region Equipe Empresa
                var equipeList =
                    from rh in recursosHumanos
                    where rh.EmpresaId == empresa.Id
                    select new EquipeExec {
                        NomeMbEqExec = rh.NomeCompleto,
                        BRMbEqExec = rh.NacionalidadeValor,
                        DocMbEqExec = rh.Passaporte ?? rh.CPF,
                        TitulacaoMbEqExec = rh.TitulacaoValor,
                        FuncaoMbEqExec = rh.FuncaoValor
                    };
                #endregion

                PedExecutoraList.Add(new PedExecutora {
                    CNPJExec = empresa.Cnpj,
                    RazaoSocialExec = empresa.RazaoSocial,
                    UfExec = empresa.Estado.Valor,
                    Equipe = new ExecEquipe {
                        EquipeExec = equipeList.ToList()
                    }
                });
            }

            ProjetoPed.PD_Equipe = new PD_Equipe {
                Empresas = new PedEmpresas {
                    Empresa = PedEmpresaList
                },
                Executoras = new PedExecutoras {
                    Executora = PedExecutoraList
                }
            };


            return ProjetoPed;
        }
    }
}
