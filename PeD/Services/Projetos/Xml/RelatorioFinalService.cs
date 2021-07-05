using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Models.Projetos.Xml;
using PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD;
using PeD.Data;
using DestRecursosEmp = PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD.DestRecursosEmp;
using DestRecursosExec = PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD.DestRecursosExec;
using PD_Recursos = PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD.PD_Recursos;
using RecursoEmpresa = PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD.RecursoEmpresa;
using RecursoParceira = PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD.RecursoParceira;

namespace PeD.Services.Projetos.Xml
{
    public class RelatorioFinalService
    {
        private GestorDbContext _context;

        public RelatorioFinalService(GestorDbContext context)
        {
            _context = context;
        }

        public RelatorioFinalPeD RelatorioFinalPeD(int projetoId)
        {
            var projeto = _context.Set<Projeto>().AsNoTracking().FirstOrDefault(p => p.Id == projetoId) ??
                          throw new Exception("Projeto não encontrato");
            var registros = _context.Set<RegistroFinanceiroInfo>()
                .Where(r => r.ProjetoId == projetoId && r.Status == StatusRegistro.Aprovado).ToList();
            return new RelatorioFinalPeD()
            {
                PD_RelFinalBase = PD_RelFinalBase(projeto), // Projeto RelatorioFinal
                PD_EquipeEmp = PD_EquipeEmp(projeto, registros), // Projeto RegistosFinaneceiroInfo
                PD_EquipeExec = PD_EquipeExec(projeto, registros), // Projeto RegistosFinaneceiroInfo
                PD_Etapas = PD_Etapas(projetoId), // RelatorioEtapa
                PD_Recursos = PD_Recursos(projetoId), // RegistroFinanceiroInfo
                PD_Resultados = PD_Resultados(projetoId),
            };
        }

        private PD_RelFinalBase PD_RelFinalBase(Projeto projeto)
        {
            var relatorioFinal = _context.Set<RelatorioFinal>()
                                     .Include(rf => rf.RelatorioArquivo)
                                     .FirstOrDefault(rf => rf.ProjetoId == projeto.Id) ??
                                 throw new Exception("Relatorio não encontrado!");
            return new PD_RelFinalBase()
            {
                CodProjeto = projeto.Codigo,
                DataIniODS = projeto.DataInicioProjeto,
                DataFimODS = projeto.DataFinalProjeto,
                ArquivoPDF = relatorioFinal.RelatorioArquivo?.FileName ?? "Não encontrado",
                ProdPrev = relatorioFinal.IsProdutoAlcancado,
                ProdJust = relatorioFinal.IsProdutoAlcancado ? "" : relatorioFinal.TecnicaProduto,
                ProdEspTec = relatorioFinal.IsProdutoAlcancado ? relatorioFinal.TecnicaProduto : "",
                TecPrev = relatorioFinal.IsTecnicaImplementada,
                TecJust = relatorioFinal.IsTecnicaImplementada ? "" : relatorioFinal.TecnicaImplementada,
                TecDesc = relatorioFinal.IsTecnicaImplementada ? relatorioFinal.TecnicaImplementada : "",
                AplicPrev = relatorioFinal.IsAplicabilidadeAlcancada,
                AplicJust = relatorioFinal.IsAplicabilidadeAlcancada ? "" : relatorioFinal.AplicabilidadeJustificativa,
                AplicFnc = relatorioFinal.IsAplicabilidadeAlcancada ? relatorioFinal.ResultadosTestes : "",
                AplicAbrang = relatorioFinal.IsAplicabilidadeAlcancada ? relatorioFinal.AbrangenciaProduto : "",
                AplicAmbito = relatorioFinal.IsAplicabilidadeAlcancada ? relatorioFinal.AmbitoAplicacaoProduto : "",
                TxDifTec = relatorioFinal.TransferenciaTecnologica
            };
        }

