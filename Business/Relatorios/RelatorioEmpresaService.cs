
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Text;
using System.Reflection;
using System.IO;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using ClosedXML.Excel;


namespace APIGestor.Business {
    public class RelatorioEmpresaService {
        private GestorDbContext context;
        protected AlocacaoRhService AlocacaoRhService;
        protected AlocacaoRmService AlocacaoRmService;
        protected RegistroFinanceiroService RegistroFinanceiroService;

        public RelatorioEmpresaService(GestorDbContext context, AlocacaoRhService alocacaoRhService, AlocacaoRmService alocacaoRmService, RegistroFinanceiroService registroFinanceiroService) {
            this.context = context;
            AlocacaoRhService = alocacaoRhService;
            AlocacaoRmService = alocacaoRmService;
            RegistroFinanceiroService = registroFinanceiroService;
        }

        public OrcamentoEmpresas orcamentoEmpresas(int projetoId) {

            List<AlocacaoRh> alocacaoRhs = AlocacaoRhService.ListarTodos(projetoId).ToList();
            List<AlocacaoRm> alocacaoRms = AlocacaoRmService.ListarTodos(projetoId).ToList();

            OrcamentoEmpresas orcamentos = new OrcamentoEmpresas(alocacaoRhs, alocacaoRms);

            return orcamentos;
        }

        public ExtratoEmpresas extratoEmpresas(int projetoId) {

            List<RegistroFinanceiro> registroFinanceiros = RegistroFinanceiroService.ListarTodos(projetoId, StatusRegistro.Aprovado).ToList();

            ExtratoEmpresas extratos = new ExtratoEmpresas(registroFinanceiros, orcamentoEmpresas(projetoId));

            return extratos;
        }


