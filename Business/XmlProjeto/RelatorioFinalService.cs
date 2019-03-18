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
    public class XmlRelatorioFinalService
    {
        private GestorDbContext _context;
        private EtapaService _etapaService;
        public XmlRelatorioFinalService(GestorDbContext context, EtapaService etapaService)
        {
            _context = context;
            _etapaService = etapaService;
        }
        public Resultado ValidaXml(int ProjetoId)
        {
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Include("Empresas.Estado")
                    .Include("Etapas")
                    .Include("AlocacoesRh.RecursoHumano")
                    .Include("AlocacoesRm.RecursoMaterial")
                    .Include("Empresas.CatalogEmpresa")
                    .Include("RelatorioFinal.Uploads")
                    .Include("RegistroFinanceiro.RecursoHumano")
                    .Include("RegistroFinanceiro.RecursoMaterial")
                    .Include("ResultadosCapacitacao.RecursoHumano")
                    .Include("ResultadosCapacitacao.Uploads")
                    .Include("ResultadosProducao.Pais")
                    .Include("ResultadosProducao.Uploads")
                    .Include("ResultadosIntelectual.Inventores.RecursoHumano")
                    .Include("ResultadosIntelectual.Depositantes.Empresa.CatalogEmpresa")
                    .Include("ResultadosInfra")
                    .Include("ResultadosSocioAmbiental")
                    .Include("ResultadosEconomico")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();
    
            if (projeto == null){
                resultado.Inconsistencias.Add("Projeto não localizado");
            }else{
                if (projeto.RegistroFinanceiro.Where(r=>r.StatusValor=="Aprovado").ToList().Count()<=0)
                    resultado.Inconsistencias.Add("Não existem refps aprovados para o projeto.");
                if (projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAnual").FirstOrDefault()==null)
                    resultado.Inconsistencias.Add("Arquivo relatório final anual não localizado");
                if (projeto.ResultadosCapacitacao.Where(r=>r.Uploads==null).ToList().Count()<=0)
                    resultado.Inconsistencias.Add("Faltando Arquivo em resultados de capacitação");
                if (projeto.ResultadosProducao.Where(r=>r.Uploads==null).ToList().Count()<=0)
                    resultado.Inconsistencias.Add("Faltando Arquivo em resultados de produção");
            }
            return resultado;
        }
        public List<CustoCatContabil> ObterCustosCat(IGrouping<Empresa, AlocacaoRm> rm, List<RegistroFinanceiro> registros)
        {
            var CustoCatContabil = new List<CustoCatContabil>();
            foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabil))
            {
                decimal custo = 0;
                foreach (var rm1 in rm0)
                {
                    custo += rm1.RecursoMaterial.ValorUnitario * rm1.Qtd;
                }
                var itemDespesa = new List<ItemDespesa>();
                foreach (var registro in registros
                        .Where(p => p.RecursoMaterial != null && p.RecursoMaterial.CategoriaContabilValor == rm0.First().RecursoMaterial.CategoriaContabilValor).ToList())
                {
                    itemDespesa.Add(new ItemDespesa
                    {
                        NomeItem = registro.NomeItem,
                        JustificaItem = rm0.First().Justificativa,
                        QtdeItem = registro.QtdItens,
                        ValorIndItem = registro.ValorUnitario.ToString(),
                        TipoItem = registro.TipoValor,
                        ItemLabE = registro.EquiparLabExistente.ToString(),
                        ItemLabN = registro.EquiparLabNovo.ToString()
                    });
                }
                CustoCatContabil.Add(new CustoCatContabil
                {
                    CategoriaContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                    ItemDespesa = itemDespesa
                });
            }
            return CustoCatContabil;
        }

        private List<string> ObterMesReferencia(Projeto projeto, List<RegistroFinanceiro> registros)
        {
            DateTime? DataInicio = projeto.DataInicio;
            string MesMb = null;
            string HoraMesMb = null;
            foreach (var mes in registros)
            {
                DateTime? DataAprov = mes.Mes;
                if (DataAprov != null && DataInicio != null)
                {
                    int nMonths = 0;
                    if (DataInicio.Value.Year == DataAprov.Value.Year)
                        nMonths = DataAprov.Value.Month - DataInicio.Value.Month;
                    else
                        nMonths = (12 - DataInicio.Value.Month) + DataAprov.Value.Month;
                    MesMb += (nMonths + 1) + ",";
                    HoraMesMb += (mes.QtdHrs + "/" + (nMonths + 1)) + ",";
                }
            }
            return new List<string>{ MesMb.TrimEnd(','),HoraMesMb.TrimEnd(',') };
        }
        public XmlRelatorioFinal GerarXml(int ProjetoId, string Versao, string UserId)
        {
            XmlRelatorioFinal relatorio = new XmlRelatorioFinal();
            Projeto projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Include("Empresas.Estado")
                        .Include("Etapas")
                        .Include("AlocacoesRh.RecursoHumano")
                        .Include("AlocacoesRm.RecursoMaterial")
                        .Include("Empresas.CatalogEmpresa")
                        .Include("RelatorioFinal.Uploads")
                        .Where(p => p.Id == ProjetoId)
                        .FirstOrDefault();

            var registros = _context.RegistrosFinanceiros
                            .Include("RecursoHumano")
                            .Include("RecursoMaterial")
                            .Where(p => p.ProjetoId == ProjetoId)
                            .Where(p => p.StatusValor == "Aprovado")
                            .ToList();

            int?[] rhIds = registros.Where(r => r.RecursoHumano != null).Select(r => r.RecursoHumanoId).ToArray();
            int?[] rmIds = registros.Where(r => r.RecursoMaterial != null).Select(r => r.RecursoMaterialId).ToArray();

                relatorio.PD_RelFinalBase = new PD_RelFinalBase
                {
                    CodProjeto = projeto.Codigo,
                    ArquivoPDF = projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAnual").FirstOrDefault().NomeArquivo,
                    DataIniODS = projeto.DataInicio.ToString(),
                    DataFimODS = (projeto.Etapas.LastOrDefault().DataFim==null)? _etapaService.AddDataEtapas(projeto.Etapas).LastOrDefault().DataFim.ToString() : projeto.Etapas.LastOrDefault().DataFim.ToString(),
                    ProdPrev = projeto.RelatorioFinal.ProdutoAlcancado.ToString(),
                    ProdJust = projeto.RelatorioFinal.JustificativaProduto,
                    ProdEspTec = projeto.RelatorioFinal.EspecificacaoProduto,
                    TecPrev = projeto.RelatorioFinal.TecnicaPrevista.ToString(),
                    TecJust = projeto.RelatorioFinal.JustificativaTecnica,
                    TecDesc = projeto.RelatorioFinal.DescTecnica,
                    AplicPrev = projeto.RelatorioFinal.AplicabilidadePrevista.ToString(),
                    AplicJust = projeto.RelatorioFinal.JustificativaAplicabilidade,
                    AplicFnc = projeto.RelatorioFinal.DescTestes,
                    AplicAbrang = projeto.RelatorioFinal.DescAbrangencia,
                    AplicAmbito = projeto.RelatorioFinal.DescAmbito,
                    TxDifTec = projeto.RelatorioFinal.DescAtividades
                };
                // PD_EQUIPEEMP
                var PedEmpresaList = new List<PedEmpresa>();
                var EmpresasFinanciadoras = projeto.Empresas
                    .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                    .ToList();
                foreach (Empresa empresa in EmpresasFinanciadoras)
                {
                    var equipeList = new List<EquipeEmpresa>();
                    foreach (AlocacaoRh alRh in projeto.AlocacoesRh
                        .Where(p => p.RecursoHumano.CPF != null) // somente brasileiros
                        .Where(p => p.RecursoHumano.Empresa == empresa)
                        .Where(p => rhIds.Contains(p.RecursoHumano.Id))
                        .ToList())
                    {
                        var strMesHora = ObterMesReferencia(projeto, registros.Where(r => r.RecursoHumanoId == alRh.RecursoHumanoId).ToList());
                        equipeList.Add(new EquipeEmpresa
                        {
                            NomeMbEqEmp = alRh.RecursoHumano.NomeCompleto,
                            CpfMbEqEmp = alRh.RecursoHumano.CPF,
                            TitulacaoMbEqEmp = alRh.RecursoHumano.TitulacaoValor,
                            FuncaoMbEqEmp = alRh.RecursoHumano.FuncaoValor,
                            HhMbEqEmp = alRh.RecursoHumano.ValorHora.ToString(),
                            MesMbEqEmp = strMesHora[0],
                            HoraMesMbEqEmp = strMesHora[1]
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
                // PD_EQUIPEEXEC
                var PedExecutoraList = new List<PedExecutora>();
                foreach (Empresa empresa in projeto.Empresas
                    .Where(p => p.ClassificacaoValor == "Executora")
                    .ToList())
                {
                    var equipeList = new List<EquipeExec>();
                    foreach (AlocacaoRh alRh in projeto.AlocacoesRh
                        .Where(p => p.RecursoHumano.Empresa == empresa)
                        .Where(p => rhIds.Contains(p.RecursoHumano.Id))
                        .ToList())
                    {
                        var strMesHora = ObterMesReferencia(projeto, registros.Where(r => r.RecursoHumanoId == alRh.RecursoHumanoId).ToList());
                        
                        equipeList.Add(new EquipeExec
                        {
                            NomeMbEqExec = alRh.RecursoHumano.NomeCompleto,
                            BRMbEqExec = alRh.RecursoHumano.NacionalidadeValor,
                            DocMbEqExec = alRh.RecursoHumano.CPF ?? alRh.RecursoHumano.Passaporte,
                            TitulacaoMbEqExec = alRh.RecursoHumano.TitulacaoValor,
                            FuncaoMbEqExec = alRh.RecursoHumano.FuncaoValor,
                            HhMbEqExec = alRh.RecursoHumano.ValorHora.ToString(),
                            MesMbEqExec = strMesHora[0],
                            HoraMesMbEqExec = strMesHora[1]
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
                relatorio.PD_EquipeEmp = new PD_EquipeEmp
                {
                    Empresas = new PedEmpresas
                    {
                        Empresa = PedEmpresaList
                    }
                };
                relatorio.PD_EquipeExec = new PD_EquipeExec
                {
                    Executoras = new PedExecutoras
                    {
                        Executora = PedExecutoraList
                    }
                };
                // PD_ETAPAS
                var EtapasList = new List<PD_Etapa>();
                int ordem = 1;
                int anterior = 0;
                foreach (Etapa etapa in projeto.Etapas.OrderBy(e => e.Id))
                {
                    string mesExecEtapa = null;
                    int duracao = etapa.Duracao;
                    var etapalist = new List<string>();
                    int last = 0;
                    for (int a = anterior + 1; a <= (anterior + duracao); a++)
                    {
                        etapalist.Add((a).ToString());
                        last = a;
                    }
                    anterior = last;
                    mesExecEtapa = String.Join(",", etapalist);

                    EtapasList.Add(new PD_Etapa
                    {
                        EtapaN = ordem.ToString().PadLeft(2, '0'),
                        Atividades = etapa.AtividadesRealizadas,
                        MesExecEtapa = mesExecEtapa
                    });
                    ordem++;
                }
                relatorio.PD_Etapas = new PD_Etapas
                {
                    Etapa = EtapasList
                };
                // PD_RECURSO
                relatorio.PD_Recursos = new RF_Recursos
                {
                    RecursoEmpresa = new List<RF_RecursoEmpresa>(),
                    RecursoParceira = new List<RF_RecursoParceira>()
                };
                foreach (Empresa empresa in EmpresasFinanciadoras)
                {
                    var DestRecursosExec = new List<RF_DestRecursosExec>();
                    foreach (var rm in projeto.AlocacoesRm
                        .Where(p => p.EmpresaRecebedora.ClassificacaoValor == "Executora")
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList())
                    {
                        DestRecursosExec.Add(new RF_DestRecursosExec
                        {
                            CNPJExec = rm.First().EmpresaRecebedora.Cnpj,
                            CustoCatContabil = ObterCustosCat(rm, registros)
                        });
                    }

                    var DestRecursosEmp = new List<RF_DestRecursosEmp>();
                    foreach (var rm in projeto.AlocacoesRm
                            .Where(p => p.EmpresaRecebedora == empresa)
                            .Where(p => p.EmpresaFinanciadora == empresa)
                            .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                            .GroupBy(p => p.EmpresaRecebedora)
                            .ToList())
                    {
                        DestRecursosEmp.Add(new RF_DestRecursosEmp
                        {
                            CustoCatContabil = ObterCustosCat(rm, registros)
                        });
                    }

                    relatorio.PD_Recursos.RecursoEmpresa.Add(new RF_RecursoEmpresa
                    {
                        CodEmpresa = empresa.CatalogEmpresa.Valor,
                        DestRecursos = new DestRecursos
                        {
                            DestRecursosEmp = DestRecursosEmp,
                            DestRecursosExec = DestRecursosExec
                        }
                    });
                }

                foreach (Empresa empresa in projeto.Empresas
                    .Where(p => p.ClassificacaoValor == "Parceira")
                    .ToList())
                {
                    var DestRecursosExec = new List<RF_DestRecursosExec>();
                    foreach (var rm in projeto.AlocacoesRm
                        .Where(p => p.EmpresaRecebedora.ClassificacaoValor == "Executora")
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList())
                    {
                        DestRecursosExec.Add(new RF_DestRecursosExec
                        {
                            CNPJExec = rm.First().EmpresaRecebedora.Cnpj,
                            CustoCatContabil = ObterCustosCat(rm, registros)
                        });
                    }

                    relatorio.PD_Recursos.RecursoParceira.Add(new RF_RecursoParceira
                    {
                        CNPJParc = empresa.Cnpj,
                        DestRecursosExec = DestRecursosExec
                    });
                }

                // PD_RESULTADO
                relatorio.PD_Resultados = new PD_Resultados
                {
                    PD_ResultadosCP = new PD_ResultadosCP(),
                    PD_ResultadosCT = new PD_ResultadosCT
                    {
                        PD_ResultadosCT_PC = new PD_ResultadosCT_PC(),
                        PD_ResultadosCT_IE = new PD_ResultadosCT_IE(),
                        PD_ResultadosCT_PI = new PD_ResultadosCT_PI()
                    },
                    PD_ResultadosSA = new PD_ResultadosSA(),
                    PD_ResultadosIE = new PD_ResultadosIE()
                };
                var listIdCp = new List<IdCP>();
                foreach (ResultadoCapacitacao rCp in _context.ResultadosCapacitacao.Include("RecursoHumano").Include("Uploads").Where(r => r.ProjetoId == ProjetoId).ToList())
                {
                    listIdCp.Add(new IdCP
                    {
                        TipoCP = rCp.TipoValor,
                        ConclusaoCP = rCp.Conclusao.ToString(),
                        DataCP = rCp.DataConclusao.ToString(),
                        DocMmbEqCP = (rCp.RecursoHumano.CPF != null) ? rCp.RecursoHumano.CPF : rCp.RecursoHumano.Passaporte,
                        CNPJInstCP = rCp.CnpjInstituicao,
                        AreaCP = rCp.AreaPesquisa,
                        TituloCP = rCp.TituloTrabalho,
                        ArquivoPDF = rCp.Uploads.First().NomeArquivo,
                    });
                }
                relatorio.PD_Resultados.PD_ResultadosCP.IdCP = listIdCp;

                var listIdCT_PC = new List<IdCT_PC>();
                foreach (ResultadoProducao rCT_PC in _context.ResultadosProducao.Include("Pais").Include("Uploads").Where(r => r.ProjetoId == ProjetoId).ToList())
                {
                    listIdCT_PC.Add(new IdCT_PC
                    {
                        TipoCT_PC = rCT_PC.TipoValor,
                        ConfPubCT_PC = rCT_PC.Confirmacao.ToString(),
                        DataCT_PC = rCT_PC.DataPublicacao.ToString(),
                        NomeCT_PC = rCT_PC.Nome,
                        LinkCT_PC = rCT_PC.Url,
                        PaisCT_PC = rCT_PC.Pais.Nome,
                        CidadeCT_PC = rCT_PC.Cidade,
                        TituloCT_PC = rCT_PC.Titulo,
                        ArquivoPDF = rCT_PC.Uploads.First().NomeArquivo,
                    });
                }
                relatorio.PD_Resultados.PD_ResultadosCT.PD_ResultadosCT_PC.IdCT_PC = listIdCT_PC;

                var listIdCT_IE = new List<IdCT_IE>();
                foreach (ResultadoInfra rCT_IE in _context.ResultadosInfra.Where(r => r.ProjetoId == ProjetoId).ToList())
                {
                    listIdCT_IE.Add(new IdCT_IE
                    {
                        TipoCT_IE = rCT_IE.TipoValor,
                        CNPJInstBenefCT_IE = rCT_IE.CnpjReceptora,
                        NomeLabCT_IE = rCT_IE.NomeLaboratorio,
                        AreaLabCT_IE = rCT_IE.AreaPesquisa,
                        ApoioLabCT_IE = rCT_IE.ListaMateriais
                    });
                }
                relatorio.PD_Resultados.PD_ResultadosCT.PD_ResultadosCT_IE.IdCT_IE = listIdCT_IE;

                var listIdCT_PI = new List<IdCT_PI>();
                foreach (ResultadoIntelectual rCT_PI in _context.ResultadosIntelectual.Include("Inventores.RecursoHumano").Include("Depositantes.Empresa.CatalogEmpresa").Where(r => r.ProjetoId == ProjetoId).ToList())
                {
                    var listIvts = new List<Inventor_PI>();
                    foreach (ResultadoIntelectualInventor ivt in rCT_PI.Inventores)
                    {
                        listIvts.Add(new Inventor_PI
                        {
                            DocMbEqCT_PI = (ivt.RecursoHumano.CPF != null) ? ivt.RecursoHumano.CPF : ivt.RecursoHumano.Passaporte
                        });
                    }
                    var listDpts = new List<Depositante_PI>();
                    foreach (ResultadoIntelectualDepositante dpt in rCT_PI.Depositantes)
                    {
                        listDpts.Add(new Depositante_PI
                        {
                            CNPJInstCT_PI = (dpt.Empresa.Cnpj != null) ? dpt.Empresa.Cnpj : dpt.Empresa.CatalogEmpresa.Cnpj,
                            PercInstCT_PI = dpt.Entidade.ToString()
                        });
                    }
                    listIdCT_PI.Add(new IdCT_PI
                    {
                        TipoCT_PI = rCT_PI.TipoValor,
                        DataCT_PI = rCT_PI.DataPedido.ToString(),
                        NumeroCT_PI = rCT_PI.NumeroPedido,
                        TituloCT_PI = rCT_PI.Titulo,
                        Inventores_PI = new Inventores_PI
                        {
                            Inventor = listIvts
                        },
                        Depositantes_PI = new Depositantes_PI
                        {
                            Depositante = listDpts
                        }
                    });
                }
                relatorio.PD_Resultados.PD_ResultadosCT.PD_ResultadosCT_PI.IdCT_PI = listIdCT_PI;

                // PD_RESULTADOS_SA
                var listIdSA = new List<IdSA>();
                foreach (ResultadoSocioAmbiental rSa in _context.ResultadosSocioAmbiental.Where(r => r.ProjetoId == ProjetoId).ToList())
                {
                    listIdSA.Add(new IdSA
                    {
                        TipoISA = rSa.TipoValor,
                        PossibISA = rSa.Positivo.ToString(),
                        TxtISA = rSa.Desc
                    });
                }
                relatorio.PD_Resultados.PD_ResultadosSA.IdSA = listIdSA;

                // PD_RESULTADOS_IE
                var listIdIE = new List<IdIE>();
                foreach (ResultadoEconomico rIe in _context.ResultadosEconomico.Where(r => r.ProjetoId == ProjetoId).ToList())
                {
                    listIdIE.Add(new IdIE
                    {
                        TipoIE = rIe.TipoValor,
                        TxtBenefIE = rIe.Desc,
                        UnidBenefIE = rIe.UnidadeBase,
                        BaseBenefIE = rIe.ValorIndicador.ToString(),
                        PerBenefIE = rIe.Percentagem.ToString(),
                        VlrBenefIE = rIe.ValorBeneficio.ToString()
                    });
                }
                relatorio.PD_Resultados.PD_ResultadosIE.IdIE = listIdIE;
            return relatorio;
        }
    }
}