        private PD_EquipeEmp PD_EquipeEmp(Projeto projeto, List<RegistroFinanceiroInfo> registros)
        {
            var rhRegistros = registros.Where(r => r.CategoriaContabilCodigo == "RH").ToList();
            var rhIds = rhRegistros.Where(r => r.RecursoHumanoId.HasValue).Select(r => r.RecursoHumanoId.Value)
                .ToList();
            var recursosHumanos = _context.Set<RecursoHumano>()
                .Include(r => r.Empresa)
                .Where(r => r.ProjetoId == projeto.Id && r.EmpresaId != null && rhIds.Contains(r.Id))
                .ToList()
                .GroupBy(r => r.EmpresaId);


            return new PD_EquipeEmp()
            {
                Empresas = new PedEmpresas()
                {
                    Empresa = recursosHumanos.Select(e => new PedEmpresa()
                    {
                        CodEmpresa = e.First().Empresa.Valor,
                        TipoEmpresa = e.First().Empresa.Id == projeto.ProponenteId ? "P" : "C",
                        Equipe = new Equipe()
                        {
                            EquipeEmpresa = e.Select(r => new EquipeEmpresa()
                            {
                                NomeMbEqEmp = r.NomeCompleto,
                                CpfMbEqEmp = r.Documento,
                                TitulacaoMbEqEmp = r.Titulacao,
                                FuncaoMbEqEmp = r.Funcao,
                                HhMbEqEmp = r.ValorHora,
                                MesMbEqEmp = MesMbEqEmp(registros.Where(rf => rf.RecursoHumanoId == r.Id),
                                    projeto.DataInicioProjeto),
                                HoraMesMbEqEmp = HoraMesMbEqEmp(registros.Where(rf => rf.RecursoHumanoId == r.Id))
                            }).ToList()
                        }
                    }).ToList()
                }
            };
        }

        protected string MesMbEqEmp(IEnumerable<RegistroFinanceiroInfo> registros, DateTime inicio)
        {
            Func<DateTime, int> mesesDiff = (fim) =>
                1 + (fim.Year - inicio.Year) * 12 + fim.Month - inicio.Month;
            var meses = registros.OrderBy(rf => rf.MesReferencia).Select(rf => mesesDiff(rf.MesReferencia));
            return string.Join(',', meses);
        }

        protected string HoraMesMbEqEmp(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            var horas = registros.OrderBy(rf => rf.MesReferencia).Select(rf => rf.QuantidadeHoras);
            return string.Join(',', horas);
        }

        private PD_EquipeExec PD_EquipeExec(Projeto projeto, List<RegistroFinanceiroInfo> registros)
        {
            var rHregistros = registros
                .Where(r => r.CategoriaContabilCodigo == "RH").ToList();
            var rhIds = rHregistros.Where(r => r.RecursoHumanoId.HasValue).Select(r => r.RecursoHumanoId.Value);
            var recursosHumanos = _context.Set<RecursoHumano>()
                .Include(r => r.CoExecutor)
                .Where(r => r.ProjetoId == projeto.Id && r.CoExecutorId != null && rhIds.Contains(r.Id))
                .ToList()
                .GroupBy(r => r.CoExecutorId);


            return new PD_EquipeExec()
            {
                Executoras = new PedExecutoras()
                {
                    Executora = recursosHumanos.Select(r => new PedExecutora()
                    {
                        UfExec = r.First().CoExecutor.UF,
                        RazaoSocialExec = r.First().CoExecutor.RazaoSocial,
                        CNPJExec = r.First().CoExecutor.CNPJ,
                        Equipe = new ExecEquipe()
                        {
                            EquipeExec = r.Select(rh => new EquipeExec()
                            {
                                NomeMbEqExec = rh.NomeCompleto,
                                BRMbEqExec = rh.Nacionalidade,
                                DocMbEqExec = rh.Documento,
                                TitulacaoMbEqExec = rh.Titulacao,
                                FuncaoMbEqExec = rh.Funcao,
                                HhMbEqExec = rh.ValorHora,
                                MesMbEqExec = MesMbEqEmp(registros.Where(rf => rf.RecursoHumanoId == rh.Id),
                                    projeto.DataInicioProjeto),
                                HoraMesMbEqExec = HoraMesMbEqEmp(registros.Where(rf => rf.RecursoHumanoId == rh.Id))
                            }).ToList()
                        }
                    }).ToList()
                }
            };
        }

        private PD_Etapas PD_Etapas(int projetoId)
        {
            var etapas = _context.Set<RelatorioEtapa>()
                .Include(e => e.Etapa)
                .Where(re => re.ProjetoId == projetoId).ToList();
            return new PD_Etapas()
            {
                Etapa = etapas.Select(etapa => new PD_Etapa()
                {
                    Atividades = etapa.AtividadesRealizadas,
                    EtapaN = etapa.Etapa.Ordem,
                    MesExecEtapa = string.Join(',', etapa.Etapa.Meses)
                }).ToList()
            };
        }