        #region Gerar CSV
        public List<RelatorioEmpresaCsv> FormatRelatorioCsv(OrcamentoEmpresas Relatorios) {
            var records = new List<RelatorioEmpresaCsv>();
            foreach (var empresa in Relatorios.Empresas) {

                foreach (var relatorio in empresa.Relatorios) {

                    foreach (var item in relatorio.Items) {

                        var newItem = new RelatorioEmpresaCsv();
                        newItem.NomeRecurso = item.Desc;
                        if (item.RecursoHumano != null) {
                            newItem.CPF = item.RecursoHumano.CPF;
                            newItem.FUNCAO = item.RecursoHumano.FuncaoValor;
                            newItem.TITULACAO = item.RecursoHumano.TitulacaoValor;
                            newItem.CL = item.RecursoHumano.UrlCurriculo;
                            newItem.ValorHora = item.RecursoHumano.ValorHora;
                            newItem.EntidadeRecebedora = (item.RecursoHumano.Empresa.Cnpj == null) ? item.RecursoHumano.Empresa.CatalogEmpresa.Nome : item.RecursoHumano.Empresa.RazaoSocial;
                            newItem.CnpjEntidadeRecebedora = (item.RecursoHumano.Empresa.Cnpj == null) ? null : item.RecursoHumano.Empresa.Cnpj;
                            newItem.CategoriaContabil = CategoriaContabil.RH.ToString();
                        }
                        if (item.AlocacaoRh != null) {
                            newItem.QtdHoras = item.AlocacaoRh.HrsTotais;
                            newItem.ValorTotal = newItem.ValorHora * newItem.QtdHoras;
                            newItem.Justificativa = item.AlocacaoRh.Justificativa;
                            newItem.EntidadePagadora = (item.AlocacaoRh.Empresa.Cnpj == null) ? item.AlocacaoRh.Empresa.CatalogEmpresa.Nome : item.AlocacaoRh.Empresa.RazaoSocial;
                            newItem.CnpjEntidadePagadora = item.AlocacaoRh.Empresa.Cnpj;
                        }
                        if (item.RecursoMaterial != null) {
                            newItem.CategoriaContabil = item.RecursoMaterial.CategoriaContabilValor;
                            newItem.ValorUnitario = item.RecursoMaterial.ValorUnitario;
                            newItem.EspecificacaoTecnica = item.RecursoMaterial.Especificacao;
                        }
                        if (item.AlocacaoRm != null) {
                            newItem.Unidades = item.AlocacaoRm.Qtd;
                            newItem.ValorTotal = newItem.ValorUnitario * newItem.Unidades;
                            newItem.Justificativa = item.AlocacaoRm.Justificativa;
                            newItem.EntidadePagadora = (item.AlocacaoRm.EmpresaFinanciadora.Cnpj == null) ? item.AlocacaoRm.EmpresaFinanciadora.CatalogEmpresa.Nome : item.AlocacaoRm.EmpresaFinanciadora.RazaoSocial;
                            newItem.CnpjEntidadePagadora = (item.AlocacaoRm.EmpresaFinanciadora.Cnpj == null) ? null : item.AlocacaoRm.EmpresaFinanciadora.Cnpj; ;
                            newItem.EntidadeRecebedora = (item.AlocacaoRm.EmpresaRecebedora == null) ? null : (item.AlocacaoRm.EmpresaRecebedora.Cnpj == null) ? item.AlocacaoRm.EmpresaRecebedora.CatalogEmpresa.Nome : item.AlocacaoRm.EmpresaRecebedora.RazaoSocial;
                            newItem.CnpjEntidadeRecebedora = (item.AlocacaoRm.EmpresaRecebedora == null) ? null : (item.AlocacaoRm.EmpresaRecebedora.Cnpj == null) ? null : item.AlocacaoRm.EmpresaRecebedora.Cnpj;
                        }
                        //if (item.RegistroFinanceiro != null) {
                        //    newItem.NomeItem = item.RegistroFinanceiro.NomeItem;
                        //    newItem.MesReferencia = item.RegistroFinanceiro.Mes.Value.Month.ToString() + "/" + item.RegistroFinanceiro.Mes.Value.Year.ToString();
                        //    newItem.TipoDocumento = item.RegistroFinanceiro.TipoDocumentoValor;
                        //    newItem.DataDocumento = item.RegistroFinanceiro.DataDocumento.ToString();
                        //    newItem.ArquivoComprovante = (item.RegistroFinanceiro.Uploads.FirstOrDefault() != null) ? item.RegistroFinanceiro.Uploads.FirstOrDefault().NomeArquivo : null;
                        //    newItem.AtividadeRealizada = item.RegistroFinanceiro.AtividadeRealizada;
                        //    newItem.Beneficiado = item.RegistroFinanceiro.Beneficiado;
                        //    newItem.ObsInternas = (item.RegistroFinanceiro.ObsInternas.LastOrDefault() != null) ? item.RegistroFinanceiro.ObsInternas.LastOrDefault().Texto : null;
                        //    newItem.UsuarioAprovacao = (item.RegistroFinanceiro.ObsInternas.LastOrDefault() != null) ? item.RegistroFinanceiro.ObsInternas.LastOrDefault().User.NomeCompleto : null;
                        //    newItem.EquiparLabExistente = (item.RegistroFinanceiro.EquiparLabExistente.HasValue) ? "Sim" : "Nao";
                        //    newItem.EquiparLabNovo = (item.RegistroFinanceiro.EquiparLabNovo.HasValue) ? "Sim" : "Nao";
                        //    newItem.ItemNacional = (item.RegistroFinanceiro.ItemNacional.HasValue) ? "Sim" : "Nao";
                        //    newItem.DataAprovacao = (item.RegistroFinanceiro.ObsInternas.LastOrDefault() != null) ? item.RegistroFinanceiro.ObsInternas.LastOrDefault().Created.ToString() : null;
                        //}
                        newItem.Id = item.AlocacaoId;
                        newItem.Etapa = (item.Etapa != null) ? item.Etapa.Nome : null;

                        records.Add(newItem);
                    }
                }
            }
            return records;
        }
        public MemoryStream ExportarRelatorio(List<RelatorioEmpresaCsv> data, string tipo) {
            var mr = new MemoryStream();
            var tw = new StreamWriter(mr, Encoding.UTF8);
            var csv = new CsvWriter(tw);
            csv.Configuration.Delimiter = ";";
            csv.Configuration.RegisterClassMap(new RelatorioEmpresaCsvMap(tipo));

            csv.WriteRecords(data);
            tw.Flush();
            tw.Close();
            return mr;
        }
        #endregion


