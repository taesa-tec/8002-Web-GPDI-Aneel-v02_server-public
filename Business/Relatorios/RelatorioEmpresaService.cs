
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

    }
}