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
    public class XmlRelatorioFinalGestaoService : IXmlService<XmlRelatorioFinalGestao>
    {
        private GestorDbContext _context;
        private EtapaService _etapaService;
        public XmlRelatorioFinalGestaoService(GestorDbContext context, EtapaService etapaService)
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

            if (projeto == null)
            {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            else
            {
                if (projeto.RegistroFinanceiro.Where(r => r.StatusValor == "Aprovado").ToList().Count() <= 0)
                    resultado.Inconsistencias.Add("Não existem refps aprovados para o projeto.");
                if (projeto.RelatorioFinal.Uploads.Where(u => u.CategoriaValor == "RelatorioFinalAnual").FirstOrDefault() == null)
                    resultado.Inconsistencias.Add("Arquivo relatório final anual não localizado");
                if (projeto.ResultadosCapacitacao.Where(r => r.Uploads == null).ToList().Count() <= 0)
                    resultado.Inconsistencias.Add("Faltando Arquivo em resultados de capacitação");
                if (projeto.ResultadosProducao.Where(r => r.Uploads == null).ToList().Count() <= 0)
                    resultado.Inconsistencias.Add("Faltando Arquivo em resultados de produção");
            }
            return resultado;
        }
        public List<CustoCatContabil> ObterCustosCat(ICollection<AlocacaoRm> AlocacoesRm,  Empresa empresa, int?[] rmIds, List<RegistroFinanceiro> registros)
        {
            var CustoCatContabil = new List<CustoCatContabil>();
            foreach (var rm in AlocacoesRm
                        .Where(p => p.EmpresaRecebedora == empresa)
                        .Where(p => p.EmpresaFinanciadora == empresa)
                        .Where(p => rmIds.Contains(p.RecursoMaterial.Id))
                        .GroupBy(p => p.EmpresaRecebedora)
                        .ToList())
            {
                foreach (var rm0 in rm.GroupBy(p => p.RecursoMaterial.CategoriaContabilGestao.Valor))
                {
                    var itemDespesa = new List<ItemDespesa>();
                    foreach (var registro in registros
                            .Where(p => p.RecursoMaterial != null && p.RecursoMaterial.CategoriaContabilGestao.Valor == rm0.First().RecursoMaterial.CategoriaContabilGestao.Valor).ToList())
                    {
                        itemDespesa.Add(new ItemDespesa
                        {
                            NomeItem = registro.NomeItem,
                            JustificaItem = rm0.First().Justificativa,
                            QtdeItem = registro.QtdItens,
                            ValorIndItem = registro.ValorUnitario.ToString(),
                            TipoItem = registro.TipoValor
                        });
                    }
                    CustoCatContabil.Add(new CustoCatContabil
                    {
                        CategoriaContabil = rm0.First().RecursoMaterial.CategoriaContabilValor,
                        ItemDespesa = itemDespesa
                    });
                }
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
            return new List<string> { MesMb.TrimEnd(','), HoraMesMb.TrimEnd(',') };
        }
        public XmlRelatorioFinalGestao GerarXml(int ProjetoId, string Versao, string UserId)
        {
            XmlRelatorioFinalGestao relatorio = new XmlRelatorioFinalGestao();
            Projeto projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Include("Empresas.Estado")
                        .Include("Etapas")
                        .Include("Atividades")
                        .Include("AlocacoesRh.RecursoHumano")
                        .Include("AlocacoesRm.RecursoMaterial.CategoriaContabilGestao")
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

            foreach (var rm0 in registros.GroupBy(p => p.Atividade.Valor))
            {
                decimal? custo = 0;
                foreach (var rm1 in rm0)
                {
                    custo += rm1.ValorUnitario * rm1.QtdItens;
                }
                string _resAtividade = null;
                switch(rm0.First().Atividade.Valor){
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
                AtividadesList.Add(new RFG_Atividade
                {
                    TipoAtividade = rm0.First().Atividade.Valor,
                    ResAtividade = _resAtividade,
                    CustoAtividade = custo.ToString()
                });
            }
            relatorio.PD_RelFinalBase = new PD_RelFinalBase
            {
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
            relatorio.PD_Equipe = new PD_Equipe
            {
                Empresas = new PedEmpresas
                {
                    Empresa = PedEmpresaList
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
            relatorio.PD_Recursos = new RFG_Recursos
            {
                RecursoEmpresa = new List<RFG_RecursoEmpresa>()
            };
            foreach (Empresa empresa in EmpresasFinanciadoras)
            {

                relatorio.PD_Recursos.RecursoEmpresa.Add(new RFG_RecursoEmpresa
                {
                    CodEmpresa = empresa.CatalogEmpresa.Valor,
                    CustoCatContabil = ObterCustosCat(projeto.AlocacoesRm, empresa, rmIds, registros)
                });
            }


            // PD_RESULTADO
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

            relatorio.PD_ResultadosCP = new PD_ResultadosCP{
                IdCP = listIdCp
            };

            var listIdPC = new List<IdPC>();
            foreach (ResultadoProducao rCT_PC in _context.ResultadosProducao.Include("Pais").Include("Uploads").Where(r => r.ProjetoId == ProjetoId).ToList())
            {
                listIdPC.Add(new IdPC
                {
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
            relatorio.PD_ResultadosPC = new PD_ResultadosPC{
                IdPC = listIdPC
            };
            return relatorio;
        }
    }
}
