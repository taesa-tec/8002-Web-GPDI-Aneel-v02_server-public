using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace APIGestor.Models {

    #region Itens do Relatório
    public class RelatorioEmpresaItem {
        public string Desc { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        public decimal? Valor { get; set; }
    }

    public class OrcamentoEmpresaItem : RelatorioEmpresaItem {
        public int AlocacaoId { get; set; }
        public Etapa Etapa { get; set; }
        public AlocacaoRh AlocacaoRh { get; set; }
        public AlocacaoRm AlocacaoRm { get; set; }
    }

    public class ExtratoEmpresaItem : RelatorioEmpresaItem {
        public RegistroFinanceiro RegistroFinanceiro { get; set; }
    }

    #endregion

    #region Categorias
    public class RelatorioEmpresaCategorias<T> where T : RelatorioEmpresaItem {
        public string Desc { get; set; }
        public List<T> Items { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }

    }

    public class OrcamentoEmpresaCategorias : RelatorioEmpresaCategorias<OrcamentoEmpresaItem> {

    }

    public class ExtratoEmpresaCategorias : RelatorioEmpresaCategorias<ExtratoEmpresaItem> {

        public int? TotalAprovado { get; set; }
        public decimal? ValorAprovado { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal? Desvio {
            get {
                return this.TotalAprovado / Total;
            }
        }
    }
    #endregion

    #region Relatório por empresa
    public class RelatorioEmpresa<T> {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<T> Relatorios { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }

    }

    public class OrcamentoEmpresa : RelatorioEmpresa<OrcamentoEmpresaCategorias> {

    }

    public class ExtratoEmpresa : RelatorioEmpresa<ExtratoEmpresaCategorias> {

        public int TotalAprovado { get; set; }
        public decimal ValorAprovado { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal? Desvio {
            get {
                return this.ValorAprovado / this.Valor;
            }
        }
    }

    #endregion

    #region Relatórios empresas

    public class RelatorioEmpresas<T> {
        public List<T> Empresas { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }

    }
    public class OrcamentoEmpresas : RelatorioEmpresas<OrcamentoEmpresa> {

        protected List<AlocacaoRh> alocacaoRhs;
        protected List<AlocacaoRm> alocacaoRms;

        public OrcamentoEmpresas(List<AlocacaoRh> alocacaoRhs, List<AlocacaoRm> alocacaoRms) {
            this.alocacaoRhs = alocacaoRhs;
            this.alocacaoRms = alocacaoRms;
        }
    }
    public class ExtratoEmpresas : RelatorioEmpresas<ExtratoEmpresa> {

        protected List<RegistroFinanceiro> registroFinanceiros;
        public int TotalAprovado { get; set; }
        public decimal ValorAprovado { get; set; }
        public decimal? Desvio {
            get {
                return this.ValorAprovado / this.Valor;
            }
        }

        public ExtratoEmpresas(List<RegistroFinanceiro> registroFinanceiros) {
            this.registroFinanceiros = registroFinanceiros;
        }
    }
    #endregion

    public class RelatorioEmpresaCsv {
        public int Id { get; set; }
        public string Etapa { get; set; }
        public string NomeRecurso { get; set; }
        public string CPF { get; set; }
        public string FUNCAO { get; set; }
        public string TITULACAO { get; set; }
        public string CL { get; set; }
        public string CategoriaContabil { get; set; }
        public string EspecificacaoTecnica { get; set; }
        public string Justificativa { get; set; }
        public string EntidadePagadora { get; set; }
        public string CnpjEntidadePagadora { get; set; }
        public string EntidadeRecebedora { get; set; }
        public string CnpjEntidadeRecebedora { get; set; }
        public int? QtdHoras { get; set; }
        public decimal ValorHora { get; set; }
        public int Unidades { get; set; }
        public decimal ValorUnitario { get; set; }
        public string NomeItem { get; set; }
        public string MesReferencia { get; set; }
        public string TipoDocumento { get; set; }
        public string DataDocumento { get; set; }
        public string ArquivoComprovante { get; set; }
        public string AtividadeRealizada { get; set; }
        public string Beneficiado { get; set; }
        public string ObsInternas { get; set; }
        public string UsuarioAprovacao { get; set; }
        public string EquiparLabExistente { get; set; }
        public string EquiparLabNovo { get; set; }
        public string ItemNacional { get; set; }
        public string DataAprovacao { get; set; }
        public decimal? ValorTotal { get; set; }
    }

    public sealed class RelatorioEmpresaCsvMap : ClassMap<RelatorioEmpresaCsv> {
        public RelatorioEmpresaCsvMap(string type) {
            switch (type) {
                case "RelatorioEmpresa":
                    Map(m => m.Id);
                    Map(m => m.Etapa);
                    Map(m => m.NomeRecurso);
                    Map(m => m.CPF);
                    Map(m => m.FUNCAO);
                    Map(m => m.TITULACAO);
                    Map(m => m.CL);
                    Map(m => m.CategoriaContabil);
                    Map(m => m.EspecificacaoTecnica);
                    Map(m => m.Justificativa);
                    Map(m => m.EntidadePagadora);
                    Map(m => m.CnpjEntidadePagadora);
                    Map(m => m.EntidadeRecebedora);
                    Map(m => m.CnpjEntidadeRecebedora);
                    Map(m => m.QtdHoras);
                    Map(m => m.ValorHora);
                    Map(m => m.Unidades);
                    Map(m => m.ValorUnitario);
                    Map(m => m.ValorTotal);
                    break;
                case "RelatorioREFP":
                    Map(m => m.Id);
                    Map(m => m.Etapa);
                    Map(m => m.NomeRecurso);
                    Map(m => m.NomeItem);
                    Map(m => m.CPF);
                    Map(m => m.FUNCAO);
                    Map(m => m.TITULACAO);
                    Map(m => m.CL);
                    Map(m => m.CategoriaContabil);
                    Map(m => m.EspecificacaoTecnica);
                    Map(m => m.Justificativa);
                    Map(m => m.EntidadePagadora);
                    Map(m => m.CnpjEntidadePagadora);
                    Map(m => m.EntidadeRecebedora);
                    Map(m => m.CnpjEntidadeRecebedora);
                    Map(m => m.QtdHoras);
                    Map(m => m.ValorHora);
                    Map(m => m.Unidades);
                    Map(m => m.ValorUnitario);
                    Map(m => m.MesReferencia);
                    Map(m => m.TipoDocumento);
                    Map(m => m.DataDocumento);
                    Map(m => m.ArquivoComprovante);
                    Map(m => m.AtividadeRealizada);
                    Map(m => m.Beneficiado);
                    Map(m => m.ObsInternas);
                    Map(m => m.UsuarioAprovacao);
                    Map(m => m.EquiparLabExistente);
                    Map(m => m.EquiparLabNovo);
                    Map(m => m.ItemNacional);
                    Map(m => m.DataAprovacao);
                    Map(m => m.ValorTotal);
                    break;
            }
        }
    }
}