using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Xml.ProjetoPeD;
using PeD.Data;
using Empresa = PeD.Core.Models.Projetos.Xml.ProjetoPeD.Empresa;

namespace PeD.Services.Projetos.Xml
{
    public class ProjetoPeDService
    {
        private GestorDbContext _context;

        public ProjetoPeDService(GestorDbContext context)
        {
            _context = context;
        }

        public ProjetoPed ProjetoPed(int projetoId)
        {
            var projeto = _context.Set<Projeto>()
                              .Include(p => p.PlanoTrabalho)
                              .Include(p => p.Tema)
                              .Include(p => p.SubTemas).ThenInclude(s => s.SubTema)
                              .FirstOrDefault(p => p.Id == projetoId) ??
                          throw new Exception("Projeto Não encontrado");

            return new ProjetoPed()
            {
                PD_ProjetoBase = PD_ProjetoBase(projeto),
                PD_Equipe = PD_Equipe(projeto),
                PD_Recursos = PD_Recursos(projeto)
            };
        }

        #region PD_ProjetoBase

        private PD_ProjetoBase PD_ProjetoBase(Projeto projeto)
        {
            var produtoFinal =
                _context.Set<Produto>().Include(p => p.FaseCadeia)
                    .FirstOrDefault(p => p.ProjetoId == projeto.Id && p.Classificacao == ProdutoClassificacao.Final) ??
                throw new Exception("Produto final não encontrado");

            return new PD_ProjetoBase()
            {
                Duracao = projeto.Duracao,
                Titulo = projeto.Titulo,
                Segmento = projeto.SegmentoId,
                CodTema = projeto.Tema.Valor,
                OutroTema = projeto.TemaOutro,
                Subtemas = new SubTemas()
                {
                    Subtema = projeto.SubTemas.Select(s => new SubTema()
                    {
                        CodSubtema = s.SubTema?.Valor ?? "",
                        OutroSubtema = s.Outro
                    }).ToList()
                },
                FaseInovacao = produtoFinal.FaseCadeia.Id,
                TipoProduto = produtoFinal.TipoId,
                DescricaoProduto = produtoFinal.Descricao,
                Motivacao = projeto.PlanoTrabalho.Motivacao,
                Originalidade = projeto.PlanoTrabalho.Originalidade,
                Aplicabilidade = projeto.PlanoTrabalho.Aplicabilidade,
                Relevancia = projeto.PlanoTrabalho.Relevancia,
                RazoabCustos = projeto.PlanoTrabalho.RazoabilidadeCustos,
                PesqCorrelata = projeto.PlanoTrabalho.PesquisasCorrelatasPeDAneel,
                AvIniANEEL = true
            };
        }

        #endregion

        #region PD_Equipe

        private PD_Equipe PD_Equipe(Projeto projeto)
        {
            var recursos = _context.Set<RecursoHumano>()
                .Include(r => r.Empresa)
                .Where(a => a.ProjetoId == projeto.Id).ToList();
            return new PD_Equipe()
            {
                Empresas = new Empresas()
                {
                    Empresa = Empresas(projeto, recursos)
                },
                Executoras = new Executoras()
                {
                    Executora = Executoras(recursos)
                }
            };
        }

        private List<Empresa> Empresas(Projeto projeto, IEnumerable<RecursoHumano> recursos)
        {
            return recursos
                .Where(r => r.Empresa is {Funcao: Funcao.Cooperada})
                .GroupBy(r => r.EmpresaId)
                .Select(r => new Empresa()
                {
                    CodEmpresa = r.First().Empresa.Codigo,
                    TipoEmpresa = r.First().Empresa.EmpresaRefId == projeto.ProponenteId,
                    Equipe = new Equipe()
                    {
                        EquipeEmpresa = EquipeEmpresas(r)
                    }
                }).ToList();
        }