        public XLWorkbook gerarXLSOrcamento(int projetoId) {


            XLWorkbook xls = new XLWorkbook();

            var workRH = xls.AddWorksheet("Recursos Humanos");
            workRH.ColumnWidth = 40;
            var workRM = xls.AddWorksheet("Recursos Materiais");
            workRM.ColumnWidth = 40;

            List<OrcamentoEmpresaItem> items = new List<OrcamentoEmpresaItem>();

            OrcamentoEmpresas orcamento = orcamentoEmpresas(projetoId);

            foreach (var empresa in orcamento.Empresas) {
                foreach (var categoria in empresa.Relatorios) {
                    foreach (var item in categoria.Items) {
                        items.Add(item);
                    }

                }

            }

            var table = new DataTable("Recursos Humanos");
            table.Columns.Add("Nome");
            table.Columns.Add("Titulo");
            table.Columns.Add("Cpf");
            table.Columns.Add("Empresa");
            table.Columns.Add("Custo Hora",typeof(decimal));
            table.Columns.Add("Horas Totais", typeof(int));
            table.Columns.Add("Custo", typeof(decimal));
            table.Columns.Add("Currículo Lattes");

            var rhItems = from i in items
                          where i.RecursoHumano != null
                          let custo = i.RecursoHumano.ValorHora * i.AlocacaoRh.HrsTotais
                          select new {
                              i.Desc,
                              i.RecursoHumano.TitulacaoValor,
                              i.RecursoHumano.CPF,
                              i.AlocacaoRh.Empresa.NomeEmpresa,
                              i.RecursoHumano.ValorHora,
                              i.AlocacaoRh.HrsTotais,
                              custo,
                              i.RecursoHumano.UrlCurriculo
                          };
            foreach (var item in rhItems) {
                table.Rows.Add(item.Desc, item.TitulacaoValor, item.CPF, item.NomeEmpresa, item.ValorHora, item.HrsTotais, item.custo, item.UrlCurriculo);
            }

            workRH.Cell(1, 1).InsertTable(table);

            table = new DataTable("Recursos Materiais");
            table.Columns.Add("Descrição");
            table.Columns.Add("Nome");
            table.Columns.Add("Categoria");
            table.Columns.Add("Especificação");
            table.Columns.Add("Atividade");
            table.Columns.Add("Quantidade", typeof(int));
            table.Columns.Add("Valor Unitário", typeof(decimal));
            table.Columns.Add("Custo", typeof(decimal));
            table.Columns.Add("Empresa Financiadora");

            var rmItems = from i in items
                          where i.RecursoMaterial != null
                          select i;



            foreach (var i in rmItems) {
                string atividade = i.RecursoMaterial.Atividade != null ? i.RecursoMaterial.Atividade.Nome : String.Empty;
                decimal custo = i.AlocacaoRm.Qtd * i.RecursoMaterial.ValorUnitario;
                table.Rows.Add(
                    i.Desc,
                    i.RecursoMaterial.Nome,
                    i.RecursoMaterial.categoria,
                    i.RecursoMaterial.Especificacao,
                    atividade,
                    i.AlocacaoRm.Qtd,
                    i.RecursoMaterial.ValorUnitario,
                    custo,
                    i.AlocacaoRm.EmpresaFinanciadora.NomeEmpresa
                    );
            }

            workRM.Cell(1, 1).InsertTable(table);

            return xls;
        }

