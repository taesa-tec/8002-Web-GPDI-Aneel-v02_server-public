using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD;
using PeD.Data;

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
            return new RelatorioFinalPeD()
            {
                PD_RelFinalBase = PD_RelFinalBase(projetoId),
                PD_EquipeEmp = PD_EquipeEmp(projetoId), // @todo Depende das empresas cooperadas e coexecutoras
                PD_EquipeExec = PD_EquipeExec(projetoId), // @todo Depende das empresas cooperadas e coexecutoras
                PD_Etapas = PD_Etapas(projetoId),
                PD_Recursos = PD_Recursos(projetoId),
                PD_Resultados = PD_Resultados(projetoId),
            };
        }

        private PD_RelFinalBase PD_RelFinalBase(int projetoId)
        {
            var projeto = _context.Set<Projeto>().FirstOrDefault(p => p.Id == projetoId) ??
                          throw new Exception($"Projeto não encontrado!");
            var relatorioFinal = _context.Set<RelatorioFinal>()
                                     .Include(rf => rf.RelatorioArquivo)
                                     .FirstOrDefault(rf => rf.ProjetoId == projetoId) ??
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

        private PD_EquipeEmp PD_EquipeEmp(int projetoId)
        {
            return new PD_EquipeEmp() { };
        }

        private PD_EquipeExec PD_EquipeExec(int projetoId)
        {
            return new PD_EquipeExec();
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
            return new PD_Recursos();
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
            var producoes = _context.Set<ProducaoCientifica>()
                .Include(pc => pc.ArquivoTrabalhoOrigem)
                .Where(pc => pc.ProjetoId == projetoId).ToList();
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