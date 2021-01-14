using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Data;
using PeD.Models.Projetos;
using PeD.Models.Projetos.Resultados;
using PeD.Models.Projetos.Xmls;

namespace PeD.Services.Projetos.XmlProjeto {
    public class XmlRelatorioFinalGestaoService : IXmlService<XmlRelatorioFinalGestao> {
        private GestorDbContext _context;
        private EtapaService _etapaService;
        public XmlRelatorioFinalGestaoService( GestorDbContext context, EtapaService etapaService ) {
            _context = context;
            _etapaService = etapaService;
        }

        public Resultado ValidaXml( int ProjetoId ) {
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            Projeto projeto = _context.Projetos
                    //.Include("CatalogEmpresa")
                    //.Include("Empresas.Estado")
                    //.Include("Etapas")
                    // .Include("AlocacoesRh.RecursoHumano")
                    // .Include("AlocacoesRm.RecursoMaterial")
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

            if(projeto == null) {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            else {
                if(projeto.RegistroFinanceiro.Where(r => r.StatusValor == "Aprovado").ToList().Count() <= 0)
                    resultado.Inconsistencias.Add("Não existem refps aprovados para o projeto.");

                if(projeto.RelatorioFinal != null) {
                    if(projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAnual").FirstOrDefault() == null)
                        resultado.Inconsistencias.Add("Arquivo relatório final anual não localizado");
                }
                else {
                    resultado.Inconsistencias.Add("Não há Relatório final cadastrado");
                }

                if(projeto.ResultadosCapacitacao.Where(r => r.Uploads == null).ToList().Count() > 0)
                    resultado.Inconsistencias.Add("Faltando Arquivo em resultados de capacitação");
                if(projeto.ResultadosProducao.Where(r => r.Uploads == null).ToList().Count() > 0)
                    resultado.Inconsistencias.Add("Faltando Arquivo em resultados de produção");
            }
            return resultado;
        }

        public List<CustoCatContabil<ItemDespesaBase>> ObterCustosCat( List<RegistroFinanceiro> registros ) {
            var CustoCatContabil = new List<CustoCatContabil<ItemDespesaBase>>();
            var cats = from r in registros
                       group r by r.CategoriaContabilGestao.Id;

            foreach(var cat in cats) {

                var itemDespesa = from item in cat.ToList()
                                  select new ItemDespesaBase {
                                      NomeItem = item.NomeItem,
                                      JustificaItem = item.FuncaoRecurso,
                                      QtdeItem = item.QtdItens,
                                      ValorIndItem = item.ValorUnitario.ToString(),
                                      TipoItem = item.TipoValor
                                  };

                CustoCatContabil.Add(new CustoCatContabil<ItemDespesaBase> {
                    CategoriaContabil = cat.First().CategoriaContabilGestao.Valor,
                    ItemDespesa = itemDespesa.ToList()
                });
            }

            return CustoCatContabil;
        }

        private List<string> ObterMesReferencia( Projeto projeto, List<RegistroFinanceiro> registros ) {
            DateTime? DataInicio = projeto.DataInicio;
            string MesMb = null;
            string HoraMesMb = null;
            foreach(var mes in registros) {
                DateTime? DataAprov = mes.Mes;
                if(DataAprov != null && DataInicio != null) {
                    int nMonths = 0;
                    if(DataInicio.Value.Year == DataAprov.Value.Year)
                        nMonths = DataAprov.Value.Month - DataInicio.Value.Month;
                    else
                        nMonths = (12 - DataInicio.Value.Month) + DataAprov.Value.Month;
                    MesMb += (nMonths + 1) + ",";
                    HoraMesMb += (mes.QtdHrs + "/" + (nMonths + 1)) + ",";
                }
            }
            return new List<string> { MesMb.TrimEnd(','), HoraMesMb.TrimEnd(',') };
        }

        public XmlRelatorioFinalGestao GerarXml( int ProjetoId, string Versao, string UserId ) {
            XmlRelatorioFinalGestao relatorio = new XmlRelatorioFinalGestao();
            Projeto projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Include("Empresas.Estado")
                        .Include("Etapas.EtapaMeses")
                        .Include("Atividades")
                        //.Include("AlocacoesRh.RecursoHumano")
                        //.Include("AlocacoesRm.RecursoMaterial.CategoriaContabilGestao")
                        .Include("Empresas.CatalogEmpresa")
                        .Include("RelatorioFinal.Uploads")
                        .Where(p => p.Id == ProjetoId)
                        .FirstOrDefault();

            var registros = _context.RegistrosFinanceiros
                            .Include("RecursoHumano")
                            .Include("RecursoMaterial.CategoriaContabilGestao")
                            .Include("CategoriaContabilGestao")
                            .Include("Atividade")
                            .Where(p => p.ProjetoId == ProjetoId)
                            .Where(p => p.StatusValor == "Aprovado")
                            .ToList();

            int?[] rhIds = registros.Where(r => r.RecursoHumano != null).Select(r => r.RecursoHumanoId).ToArray();
            int?[] rmIds = registros.Where(r => r.RecursoMaterial != null).Select(r => r.RecursoMaterialId).ToArray();

            var AtividadesList = new List<RFG_Atividade>();

            foreach(var rm0 in registros.Where(r => r.RecursoMaterial != null && r.Atividade != null).GroupBy(p => p.Atividade.Valor)) {
                decimal? custo = 0;
                foreach(var rm1 in rm0) {
                    custo += rm1.ValorUnitario * rm1.QtdItens;
                }
                string _resAtividade = null;
                switch(rm0.First().Atividade.Valor) {
                    case "HH":
                        _resAtividade = projeto.Atividades.ResDedicacaoHorario;
                        break;
                    case "EC":
                        _resAtividade = projeto.Atividades.ResParticipacaoMembros;
                        break;
                    case "FG":
                        _resAtividade = projeto.Atividades.ResDesenvFerramenta;
                        break;
                    case "PP":
                        _resAtividade = projeto.Atividades.ResProspTecnologica;
                        break;
                    case "RP":
                        _resAtividade = projeto.Atividades.ResDivulgacaoResultados;
                        break;
                    case "AP":
                        _resAtividade = projeto.Atividades.ResParticipacaoTecnicos;
                        break;
                    case "BA":
                        _resAtividade = projeto.Atividades.ResBuscaAnterioridade;
                        break;
                    case "CA":
                        _resAtividade = projeto.Atividades.ResContratacaoAuditoria;
                        break;
                    case "AC":
                        _resAtividade = projeto.Atividades.ResApoioCitenel;
                        break;
                }
                AtividadesList.Add(new RFG_Atividade {
                    TipoAtividade = rm0.First().Atividade.Valor,
                    ResAtividade = _resAtividade,
                    CustoAtividade = custo.ToString()
                });
            }

            relatorio.PD_RelFinalBase = new PD_RelFinalBase {
                CodProjeto = projeto.Codigo,
                ArquivoPDF = projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAnual").FirstOrDefault().NomeArquivo,
                DataIniODS = projeto.DataInicio.ToString(),
                DataFimODS = (projeto.Etapas.LastOrDefault().DataFim == null) ? _etapaService.AddDataEtapas(projeto.Etapas).LastOrDefault().DataFim.ToString() : projeto.Etapas.LastOrDefault().DataFim.ToString(),
                Atividades = new RFG_Atividades { Atividade = AtividadesList }
            };

            // PD_EQUIPEEMP
            var PedEmpresaList = new List<PedEmpresa>();
            var EmpresasFinanciadoras = projeto.Empresas
                .Where(p => p.ClassificacaoValor == "Energia" || p.ClassificacaoValor == "Proponente")
                .ToList();
            foreach(Empresa empresa in EmpresasFinanciadoras) {
                var equipeList = new List<EquipeEmpresa>();

                var registrosRH = from r in registros
                                  where r.RecursoHumano != null && r.RecursoHumano.CPF != null && r.RecursoHumano.Empresa.Id == empresa.Id
                                  select r;

                foreach(var reg in registrosRH) {
                    var strMesHora = ObterMesReferencia(projeto, registros.Where(r => r.RecursoHumanoId == reg.RecursoHumanoId).ToList());
                    equipeList.Add(new EquipeEmpresa {
                        NomeMbEqEmp = reg.RecursoHumano.NomeCompleto,
                        CpfMbEqEmp = reg.RecursoHumano.CPF,
                        HhMbEqEmp = reg.RecursoHumano.ValorHora.ToString(),
                        MesMbEqEmp = strMesHora[0],
                        HoraMesMbEqEmp = strMesHora[1]
                    });
                }

                PedEmpresaList.Add(new PedEmpresa {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    TipoEmpresa = empresa.ClassificacaoValor,
                    Equipe = new Equipe {
                        EquipeEmpresa = equipeList
                    }
                });
            }
            relatorio.PD_Equipe = new PD_Equipe {
                Empresas = new PedEmpresas {
                    Empresa = PedEmpresaList
                }
            };
            // PD_ETAPAS
            var EtapasList = new List<PD_Etapa>();
            int ordem = 1;

            var meses = new List<DateTime>();

            projeto.Etapas.ForEach(e =>
                meses.AddRange(
                    e.EtapaMeses.Select(m => m.Mes)
                )
            );
            meses = meses.GroupBy(m => m).Select(m => m.First()).OrderBy(m => m).ToList();


            projeto.Etapas.Select(e => e.EtapaMeses.Select(m => m.Mes));

            foreach(Etapa etapa in projeto.Etapas.OrderBy(e => e.Id)) {

                var etapalist = etapa.EtapaMeses.Select(m => meses.IndexOf(m.Mes) + 1).OrderBy(i => i).ToList();

                var mesExecEtapa = String.Join(",", etapalist);

                EtapasList.Add(new PD_Etapa {
                    EtapaN = ordem.ToString().PadLeft(2, '0'),
                    Atividades = etapa.AtividadesRealizadas,
                    MesExecEtapa = mesExecEtapa
                });

                ordem++;
            }
            relatorio.PD_Etapas = new PD_Etapas {
                Etapa = EtapasList
            };
            // PD_RECURSO
            relatorio.PD_Recursos = new RFG_Recursos {
                RecursoEmpresa = new List<RFG_RecursoEmpresa>()
            };
            foreach(Empresa empresa in EmpresasFinanciadoras) {

                relatorio.PD_Recursos.RecursoEmpresa.Add(new RFG_RecursoEmpresa {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    CustoCatContabil = ObterCustosCat(registros.Where(r => r.EmpresaFinanciadoraId == empresa.Id && r.RecursoMaterial != null).ToList())
                });
            }


            // PD_RESULTADO
            var listIdCp = new List<IdCP>();
            foreach(ResultadoCapacitacao rCp in _context.ResultadosCapacitacao.Include("RecursoHumano").Include("Uploads").Where(r => r.ProjetoId == ProjetoId).ToList()) {
                listIdCp.Add(new IdCP {
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

            relatorio.PD_ResultadosCP = new PD_ResultadosCP {
                IdCP = listIdCp
            };

            var listIdPC = new List<IdPC>();
            foreach(ResultadoProducao rCT_PC in _context.ResultadosProducao.Include("Pais").Include("Uploads").Where(r => r.ProjetoId == ProjetoId).ToList()) {
                listIdPC.Add(new IdPC {
                    TipoPC = rCT_PC.TipoValor,
                    ConfPubPC = rCT_PC.Confirmacao.ToString(),
                    DataPC = rCT_PC.DataPublicacao.ToString(),
                    NomePC = rCT_PC.Nome,
                    LinkPC = rCT_PC.Url,
                    PaisPC = rCT_PC.Pais.Nome,
                    CidadePC = rCT_PC.Cidade,
                    TituloPC = rCT_PC.Titulo,
                    ArquivoPDF = rCT_PC.Uploads.First().NomeArquivo,
                });
            }
            relatorio.PD_ResultadosPC = new PD_ResultadosPC {
                IdPC = listIdPC
            };
            return relatorio;
        }
    }
}
