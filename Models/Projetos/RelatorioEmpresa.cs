using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CsvHelper.Configuration;

namespace APIGestor.Models.Projetos
{
    #region Base Relatorio

    public class RelatorioEmpresas<T>
    {
        public List<T> Empresas { get; set; } = new List<T>();
        public int Total { get; set; }
        public decimal? Valor { get; set; }
    }

    public abstract class RelatorioEmpresa<T>
    {
        protected Dictionary<string, T> CategoriaRelatorios { get; set; } = new Dictionary<string, T>();

        public List<T> Relatorios
        {
            get
            {
                List<T> ts = new List<T>();
                foreach (var item in CategoriaRelatorios)
                {
                    ts.Add(item.Value);
                }

                return ts;
            }
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }

        public RelatorioEmpresa(Empresa empresa)
        {
            this.Id = empresa.Id;
            this.Nome = empresa.CatalogEmpresa != null ? empresa.CatalogEmpresa.Nome : empresa.RazaoSocial;
        }
    }

    public abstract class RelatoriosCategoria<T>
    {
        public string Desc { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public int Total
        {
            get { return Items.Count; }
        }

        public decimal Valor { get; set; }

        public void addItem(T item)
        {
            this.Items.Add(item);
        }

        protected RelatoriosCategoria(string desc)
        {
            Desc = desc;
        }
    }

    public abstract class RelatorioEmpresaItem
    {
        public string Desc { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        public decimal? Valor { get; set; }
    }

    #endregion

    #region Orçamentos - Proposta

    public class OrcamentoEmpresas : RelatorioEmpresas<OrcamentoEmpresa>
    {
        protected List<AlocacaoRh> alocacaoRhs;
        protected List<AlocacaoRm> alocacaoRms;

        public new int Total
        {
            get
            {
                int t = 0;
                foreach (var empresa in Empresas)
                {
                    t += empresa.Total;
                }

                return t;
            }
        }

        public decimal ValorOrcamento
        {
            get
            {
                decimal t = 0;
                foreach (var empresa in Empresas)
                {
                    t += empresa.ValorOrcamento;
                }

                return t;
            }
        }

        public List<OrcamentoEmpresaItem> OrcamentoEmpresaItens
        {
            get
            {
                var itens = new List<OrcamentoEmpresaItem>();
                foreach (var empresa in Empresas)
                {
                    foreach (var categoria in empresa.Relatorios)
                    {
                        itens.AddRange(categoria.Items);
                    }
                }

                return itens;
            }
        }

        public OrcamentoEmpresas(List<AlocacaoRh> alocacaoRhs, List<AlocacaoRm> alocacaoRms)
        {
            this.alocacaoRhs = alocacaoRhs;
            this.alocacaoRms = alocacaoRms;

            Dictionary<int, OrcamentoEmpresa> empresas = new Dictionary<int, OrcamentoEmpresa>();

            alocacaoRhs.ForEach(alocacao =>
            {
                if (!empresas.ContainsKey(alocacao.Empresa.Id))
                    empresas.Add(alocacao.Empresa.Id, new OrcamentoEmpresa(alocacao.Empresa));
                empresas[alocacao.Empresa.Id].addItem(alocacao);
            });

            alocacaoRms.ForEach(alocacao =>
            {
                if (!empresas.ContainsKey(alocacao.EmpresaFinanciadora.Id))
                    empresas.Add(alocacao.EmpresaFinanciadora.Id, new OrcamentoEmpresa(alocacao.EmpresaFinanciadora));
                empresas[alocacao.EmpresaFinanciadora.Id].addItem(alocacao);
            });

            foreach (var empresa in empresas)
            {
                Empresas.Add(empresa.Value);
            }
        }
    }

    public class OrcamentoEmpresa : RelatorioEmpresa<OrcamentoCategoria>
    {
        public void addItem(AlocacaoRh alocacao)
        {
            if (!this.CategoriaRelatorios.ContainsKey("RH"))
            {
                this.CategoriaRelatorios.Add("RH", new OrcamentoCategoria("Recursos Humanos"));
            }

            this.CategoriaRelatorios["RH"].addItem(new OrcamentoEmpresaItem(alocacao));
        }

        public new int Total
        {
            get
            {
                int total = 0;
                foreach (var item in Relatorios)
                {
                    total += item.Total;
                }

                return total;
            }
        }

        public decimal ValorOrcamento
        {
            get
            {
                decimal valor = 0;
                foreach (var item in Relatorios)
                {
                    valor += (decimal) item.ValorOrcamento;
                }

                return valor;
            }
        }

