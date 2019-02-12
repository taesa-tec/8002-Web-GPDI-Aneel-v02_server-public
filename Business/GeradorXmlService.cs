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
                .Where(p => p.Categoria == (CategoriaUpload)6)
                .ToList();
            return Upload;
        }
        public XDocument CriarArquivo(XmlDocument XmlDoc, string Tipo, string CodigoEmpresa, int projetoId, string fileName, string UserId)
        {
            XDocument xDoc = XDocument.Parse(XmlDoc.InnerXml);
            xDoc.Declaration = new XDeclaration("1.0", "ISO8859-1", null);
            xDoc.Root.SetAttributeValue("Tipo", Tipo);
            xDoc.Root.SetAttributeValue("CodigoEmpresa", CodigoEmpresa);
            xDoc.Descendants()
                .Where(a => a.IsEmpty || String.IsNullOrWhiteSpace(a.Value))
                .Remove();
            string folderName = "uploads/xmls/";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            var upload = new Upload
            {
                NomeArquivo = fileName,
                UserId = UserId,
                Url = newPath,
                ProjetoId = projetoId,
                Categoria = (CategoriaUpload)6
            };
            _context.Uploads.Add(upload);
            _context.SaveChanges();

            string fullPath = Path.Combine(newPath, upload.Id.ToString());
            xDoc.Save(fullPath);

            return xDoc;
        }
        public XDocument GerarXmlInicioExec(int ProjetoId, string Versao, string UserId)
        {
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();
            if (projeto == null)
                return null;
            if (projeto.Codigo==null)
                return null;
            InicioExecucao Interesse = new InicioExecucao{
                PD_InicioExecProjeto = new PD_InicioExecProjeto{
                    Projeto = new InicioProjeto{
                        CodProjeto = projeto.Codigo,
                        DataIniProjeto = projeto.DataInicio.ToString(),
                        DirPropIntProjeto = projeto.CompartResultadosValor
                    }
                }
            };
            string NumeroProjeto = projeto.Numero;
            string Tipo = "INICIOEXECUCAOPROJETO";
            string CodigoEmpresa = projeto.CatalogEmpresa.Valor;
            string fileName = "APLPED" + CodigoEmpresa + "_"+Tipo+"_" + NumeroProjeto + "_" + Versao + ".XML";

            return CriarArquivo(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(Interesse), "PED"), Tipo, CodigoEmpresa, ProjetoId, fileName, UserId);
        }
        public XDocument GerarXmlInteresseExec(int ProjetoId, string Versao, string UserId)
        {
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();
            if (projeto == null)
                return null;
            if (projeto.Codigo==null)
                return null;
            InteresseExecucao Interesse = new InteresseExecucao{
                PD_InteresseProjeto = new PD_InteresseProjeto{
                    Projeto = new InteresseProjeto{
                        CodProjeto = projeto.Codigo
                    }
                }
            };
            string NumeroProjeto = projeto.Numero;
            string Tipo = "INTERESSEPROJETOPED";
            string CodigoEmpresa = projeto.CatalogEmpresa.Valor;
            string fileName = "APLPED" + CodigoEmpresa + "_"+Tipo+"_" + NumeroProjeto + "_" + Versao + ".XML";

            return CriarArquivo(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(Interesse), "PED"), Tipo, CodigoEmpresa, ProjetoId, fileName, UserId);
        }
        public XDocument GerarXmlProjetoPed(int ProjetoId, string Versao, string UserId)
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
            if (projeto == null)
                return null;
            if (projeto.Tema == null || projeto.Produtos == null)
                return null;
            string NumeroProjeto = projeto.Numero;
            string Tipo = "PROJETOPED";
            string CodigoEmpresa = projeto.CatalogEmpresa.Valor;

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
                Duracao = projeto.Etapas.Count() * 6,
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
            string fileName = "APLPED" + CodigoEmpresa + "_"+Tipo+"_" + NumeroProjeto + "_" + Versao + ".XML";

            return CriarArquivo(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(ProjetoPed), "PED"), Tipo, CodigoEmpresa, ProjetoId, fileName, UserId);
        }
    }
}
