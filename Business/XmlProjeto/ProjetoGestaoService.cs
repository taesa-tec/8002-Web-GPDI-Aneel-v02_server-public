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
    public class XmlProjetoGestaoService : IXmlService<XmlProjetoGestao>
    {
        private GestorDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public XmlProjetoGestaoService(GestorDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public Resultado ValidaXml(int ProjetoId)
        {
            Projeto projeto = obterProjeto(ProjetoId);
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            if (projeto.Etapas.Count() == 0)
                resultado.Inconsistencias.Add("Etapas do projeto não preenchida");
            if (projeto.Atividades.DedicacaoHorario == null)
                resultado.Inconsistencias.Add("Atividades do projeto não preenchidas");
            if(projeto.RecursosHumanos.Where(p => p.GerenteProjeto == true).Count() == 0)
                resultado.Inconsistencias.Add("O projeto não tem nenhum gerente cadastrado");

            return resultado;
        }
        public Projeto obterProjeto(int Id)
        {
            return _context.Projetos
                         .Include("CatalogEmpresa")
                         .Include("Etapas")
                         .Include("Atividades")
                         .Include("Empresas.Estado")
                         .Include("Empresas.CatalogEmpresa")
                         .Include("RecursosHumanos")
                         // .Include(p => p.RecursosMateriais)
                         .Include("AlocacoesRh.RecursoHumano")
                         .Include("AlocacoesRm.RecursoMaterial.Atividade")
                         .Include("RelatorioFinal.Uploads")
                         .Where(p => p.Id == Id)
                         .FirstOrDefault();
        }
        public XmlProjetoGestao GerarXml(int ProjetoId, string Versao, string UserId)
        {
            XmlProjetoGestao relatorio = new XmlProjetoGestao();
            Projeto projeto = obterProjeto(ProjetoId);

            // EMPRESAS
            var EmpresaList = new List<GstEmpresa>();
            // CustosContabeis
            var GstCustoCatContabilList = new List<GstCustoCatContabil>();

            var EmpresasFinanciadoras = projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                .ToList();
            foreach (Empresa empresa in EmpresasFinanciadoras)
            {
                var equipeList = new List<GstEquipeGestao>();
                foreach (RecursoHumano rh in projeto.RecursosHumanos
                    .Where(p => p.CPF != null)
                    .Where(p => p.Empresa == empresa)
                    .ToList())
                {
                    equipeList.Add(new GstEquipeGestao
                    {
                        NomeMbEqEmp = rh.NomeCompleto,
                        CpfMbEqEmp = rh.CPF
                    });
                }

                var GstCustoCatContabil = new List<GstCustoCatContabil>();
                //RH
                foreach (var rh in projeto.AlocacoesRh
                        .Where(p => p.Empresa == empresa)
                        .GroupBy(p => p.Empresa)
                        .ToList())
                {
                    decimal? custo = 0;
                    foreach (var a in rh)
                    {
                        custo += a.RecursoHumano.ValorHora * ((a.HrsMes1 + a.HrsMes2 + a.HrsMes3 + a.HrsMes4 + a.HrsMes5 + a.HrsMes6) + (a.HrsMes7 + a.HrsMes8 + a.HrsMes9 + a.HrsMes10 + a.HrsMes11 + a.HrsMes12) + (a.HrsMes13 + a.HrsMes14 + a.HrsMes15 + a.HrsMes16 + a.HrsMes17 + a.HrsMes18) + (a.HrsMes19 + a.HrsMes20 + a.HrsMes21 + a.HrsMes22 + a.HrsMes23 + a.HrsMes24));
                    }
                    GstCustoCatContabil.Add(new GstCustoCatContabil
                    {
                        CategoriaContabil = "RH",
                        CustoEmpresa = custo.ToString()
                    });
                }
                // RM
                foreach (var rm in projeto.AlocacoesRm
                        .Where(p => p.EmpresaRecebedora == empresa)
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList())
                {
                    foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabilGestao.Valor))
                    {
                        decimal custo = 0;
                        foreach (var rm1 in rm0)
                        {
                            custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                        }
                        GstCustoCatContabil.Add(new GstCustoCatContabil
                        {
                            CategoriaContabil = rm0.First().RecursoMaterial.CategoriaContabilGestao.Valor,
                            CustoEmpresa = custo.ToString()
                        });
                    }
                }

                EmpresaList.Add(new GstEmpresa
                {
                    TipoEmpresa = empresa.ClassificacaoValor,
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    Equipe = new GstEquipe
                    {
                        EquipeGestao = equipeList
                    },
                    CustosContabeis = new GstCustosContabeis
                    {
                        CustoCatContabil = GstCustoCatContabil
                    }
                });
            }
            // Atividades
            var AtividadesList = new List<GstAtividade>();
            //RH
            foreach (var a in projeto.AlocacoesRh.ToList())
            {
                decimal? custo = 0;
                custo += a.RecursoHumano.ValorHora * ((a.HrsMes1 + a.HrsMes2 + a.HrsMes3 + a.HrsMes4 + a.HrsMes5 + a.HrsMes6) + (a.HrsMes7 + a.HrsMes8 + a.HrsMes9 + a.HrsMes10 + a.HrsMes11 + a.HrsMes12) + (a.HrsMes13 + a.HrsMes14 + a.HrsMes15 + a.HrsMes16 + a.HrsMes17 + a.HrsMes18) + (a.HrsMes19 + a.HrsMes20 + a.HrsMes21 + a.HrsMes22 + a.HrsMes23 + a.HrsMes24));

                string _descAtividade = projeto.Atividades.DedicacaoHorario;
                AtividadesList.Add(new GstAtividade
                {
                    TipoAtividade = "HH",
                    DescAtividade = _descAtividade,
                    CustoAtividade = custo.ToString()
                });
            }
            // RM
            foreach (var rm in projeto.AlocacoesRm
                .GroupBy(p => p.RecursoMaterial.Atividade.Valor)
                .ToList())
            {
                decimal custo = 0;
                foreach (var rm1 in rm)
                {
                    custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                }
                string _descAtividade = null;
                switch (rm.First().RecursoMaterial.Atividade.Valor)
                {
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
                AtividadesList.Add(new GstAtividade
                {
                    TipoAtividade = rm.First().RecursoMaterial.Atividade.Valor,
                    DescAtividade = _descAtividade,
                    CustoAtividade = custo.ToString()
                });
            }

            // PD_ProjetoGestao
            var gerente = projeto.RecursosHumanos.Where(p => p.GerenteProjeto == true);
            relatorio.PD_ProjetoGestao = new PD_ProjetoGestao
            {
                Empresas = new GstEmpresas
                {
                    Empresa = EmpresaList
                },
                Atividades = new GstAtividades
                {
                    Atividade = AtividadesList
                },
                Duracao = 24,
                CpfGerente = gerente.Count() > 0 ? gerente.First().CPF : ""
            };

            return relatorio;
        }
    }
}