        public void addItem(AlocacaoRm alocacao)
        {
            if (!this.CategoriaRelatorios.ContainsKey(alocacao.RecursoMaterial.categoria))
            {
                this.CategoriaRelatorios.Add(alocacao.RecursoMaterial.categoria,
                    new OrcamentoCategoria(alocacao.RecursoMaterial.categoria));
            }

            this.CategoriaRelatorios[alocacao.RecursoMaterial.categoria].addItem(new OrcamentoEmpresaItem(alocacao));
        }

        public OrcamentoEmpresa(Empresa e) : base(e)
        {
        }
    }

    public class OrcamentoCategoria : RelatoriosCategoria<OrcamentoEmpresaItem>
    {
        public OrcamentoCategoria(string desc) : base(desc)
        {
        }

        public decimal ValorOrcamento
        {
            get
            {
                decimal t = 0;
                foreach (OrcamentoEmpresaItem item in Items)
                {
                    t += (decimal) item.Valor;
                }

                return t;
            }
        }
    }

    public class OrcamentoEmpresaItem : RelatorioEmpresaItem
    {
        public int AlocacaoId { get; set; }
        public Etapa Etapa { get; set; }
        public AlocacaoRh AlocacaoRh { get; set; }
        public AlocacaoRm AlocacaoRm { get; set; }

        public OrcamentoEmpresaItem(AlocacaoRh alocacaoRh)
        {
            Desc = alocacaoRh.RecursoHumano.NomeCompleto;
            AlocacaoRh = alocacaoRh;
            AlocacaoId = alocacaoRh.Id;
            Etapa = alocacaoRh.Etapa;
            Valor = alocacaoRh.HrsTotais * alocacaoRh.RecursoHumano.ValorHora;
            RecursoHumano = alocacaoRh.RecursoHumano;
        }

        public OrcamentoEmpresaItem(AlocacaoRm alocacaoRm)
        {
            Desc = alocacaoRm.RecursoMaterial.Nome;
            AlocacaoRm = alocacaoRm;
            AlocacaoId = alocacaoRm.Id;
            Etapa = alocacaoRm.Etapa;
            Valor = alocacaoRm.Qtd * alocacaoRm.RecursoMaterial.ValorUnitario;
            RecursoMaterial = alocacaoRm.RecursoMaterial;
        }
    }

    #endregion

    #region Extratos - Iniciado

    public class ExtratoEmpresas : RelatorioEmpresas<ExtratoEmpresa>
    {
        protected OrcamentoEmpresas orcamentoEmpresas;
        protected IEnumerable<RegistroFinanceiro> registroFinanceiros;

        public new int Total
        {
            get { return this.orcamentoEmpresas.Total; }
        }

        public decimal ValorOrcamento
        {
            get { return this.orcamentoEmpresas.ValorOrcamento; }
        }

        public int TotalAprovado
        {
            get
            {
                int t = 0;
                this.Empresas.ForEach(e => t += e.TotalAprovado);
                return t;
            }
        }

        public decimal ValorAprovado
        {
            get
            {
                decimal t = 0;
                this.Empresas.ForEach(e => t += e.ValorAprovado);
                return t;
            }
        }


        public decimal Desvio
        {
            get { return 100m * (this.ValorOrcamento > 0 ? this.ValorAprovado / this.ValorOrcamento : 0); }
        }

        public List<ExtratoEmpresaItem> ExtratoEmpresaItens
        {
            get
            {
                var itens = new List<ExtratoEmpresaItem>();
                foreach (var empresa in Empresas)
                {
                    foreach (var categoria in empresa.Relatorios)
                    {
                        itens.AddRange(categoria.Items);
                    }
                }

                return itens;
            }
        }

        public ExtratoEmpresas(List<RegistroFinanceiro> registroFinanceiros, OrcamentoEmpresas orcamentoEmpresas)
        {
            this.orcamentoEmpresas = orcamentoEmpresas;

            this.registroFinanceiros = from r in registroFinanceiros
                where r.EmpresaFinanciadoraId != null
                select r;

            Dictionary<int, ExtratoEmpresa> empresas = new Dictionary<int, ExtratoEmpresa>();

            foreach (var registro in this.registroFinanceiros)
            {
                if (registro.EmpresaFinanciadoraId != null &&
                    !empresas.ContainsKey((int) registro.EmpresaFinanciadoraId))
                {
                    var empresaOrcamento =
                        this.orcamentoEmpresas.Empresas.Find(e => e.Id == registro.EmpresaFinanciadora.Id);

                    empresas.Add((int) registro.EmpresaFinanciadoraId,
                        new ExtratoEmpresa(registro.EmpresaFinanciadora, empresaOrcamento)
                    );
                }

                empresas[(int) registro.EmpresaFinanciadoraId].addItem(registro);
            }

            foreach (var empresa in empresas)
            {
                Empresas.Add(empresa.Value);
            }
        }
    }