        public XLWorkbook gerarXLSExtrato(int projetoId) {
            XLWorkbook xls = new XLWorkbook();

            var workRH = xls.AddWorksheet("Recursos Humanos");
            var workRM = xls.AddWorksheet("Recursos Materiais");
            var workEm = xls.AddWorksheet("Empresas");

            workEm.ColumnWidth = 20;
            workRH.ColumnWidth = 40;
            workRM.ColumnWidth = 40;

            var extrato = extratoEmpresas(projetoId);
            var itens = extrato.ExtratoEmpresaItens;

            #region Resumo dos dados

            var tableEmpresas = new DataTable("Empresas");
            //var tableGeral = new DataTable("Geral");

            //foreach (var col in new string[] { "Total", "Total Aprovado", "Valor", "Valor aprovado", "Desvio" }) {
            //    tableGeral.Columns.Add(col);
            //}
            //tableGeral.Rows.Add(extrato.Total, extrato.TotalAprovado, extrato.Valor, extrato.ValorAprovado, extrato.Desvio);

            tableEmpresas.Columns.Add(new DataColumn("Empresa", typeof(string)));
            foreach (var colname in new string[] { "Total", "Total Aprovado", "Valor", "Valor aprovado", "Desvio" }) {
                var coll = new DataColumn(colname);
                coll.DataType = typeof(decimal);
                tableEmpresas.Columns.Add(coll);
            }

            foreach (var empresa in extrato.Empresas) {
                tableEmpresas.Rows.Add(empresa.Nome, empresa.Total, empresa.TotalAprovado, empresa.Valor, empresa.ValorAprovado, empresa.Desvio / 100m);
            }

            //workEm.Cell(2, 5).Style.NumberFormat.Format = "0,00%";
            var coldesvio = workEm.Column("F");
            coldesvio.Style.NumberFormat.Format = "0,00%";
            //coldesvio.DataType = XLDataType.Number;

            //var table = workEm.Cell(1, 1).InsertTable(tableGeral, "Dados do Projeto");
            //table.Style.Fill.BackgroundColor = XLColor.BlueGray;

            workEm.Cell(1, 1).InsertTable(tableEmpresas, "Dados gerais por empresa");
            #endregion

            #region Registros RH
            var registrosRH = from i in itens
                              where i.RecursoHumano != null
                              select i;
            var tableRH = new DataTable("Recursos Humanos");
            foreach (var col in new string[] { "Nome completo", "CPF", "Função", "Empresa", "Empresa Financiadora", "Empresa Recebedora", "Qtd Hrs", "Custo Hora", "Custo Total", "Mês" }) {
                tableRH.Columns.Add(col);
            }

            foreach (var rrg in registrosRH) {
                var recurso = rrg.RecursoHumano;
                var registro = rrg.RegistroFinanceiro;
                var financiadora = registro.EmpresaFinanciadora;
                var recebedora = registro.EmpresaRecebedora;

                tableRH.Rows.Add(
                    recurso.NomeCompleto,
                    recurso.CPF,
                    recurso.FuncaoValor,
                    recurso.Empresa.NomeEmpresa,
                    financiadora.NomeEmpresa,
                    (recebedora != null ? recebedora.NomeEmpresa : financiadora.NomeEmpresa),
                    registro.QtdHrs,
                    recurso.ValorHora,
                    registro.ValorTotalRH,
                    registro.Mes

                );
            }
            workRH.Cell(1, 1).InsertTable(tableRH);
            #endregion

            #region Registro RM
            var tableRM = new DataTable("Recursos Materiais");
            var registrosRM = from i in itens
                              where i.RecursoMaterial != null
                              select i;

            foreach (var col in new string[] { "Nome Item", "Recurso", "Categoria", "Especificação", "Empresa Financiadora", "Empresa Recebedora", "Mês", "Qtd Itens", "Valor Unitário", "Valor Total" }) {
                tableRM.Columns.Add(col);
            }
            foreach (var rrg in registrosRM) {
                var recurso = rrg.RecursoMaterial;
                var registro = rrg.RegistroFinanceiro;
                var financiadora = registro.EmpresaFinanciadora;
                var recebedora = registro.EmpresaRecebedora;

                tableRM.Rows.Add(
                    registro.NomeItem,
                    recurso.Nome,
                    recurso.categoria,
                    recurso.Especificacao,
                    financiadora.NomeEmpresa,
                    (recebedora != null ? recebedora.NomeEmpresa : financiadora.NomeEmpresa),
                    registro.Mes,
                    registro.QtdItens,
                    registro.ValorUnitario,
                    registro.ValorTotalRM
                );
            }
            workRM.Cell(1, 1).InsertTable(tableRM);

            #endregion


            return xls;
        }
    }
}