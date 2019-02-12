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
    public class GeradorXmlService
    {
        private GestorDbContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public GeradorXmlService(GestorDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IEnumerable<Upload> ObterXmls(int projetoId)
        {
            var Upload = _context.Uploads
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Categoria == (CategoriaUpload)3)
                .ToList();
            return Upload;
        }
        public Resultado CriarArquivo(string XmlDoc, string Tipo, Projeto Projeto, string Versao, string UserId)
        {
            var resultado = new Resultado();
            XDocument xDoc = XDocument.Parse(JsonConvert.DeserializeXmlNode(XmlDoc, "PED").InnerXml);
            xDoc.Declaration = new XDeclaration("1.0", "ISO8859-1", null);
            xDoc.Root.SetAttributeValue("Tipo", Tipo);
            xDoc.Root.SetAttributeValue("CodigoEmpresa", Projeto.CatalogEmpresa.Valor);
            xDoc.Descendants()
                .Where(a => a.IsEmpty || String.IsNullOrWhiteSpace(a.Value))
                .Remove();
            string folderName = "uploads/xmls/";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string fileName = "APLPED" + Projeto.CatalogEmpresa.Valor + "_"+Tipo+"_" + Projeto.Numero + "_" + Versao + ".XML";
  
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            var upload = new Upload
            {
                NomeArquivo = fileName,
                UserId = UserId,
                Url = "wwwroot/"+folderName,
                ProjetoId = Projeto.Id,
                Categoria = (CategoriaUpload)3
            };
            _context.Uploads.Add(upload);
            _context.SaveChanges();
            resultado.Id = upload.Id.ToString();
            string fullPath = Path.Combine(newPath, upload.Id.ToString());
            xDoc.Save(fullPath);

            return resultado;
        }
        public Resultado DadosValidos(int ProjetoId, string Versao, string UserId){
            var resultado = new Resultado();
            if (ProjetoId<=0)
                resultado.Inconsistencias.Add("Informe o ProjetoId");
            else if (_context.Projetos.Where(p=>p.Id==ProjetoId).FirstOrDefault()==null)
                resultado.Inconsistencias.Add("ProjetoId não localizado");
            if (Versao==null)
                resultado.Inconsistencias.Add("Informe a Versão");
            if (UserId==null)
                resultado.Inconsistencias.Add("UserId Não localizado");
            return resultado;
        }
        public Resultado GerarXmlProrrogacao(int ProjetoId, string Versao, string UserId)
        {
            Resultado resultado = DadosValidos(ProjetoId, Versao, UserId);
            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Include("Etapas")
                        .Where(p => p.Id == ProjetoId)
                        .FirstOrDefault();
                if (projeto.Codigo==null)
                    resultado.Inconsistencias.Add("Código do Projeto não gerado");
                
                int Duracao = projeto.Etapas.Sum(p=>p.Duracao);
                if ((projeto.TipoValor=="PD" && Duracao>60)||(projeto.TipoValor=="PG" && Duracao>12))
                    resultado.Inconsistencias.Add("Duração máxima execedida para o projeto");
                
                if (resultado.Inconsistencias.Count()==0){
                    ProrrogacaoProjeto Prorrogacao = new ProrrogacaoProjeto{
                        PD_PrazoExecProjeto = new PD_PrazoExecProjeto{
                            Projeto = new ProProjeto{
                                CodProjeto = projeto.Codigo,
                                Duracao = projeto.Etapas.Sum(p=>p.Duracao)
                            }
                        }
                    };
                    string Tipo = "PRORROGAEXECUCAOPROJETO";
                    resultado = CriarArquivo(JsonConvert.SerializeObject(Prorrogacao), Tipo, projeto, Versao, UserId);
                }
            }
            resultado.Acao = "Geração Xml Prorogação do Projeto";
            return resultado;
        }
        public Resultado GerarXmlInicioExec(int ProjetoId, string Versao, string UserId)
        {   
            Resultado resultado = DadosValidos(ProjetoId, Versao, UserId);
            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();
                    
                if (projeto.Codigo==null)
                    resultado.Inconsistencias.Add("Código do Projeto não gerado");

                if (resultado.Inconsistencias.Count()==0){
                    InicioExecucao Inicio = new InicioExecucao{
                        PD_InicioExecProjeto = new PD_InicioExecProjeto{
                            Projeto = new InicioProjeto{
                                CodProjeto = projeto.Codigo,
                                DataIniProjeto = projeto.DataInicio.ToString(),
                                DirPropIntProjeto = projeto.CompartResultadosValor
                            }
                        }
                    };
                    string Tipo = "INICIOEXECUCAOPROJETO";
                    resultado = CriarArquivo(JsonConvert.SerializeObject(Inicio), Tipo, projeto, Versao, UserId);
                }
            }
            resultado.Acao = "Geração Xml Início Excecução do Projeto";
            return resultado;
        }
        public Resultado GerarXmlInteresseExec(int ProjetoId, string Versao, string UserId)
        {
            Resultado resultado = DadosValidos(ProjetoId, Versao, UserId);
            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Where(p => p.Id == ProjetoId)
                        .FirstOrDefault();
                
                if (projeto.Codigo==null)
                    resultado.Inconsistencias.Add("Código do Projeto não gerado");

                if (resultado.Inconsistencias.Count()==0){
                    InteresseExecucao Interesse = new InteresseExecucao{
                        PD_InteresseProjeto = new PD_InteresseProjeto{
                            Projeto = new InteresseProjeto{
                                CodProjeto = projeto.Codigo
                            }
                        }
                    };
                    string Tipo = "INTERESSEPROJETOPED";
                    resultado = CriarArquivo(JsonConvert.SerializeObject(Interesse), Tipo, projeto, Versao, UserId);
                }
            }
            resultado.Acao = "Geração Xml Interesse de Execução do Projeto";
            return resultado;
        }
        public Resultado GerarXmlProjetoPed(int ProjetoId, string Versao, string UserId)
        {
            Resultado resultado = DadosValidos(ProjetoId, Versao, UserId);
            if (resultado.Inconsistencias.Count == 0)
            {
                ProjetoPed ProjetoPed = new ProjetoPed();
                Projeto projeto = _context.Projetos
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
                        .Where(p => p.Id == ProjetoId)
                        .FirstOrDefault();
                if (projeto.Tema == null || projeto.Produtos.Count()<=0)
                    resultado.Inconsistencias.Add("Tema e/ou produto não cadastrados");
                if (resultado.Inconsistencias.Count == 0)
                {
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
                        Titulo = projeto.Titulo,
                        Duracao = projeto.Etapas.Sum(p=>p.Duracao),
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

                    string Tipo = "PROJETOPED";
                    resultado = CriarArquivo(JsonConvert.SerializeObject(ProjetoPed), Tipo, projeto, Versao, UserId);
                }
            }
            resultado.Acao = "Geração Xml Projeto PeD";
            return resultado;
        }
    }
}