        private PD_Recursos PD_Recursos(int projetoId)
        {
            var registros = _context.Set<RegistroFinanceiroInfo>()
                .Where(r => r.ProjetoId == projetoId
                            && r.Status == StatusRegistro.Aprovado).ToList();

            return new PD_Recursos()
            {
                RecursoEmpresa =
                    RecursoEmpresa(registros.Where(r => r.FinanciadoraId != null && r.CategoriaContabilCodigo != "RH")),
                RecursoParceira = RecursoParceira(registros.Where(r => r.CoExecutorFinanciadorId != null))
            };
        }

        private List<RecursoEmpresa> RecursoEmpresa(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return registros.GroupBy(r => r.FinanciadoraId).Select(r => new RecursoEmpresa()
            {
                CodEmpresa = r.First().CodEmpresa,
                DestRecursos = new DestRecursos()
                {
                    DestRecursosEmp = DestRecursosEmp(r.Where(i => i.FinanciadoraId == i.RecebedoraId)),
                    DestRecursosExec = DestRecursosExec(r.Where(i => i.CoExecutorRecebedorId != null))
                }
            }).ToList();
        }

        private DestRecursosEmp DestRecursosEmp(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return new DestRecursosEmp()
            {
                CustoCatContabil = CustoCatContabil(registros)
            };
        }

        private List<DestRecursosExec> DestRecursosExec(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return registros.GroupBy(r => r.CoExecutorRecebedorId).Select(i => new DestRecursosExec()
            {
                CNPJExec = i.First().CNPJExec,
                CustoCatContabil = CustoCatContabil(i)
            }).ToList();
        }

        private List<CustoCatContabil<ItemDespesa>> CustoCatContabil(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return registros.GroupBy(r => r.CategoriaContabilCodigo)
                .Select(r => new CustoCatContabil<ItemDespesa>()
                {
                    CategoriaContabil = r.Key,
                    ItemDespesa = r.Select(i => new ItemDespesa()
                    {
                        NomeItem = i.Recurso,
                        JustificaItem = i.FuncaoEtapa,
                        QtdeItem = (int) i.QuantidadeHoras,
                        ValorIndItem = i.Valor,
                        TipoItem = i.IsNacional.HasValue && i.IsNacional.Value,
                        ItemLabE = i.EquipaLaboratorioExistente.HasValue && i.EquipaLaboratorioExistente.Value,
                        ItemLabN = i.EquipaLaboratorioNovo.HasValue && i.EquipaLaboratorioNovo.Value
                    }).ToList()
                })
                .ToList();
        }

        private List<RecursoParceira> RecursoParceira(IEnumerable<RegistroFinanceiroInfo> registros)
        {
            return registros.GroupBy(r => r.CoExecutorFinanciadorId).Select(r => new RecursoParceira()
            {
                CNPJParc = r.First().CNPJParc,
                DestRecursosExec = DestRecursosExec(r)
            }).ToList();
        }

        private PD_Resultados PD_Resultados(int projetoId)
        {
            return new PD_Resultados()
            {
                PD_ResultadosCP = PD_ResultadosCP(projetoId),
                PD_ResultadosCT = PD_ResultadosCT(projetoId),
                PD_ResultadosIE = PD_ResultadosIE(projetoId),
                PD_ResultadosSA = PD_ResultadosSA(projetoId)
            };
        }

        private PD_ResultadosCP PD_ResultadosCP(int projetoId)
        {
            var capacitacoes = _context.Set<Capacitacao>()
                .Include(c => c.Recurso).Include(c => c.ArquivoTrabalhoOrigem).Where(c => c.ProjetoId == projetoId)
                .ToList();

            return new PD_ResultadosCP()
            {
                IdCP = capacitacoes.Select(cp => new IdCP()
                {
                    TipoCP = cp.Tipo.ToString(),
                    ConclusaoCP = cp.IsConcluido,
                    DataCP = cp.DataConclusao.Value,
                    DocMmbEqCP = cp.Recurso.Documento,
                    CNPJInstCP = cp.CnpjInstituicao,
                    AreaCP = cp.AreaPesquisa,
                    TituloCP = cp.TituloTrabalhoOrigem,
                    ArquivoPDF = cp.ArquivoTrabalhoOrigem?.FileName
                }).ToList()
            };
        }

        private PD_ResultadosCT PD_ResultadosCT(int projetoId)
        {
            return new PD_ResultadosCT()
            {
                PD_ResultadosCT_PC = PD_ResultadosCT_PC(projetoId),
                PD_ResultadosCT_IE = PD_ResultadosCT_IE(projetoId),
                PD_ResultadosCT_PI = PD_ResultadosCT_PI(projetoId),
            };
        }

