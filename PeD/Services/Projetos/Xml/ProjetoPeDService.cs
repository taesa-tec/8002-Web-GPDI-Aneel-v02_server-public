using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Xml.ProjetoPeD;
using PeD.Data;

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
                .Include(r => r.CoExecutor)
                .Where(a => a.ProjetoId == projeto.Id).ToList();
            return new PD_Equipe()
            {
                Empresas = new Empresas()
                {
                    Empresa = Empresas(projeto, recursos.Where(r => r.EmpresaId != null))
                },
                Executoras = new Executoras()
                {
                    Executora = Executoras(recursos.Where(r => r.CoExecutorId != null))
                }
            };
        }

        private List<Empresa> Empresas(Projeto projeto, IEnumerable<RecursoHumano> recursos)
        {
            return recursos
                .GroupBy(r => r.EmpresaId)
                .Select(r => new Empresa()
                {
                    CodEmpresa = r.First().Empresa.Valor,
                    TipoEmpresa = r.First().Empresa.Id == projeto.ProponenteId,
                    Equipe = new Equipe()
                    {
                        EquipeEmpresa = EquipeEmpresas(r)
                    }
                }).ToList();
        }

        private List<Executora> Executoras(IEnumerable<RecursoHumano> recursos)
        {
            return recursos
                .GroupBy(r => r.CoExecutorId)
                .Select(a => new Executora()
                {
                    CNPJExec = a.First().CoExecutor.CNPJ,
                    UfExec = a.First().CoExecutor.UF,
                    RazaoSocialExec = a.First().CoExecutor.RazaoSocial,
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
                .Include(a => a.EmpresaRecebedora)
                .Include(a => a.CoExecutorFinanciador)
                .Include(a => a.CoExecutorRecebedor)
                .Include(a => a.RecursoMaterial).ThenInclude(r => r.CategoriaContabil)
                .Where(a => a.ProjetoId == projeto.Id);
            return new PD_Recursos()
            {
                RecursoEmpresa = RecursoEmpresa(alocacoes.Where(a => a.EmpresaFinanciadoraId != null)),
                RecursoParceira = RecursoParceira(alocacoes.Where(a => a.CoExecutorFinanciadorId != null))
            };
        }

        private List<RecursoEmpresa> RecursoEmpresa(IEnumerable<RecursoMaterial.AlocacaoRm> alocacoes)
        {
            return alocacoes.GroupBy(a => a.EmpresaFinanciadoraId).Select(a => new RecursoEmpresa()
            {
                CodEmpresa = a.First().EmpresaFinanciadora.Valor,
                DestRecursosExec = DestRecursosExec(a)
            }).ToList();
        }

        private List<RecursoParceira> RecursoParceira(IEnumerable<RecursoMaterial.AlocacaoRm> alocacoes)
        {
            return alocacoes.GroupBy(a => a.EmpresaFinanciadoraId).Select(a => new RecursoParceira()
            {
                CNPJParc = a.First().CoExecutorFinanciador.CNPJ,
                DestRecursosExec = DestRecursosExec(a)
            }).ToList();
        }

        private List<DestRecursosExec> DestRecursosExec(IEnumerable<RecursoMaterial.AlocacaoRm> alocacoes)
        {
            return alocacoes.Where(i => i.CoExecutorRecebedorId != null).GroupBy(i => i.CoExecutorRecebedorId)
                .Select(e => new DestRecursosExec()
                {
                    CNPJExec = e.First().CoExecutorRecebedor.CNPJ,
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