        private List<Executora> Executoras(IEnumerable<RecursoHumano> recursos)
        {
            return recursos
                .Where(r => r.Empresa is {Funcao : Funcao.Executora})
                .GroupBy(r => r.EmpresaId)
                .Select(a => new Executora()
                {
                    CNPJExec = a.First().Empresa.CNPJ,
                    UfExec = a.First().Empresa.UF,
                    RazaoSocialExec = a.First().Empresa.Nome,
                    Equipe = new ExecEquipe()
                    {
                        EquipeExec = EquipeExecs(a)
                    }
                }).ToList();
        }

        private List<EquipeEmpresa> EquipeEmpresas(IEnumerable<RecursoHumano> recursos)
        {
            return recursos.Select(r => new EquipeEmpresa()
            {
                NomeMbEqEmp = r.NomeCompleto,
                CpfMbEqEmp = r.Documento,
                FuncaoMbEqEmp = r.Funcao,
                TitulacaoMbEqEmp = r.Titulacao,
            }).ToList();
        }

        private List<EquipeExec> EquipeExecs(IEnumerable<RecursoHumano> recursos)
        {
            return recursos.Select(r => new EquipeExec()
            {
                NomeMbEqExec = r.NomeCompleto,
                BRMbEqExec = r.Nacionalidade == "Brasileiro",
                DocMbEqExec = r.Documento,
                TitulacaoMbEqExec = r.Titulacao,
                FuncaoMbEqExec = r.Funcao,
            }).ToList();
        }

        #endregion

        #region PD_Recursos

        private PD_Recursos PD_Recursos(Projeto projeto)
        {
            var alocacoes = _context.Set<RecursoMaterial.AlocacaoRm>()
                .Include(a => a.EmpresaFinanciadora)
                .ThenInclude(e => e.EmpresaRef)
                .Include(a => a.EmpresaRecebedora)
                .ThenInclude(e => e.EmpresaRef)
                .Include(a => a.RecursoMaterial).ThenInclude(r => r.CategoriaContabil)
                .Where(a => a.ProjetoId == projeto.Id).ToList();
            return new PD_Recursos()
            {
                RecursoEmpresa = RecursoEmpresa(alocacoes.Where(a => a.EmpresaFinanciadora.Funcao != Funcao.Executora)),
                RecursoParceira =
                    RecursoParceira(alocacoes.Where(a => a.EmpresaFinanciadora.Funcao == Funcao.Executora))
            };
        }

        private List<RecursoEmpresa> RecursoEmpresa(IEnumerable<RecursoMaterial.AlocacaoRm> alocacoes)
        {
            return alocacoes.GroupBy(a => a.EmpresaFinanciadoraId).Select(a => new RecursoEmpresa()
            {
                CodEmpresa = a.First().EmpresaFinanciadora.Codigo,
                DestRecursosExec = DestRecursosExec(a)
            }).ToList();
        }

        private List<RecursoParceira> RecursoParceira(IEnumerable<RecursoMaterial.AlocacaoRm> alocacoes)
        {
            return alocacoes.GroupBy(a => a.EmpresaFinanciadoraId).Select(a => new RecursoParceira()
            {
                CNPJParc = a.First().EmpresaFinanciadora.CNPJ,
                DestRecursosExec = DestRecursosExec(a)
            }).ToList();
        }

        private List<DestRecursosExec> DestRecursosExec(IEnumerable<RecursoMaterial.AlocacaoRm> alocacoes)
        {
            return alocacoes.Where(i => i.EmpresaRecebedora is {Funcao: Funcao.Executora})
                .GroupBy(i => i.EmpresaRecebedoraId)
                .Select(e => new DestRecursosExec()
                {
                    CNPJExec = e.First().EmpresaRecebedora.CNPJ,
                    CustoCatContabilExec = e.GroupBy(i => i.RecursoMaterial.CategoriaContabilId)
                        .Select(c => new CustoCatContabilExec()
                        {
                            CatContabil = c.First().RecursoMaterial.CategoriaContabil.Valor,
                            CustoExec = c.Sum(i => i.Custo)
                        }).ToList()
                }).ToList();
        }

        #endregion
    }
}