        private PD_ResultadosCT_PC PD_ResultadosCT_PC(int projetoId)
        {
            var producoes = _context.Set<ProducaoCientifica>()
                .Include(pc => pc.ArquivoTrabalhoOrigem)
                .Include(pc => pc.Pais)
                .Where(pc => pc.ProjetoId == projetoId).ToList();
            return new PD_ResultadosCT_PC()
            {
                IdCT_PC = producoes.Select(pc => new IdCT_PC()
                {
                    TipoCT_PC = pc.Tipo.ToString(),
                    ConfPubCT_PC = pc.ConfirmacaoPublicacao,
                    DataCT_PC = pc.DataPublicacao,
                    NomeCT_PC = pc.NomeEventoPublicacao,
                    LinkCT_PC = pc.LinkPublicacao,
                    PaisCT_PC = pc.Pais.Nome,
                    CidadeCT_PC = pc.Cidade,
                    TituloCT_PC = pc.TituloTrabalho,
                    ArquivoPDF = pc.ArquivoTrabalhoOrigem?.FileName,
                }).ToList()
            };
        }

        private PD_ResultadosCT_IE PD_ResultadosCT_IE(int projetoId)
        {
            var apoios = _context.Set<Apoio>().Where(a => a.ProjetoId == projetoId).ToList();
            return new PD_ResultadosCT_IE()
            {
                IdCT_IE = apoios.Select(a => new IdCT_IE()
                {
                    TipoCT_IE = a.Tipo.ToString(),
                    CNPJInstBenefCT_IE = a.CnpjReceptora,
                    NomeLabCT_IE = a.Laboratorio,
                    AreaLabCT_IE = a.LaboratorioArea,
                    ApoioLabCT_IE = a.MateriaisEquipamentos
                }).ToList()
            };
        }

        private PD_ResultadosCT_PI PD_ResultadosCT_PI(int projetoId)
        {
            var propriedades = _context.Set<PropriedadeIntelectual>()
                .Include(p => p.Depositantes)
                .ThenInclude(d => d.CoExecutor)
                .Include(p => p.Depositantes)
                .ThenInclude(d => d.Empresa)
                .Include(p => p.Inventores)
                .ThenInclude(i => i.Recurso)
                .Where(p => p.ProjetoId == projetoId).ToList();
            return new PD_ResultadosCT_PI()
            {
                IdCT_PI = propriedades.Select(p => new IdCT_PI()
                {
                    TipoCT_PI = p.Tipo.ToString(),
                    DataCT_PI = p.PedidoData,
                    NumeroCT_PI = p.PedidoNumero,
                    TituloCT_PI = p.TituloINPI,
                    Inventores_PI = new Inventores_PI()
                    {
                        Inventor = p.Inventores.Select(i => new Inventor_PI()
                        {
                            DocMbEqCT_PI = i.Recurso.Documento
                        }).ToList()
                    },
                    Depositantes_PI = new Depositantes_PI()
                    {
                        Depositante = p.Depositantes.Select(d => new Depositante_PI()
                        {
                            PercInstCT_PI = d.Porcentagem,
                            CNPJInstCT_PI = d.Empresa?.Cnpj ?? d.CoExecutor?.CNPJ
                        }).ToList()
                    }
                }).ToList()
            };
        }

        private PD_ResultadosSA PD_ResultadosSA(int projetoId)
        {
            var socioambientais = _context.Set<Socioambiental>().Where(p => p.ProjetoId == projetoId).ToList();
            return new PD_ResultadosSA()
            {
                IdSA = socioambientais.Select(s => new IdSA()
                {
                    TipoISA = s.Tipo.ToString(),
                    PossibISA = s.ResultadoPositivo,
                    TxtISA = s.DescricaoResultado
                }).ToList()
            };
        }

        private PD_ResultadosIE PD_ResultadosIE(int projetoId)
        {
            var indicadores = _context.Set<IndicadorEconomico>().Where(ie => ie.ProjetoId == projetoId).ToList();
            return new PD_ResultadosIE()
            {
                IdIE = indicadores.Select(ie => new IdIE()
                {
                    TipoIE = ie.Tipo.ToString(),
                    TxtBenefIE = ie.Beneficio,
                    UnidBenefIE = ie.UnidadeBase,
                    BaseBenefIE = ie.ValorNumerico,
                    PerBenefIE = ie.PorcentagemImpacto,
                    VlrBenefIE = ie.ValorBeneficio
                }).ToList()
            };
        }
    }
}