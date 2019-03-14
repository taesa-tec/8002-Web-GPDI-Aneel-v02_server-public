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

namespace APIGestor.Business
{
    public class XmlProjetoPedService
    {
        private GestorDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public XmlProjetoPedService(GestorDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public Resultado ValidaXml(int ProjetoId)
        {
            Projeto projeto = obterProjeto(ProjetoId);
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            if (projeto.Tema == null || projeto.Produtos.Count() <= 0)
                resultado.Inconsistencias.Add("Tema e/ou produto não cadastrados");
            if (projeto.AvaliacaoInicial == null)
                resultado.Inconsistencias.Add("AvaliacaoInicial do projeto não preenchida");
            if (projeto.Etapas.Count() == 0)
                resultado.Inconsistencias.Add("Etapas do projeto não preenchida");
            if (projeto.CatalogSegmento == null)
                resultado.Inconsistencias.Add("Segmento do projeto não preenchida");
            if (projeto.Tema == null)
                resultado.Inconsistencias.Add("Tema do projeto não definido");
            if (projeto.Motivacao == null)
                resultado.Inconsistencias.Add("Motivacao do projeto não preenchida");
            if (projeto.Originalidade == null)
                resultado.Inconsistencias.Add("Originalidade do projeto não preenchida");
            if (projeto.Aplicabilidade == null)
                resultado.Inconsistencias.Add("Aplicabilidade do projeto não preenchida");
            if (projeto.Relevancia == null)
                resultado.Inconsistencias.Add("Relevancia do projeto não preenchida");
            if (projeto.Razoabilidade == null)
                resultado.Inconsistencias.Add("Razoabilidade do projeto não preenchida");
            if (projeto.Pesquisas == null)
                resultado.Inconsistencias.Add("Pesquisas do projeto não preenchida");
            return resultado;
        }
        public Projeto obterProjeto(int Id)
        {
            return _context.Projetos
                         .Include("CatalogEmpresa")
                         .Include("CatalogSegmento")
                         .Include("Tema.CatalogTema")
                         .Include("Tema.SubTemas.CatalogSubTema")
                         .Include("Etapas")
                         .Include("Produtos")
                         .Include("Empresas.Estado")
                         .Include("Empresas.CatalogEmpresa")
                         .Include("RecursosHumanos")
                         // .Include(p => p.RecursosMateriais)
                         .Include("AlocacoesRm.RecursoMaterial")
                         .Include("RelatorioFinal.Uploads")
                         .Where(p => p.Id == Id)
                         .FirstOrDefault();
        }
        public ProjetoPed GerarXml(int ProjetoId, string Versao, string UserId)
        {
            ProjetoPed ProjetoPed = new ProjetoPed();
            Projeto projeto = obterProjeto(ProjetoId);
            var SubtemasList = new List<PedSubTema>();
            foreach (TemaSubTema subTema in projeto.Tema.SubTemas)
            {
                SubtemasList.Add(new PedSubTema
                {
                    CodSubtema = subTema.CatalogSubTema.Valor,
                    OutroSubtema = subTema.OutroDesc
                });
            }
            ProjetoPed.PD_ProjetoBase = new PD_ProjetoBase
            {
                AvIniANEEL = projeto.AvaliacaoInicial.ToString(),
                Titulo = projeto.TituloDesc,
                Duracao = projeto.Etapas.Sum(p => p.Duracao),
                Segmento = projeto.CatalogSegmento.Valor,
                CodTema = projeto.Tema.CatalogTema.Valor,
                OutroTema = projeto.Tema.OutroDesc,
                Subtemas = new PedSubTemas
                {
                    Subtema = SubtemasList
                },
                Motivacao = projeto.Motivacao,
                Originalidade = projeto.Originalidade,
                Aplicabilidade = projeto.Aplicabilidade,
                Relevancia = projeto.Relevancia,
                RazoabCustos = projeto.Razoabilidade,
                PesqCorrelata = projeto.Pesquisas
            };
            Produto Produto = projeto.Produtos.Where(
                        p => p.Classificacao == (ProdutoClassificacao)(1)).FirstOrDefault();
            if (Produto != null)
            {
                ProjetoPed.PD_ProjetoBase.FaseInovacao = Produto.FaseCadeiaValor;
                ProjetoPed.PD_ProjetoBase.TipoProduto = Produto.TipoValor;
                ProjetoPed.PD_ProjetoBase.DescricaoProduto = Produto.Desc;

            }
            // PD_EQUIPE
            var PedEmpresaList = new List<PedEmpresa>();
            var EmpresasFinanciadoras = projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                .ToList();
            foreach (Empresa empresa in EmpresasFinanciadoras)
            {
                var equipeList = new List<EquipeEmpresa>();
                foreach (RecursoHumano rh in projeto.RecursosHumanos
                    .Where(p => p.CPF != null)
                    .Where(p => p.Empresa == empresa)
                    .ToList())
                {
                    equipeList.Add(new EquipeEmpresa
                    {
                        NomeMbEqEmp = rh.NomeCompleto,
                        CpfMbEqEmp = rh.CPF,
                        TitulacaoMbEqEmp = rh.TitulacaoValor,
                        FuncaoMbEqEmp = rh.FuncaoValor
                    });
                }
                PedEmpresaList.Add(new PedEmpresa
                {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    TipoEmpresa = empresa.ClassificacaoValor,
                    Equipe = new Equipe
                    {
                        EquipeEmpresa = equipeList
                    }
                });
            }
            var PedExecutoraList = new List<PedExecutora>();
            foreach (Empresa empresa in projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Executora")
                .ToList())
            {
                var equipeList = new List<EquipeExec>();
                foreach (RecursoHumano rh in projeto.RecursosHumanos
                    .Where(p => p.Empresa == empresa)
                    .ToList())
                {
                    equipeList.Add(new EquipeExec
                    {
                        NomeMbEqExec = rh.NomeCompleto,
                        BRMbEqExec = rh.NacionalidadeValor,
                        DocMbEqExec = rh.CPF ?? rh.Passaporte,
                        TitulacaoMbEqExec = rh.TitulacaoValor,
                        FuncaoMbEqExec = rh.FuncaoValor
                    });
                }
                PedExecutoraList.Add(new PedExecutora
                {
                    CNPJExec = empresa.Cnpj,
                    RazaoSocialExec = empresa.RazaoSocial,
                    UfExec = empresa.Estado.Valor,
                    Equipe = new ExecEquipe
                    {
                        EquipeExec = equipeList
                    }
                });
            }
            ProjetoPed.PD_Equipe = new PD_Equipe
            {
                Empresas = new PedEmpresas
                {
                    Empresa = PedEmpresaList
                },
                Executoras = new PedExecutoras
                {
                    Executora = PedExecutoraList
                }
            };
            // PD_RECURSO
            ProjetoPed.PD_Recursos = new PD_Recursos
            {
                RecursoEmpresa = new List<RecursoEmpresa>(),
                RecursoParceira = new List<RecursoParceira>()
            };
            foreach (Empresa empresa in EmpresasFinanciadoras)
            {
                // DestRecursosExec
                var DestRecursosExec = new List<DestRecursosExec>();
                foreach (var rm in projeto.AlocacoesRm
                    .Where(p => p.EmpresaRecebedora.ClassificacaoValor == "Executora")
                    .Where(p => p.EmpresaFinanciadora == empresa)
                    .GroupBy(p => p.EmpresaRecebedora)
                    .ToList())
                {
                    var CustoCatContabilExec = new List<CustoCatContabilExec>();
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil))
                    {
                        decimal custo = 0;
                        foreach (var rm1 in rm0)
                        {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        CustoCatContabilExec.Add(new CustoCatContabilExec
                        {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                            CustoExec = custo.ToString()
                        });
                    }
                    DestRecursosExec.Add(new DestRecursosExec
                    {
                        CNPJExec = rm.First().EmpresaRecebedora.Cnpj,
                        CustoCatContabilExec = CustoCatContabilExec
                    });
                }

                // DestRecursosEmp
                var DestRecursosEmp = new List<DestRecursosEmp>();
                foreach (var rm in projeto.AlocacoesRm
                        .Where(p => p.EmpresaRecebedora == empresa)
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList())
                {
                    var CustoCatContabilEmp = new List<CustoCatContabilEmp>();
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil))
                    {
                        decimal custo = 0;
                        foreach (var rm1 in rm0)
                        {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        CustoCatContabilEmp.Add(new CustoCatContabilEmp
                        {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                            CustoEmp = custo.ToString()
                        });
                    }
                    DestRecursosEmp.Add(new DestRecursosEmp
                    {
                        CustoCatContabilEmp = CustoCatContabilEmp
                    });
                }
                ProjetoPed.PD_Recursos.RecursoEmpresa.Add(new RecursoEmpresa
                {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    DestRecursosExec = DestRecursosExec,
                    DestRecursosEmp = DestRecursosEmp,
                });
            }

            foreach (Empresa empresa in projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Parceira")
                .ToList())
            {
                var DestRecursosExec = new List<DestRecursosExec>();
                foreach (var rm in projeto.AlocacoesRm
                    .Where(p => p.EmpresaRecebedora.ClassificacaoValor == "Executora")
                    .Where(p => p.EmpresaFinanciadora == empresa)
                    .GroupBy(p => p.EmpresaRecebedora)
                    .ToList())
                {
                    var CustoCatContabilExec = new List<CustoCatContabilExec>();
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil))
                    {
                        decimal custo = 0;
                        foreach (var rm1 in rm0)
                        {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        CustoCatContabilExec.Add(new CustoCatContabilExec
                        {
                            CatContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                            CustoExec = custo.ToString()
                        });
                    }
                    DestRecursosExec.Add(new DestRecursosExec
                    {
                        CNPJExec = rm.First().EmpresaRecebedora.Cnpj,
                        CustoCatContabilExec = CustoCatContabilExec
                    });
                }

                ProjetoPed.PD_Recursos.RecursoParceira.Add(new RecursoParceira
                {
                    CNPJParc = empresa.Cnpj,
                    DestRecursosExec = DestRecursosExec
                });
            }
            return ProjetoPed;
        }
    }
}