    public class ExtratoEmpresa : RelatorioEmpresa<ExtratoEmpresaCategorias>
    {
        OrcamentoEmpresa orcamento;

        public int TotalAprovado
        {
            get
            {
                int t = 0;
                this.Relatorios.ForEach(r => t += r.TotalAprovado);
                return t;
            }
        }

        public decimal ValorAprovado
        {
            get
            {
                decimal t = 0;
                this.Relatorios.ForEach(r => t += r.ValorAprovado);
                return t;
            }
        }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal Desvio
        {
            get { return 100m * (this.Valor > 0 ? this.ValorAprovado / this.Valor : 0); }
        }

        public ExtratoEmpresa(Empresa e, OrcamentoEmpresa orcamento) : base(e)
        {
            this.orcamento = orcamento != null ? orcamento : new OrcamentoEmpresa(e);
            this.Valor = this.orcamento.ValorOrcamento;
            this.Total = this.orcamento.Total;
        }

        public void addItem(RegistroFinanceiro registro)
        {
            if (registro.RecursoHumano != null)
            {
                if (!this.CategoriaRelatorios.ContainsKey("RH"))
                {
                    this.CategoriaRelatorios.Add("RH",
                        new ExtratoEmpresaCategorias("Recursos Humanos",
                            orcamento.Relatorios.Find(o => o.Desc == "Recursos Humanos")));
                }

                this.CategoriaRelatorios["RH"].addItem(new ExtratoEmpresaItem(registro));
            }
            else if (registro.RecursoMaterial != null)
            {
                if (!this.CategoriaRelatorios.ContainsKey(registro.RecursoMaterial.categoria))
                {
                    this.CategoriaRelatorios.Add(registro.RecursoMaterial.categoria, new ExtratoEmpresaCategorias(
                        registro.RecursoMaterial.categoria,
                        orcamento.Relatorios.Find(o => o.Desc == registro.RecursoMaterial.categoria)));
                }

                this.CategoriaRelatorios[registro.RecursoMaterial.categoria].addItem(new ExtratoEmpresaItem(registro));
            }
        }
    }

    public class ExtratoEmpresaCategorias : RelatoriosCategoria<ExtratoEmpresaItem>
    {
        public int TotalAprovado
        {
            get { return Items.Count; }
        }

        public decimal ValorAprovado
        {
            get
            {
                decimal t = 0;
                Items.ForEach(i => t += (decimal) i.Valor);
                return t;
            }
        }

        public new int Total = 0;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal Desvio
        {
            get { return 100m * (Valor > 0 ? this.ValorAprovado / Valor : 0); }
        }

        public ExtratoEmpresaCategorias(string desc, OrcamentoCategoria orcamento) : base(desc)
        {
            if (orcamento != null)
            {
                this.Valor = orcamento.ValorOrcamento;
                this.Total = orcamento.Total;
            }
            else
            {
                this.Valor = 0;
                this.Total = 0;
            }
        }
    }

    public class ExtratoEmpresaItem : RelatorioEmpresaItem
    {
        public RegistroFinanceiro RegistroFinanceiro { get; set; }

        public ExtratoEmpresaItem(RegistroFinanceiro registroFinanceiro)
        {
            RegistroFinanceiro = registroFinanceiro;
            if (registroFinanceiro.RecursoHumano != null)
            {
                RecursoHumano = registroFinanceiro.RecursoHumano;
                Desc = RecursoHumano.NomeCompleto;
                Valor = registroFinanceiro.QtdHrs * RecursoHumano.ValorHora;
            }
            else if (registroFinanceiro.RecursoMaterial != null)
            {
                RecursoMaterial = registroFinanceiro.RecursoMaterial;
                Desc = registroFinanceiro.RecursoMaterial.Nome;
                Valor = registroFinanceiro.QtdItens * registroFinanceiro.ValorUnitario; //RecursoMaterial.ValorUnitario;
            }
        }
    }

    #endregion


    public class RelatorioEmpresaCsv
    {
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

    public sealed class RelatorioEmpresaCsvMap : ClassMap<RelatorioEmpresaCsv>
    {
        public RelatorioEmpresaCsvMap(string type)
        {
            switch (type)
            {
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