using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APIGestor.Attributes;
using APIGestor.Models.Catalogs;

namespace APIGestor.Models.Projetos {
    public class RegistroFinanceiro {
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        [Logger("Tipo", "TipoValor")]
        public TipoRegistro? Tipo { get; set; }
        [NotMapped]
        public string TipoValor { get => Tipo == null ? null : Enum.GetName(typeof(TipoRegistro), Tipo); }
        [Logger("Status", "StatusValor")]
        public StatusRegistro? Status { get; set; }
        [NotMapped]
        public string StatusValor { get => Status == null ? null : Enum.GetName(typeof(StatusRegistro), Status); }
        [Logger("Recurso Humano", "RecursoHumano.NomeCompleto")]
        public int? RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        [Logger("Mês")]
        [Column(TypeName = "date")]
        public DateTime? Mes { get; set; }
        [Logger("Quantidade de horas")]
        public int? QtdHrs { get; set; }
        [Logger("Empresa Financiadora", "EmpresaFinanciadora.NomeEmpresa")]
        public int? EmpresaFinanciadoraId { get; set; }
        [ForeignKey("EmpresaFinanciadoraId")]
        public Empresa EmpresaFinanciadora { get; set; }
        [Logger("Tipo de documento", "TipoDocumentoValor")]
        public TipoDocumento? TipoDocumento { get; set; }
        [NotMapped]
        public string TipoDocumentoValor { get => TipoDocumento == null ? null : Enum.GetName(typeof(TipoDocumento), TipoDocumento); }
        [Logger("Número do documento")]
        public string NumeroDocumento { get; set; }
        [Logger("Data do documento")]
        [Column(TypeName = "date")]
        public DateTime? DataDocumento { get; set; }
        [Logger("Atividade Realizada")]
        public string AtividadeRealizada { get; set; }
        [Logger]
        public List<RegistroObs> ObsInternas { get; set; }
        [Logger("Nome do Item")]
        public string NomeItem { get; set; }
        [Logger("Recurso Material", "RecursoMaterial.Nome")]
        public int? RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        [Logger("Empresa Recebedora", "EmpresaRecebedora.NomeEmpresa")]
        public int? EmpresaRecebedoraId { get; set; }
        [ForeignKey("EmpresaRecebedoraId")]
        public Empresa EmpresaRecebedora { get; set; }
        [Logger("Benefíciado")]
        public string Beneficiado { get; set; }
        [Logger("Cnpj Benefíciado")]
        public string CnpjBeneficiado { get; set; }
        [Logger("Categoria Contábil", "CategoriaContabilValor")]
        public CategoriaContabil? CategoriaContabil { get; set; }
        [NotMapped]
        public string CategoriaContabilValor { get => CategoriaContabil == null ? null : Enum.GetName(typeof(CategoriaContabil), CategoriaContabil); }

        [Logger("Equipar Laboratório Existente")]
        public bool? EquiparLabExistente { get; set; }
        [Logger("Equipar Novo Laboratório")]
        public bool? EquiparLabNovo { get; set; }
        [Logger("Item Nacional")]
        public bool? ItemNacional { get; set; }
        [Logger("Quantidade de Itens")]
        public int? QtdItens { get; set; }

        [Logger("Valor Unitário")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ValorUnitario { get; set; }
        [Logger("Especificação Técnica")]
        public string EspecificacaoTecnica { get; set; }
        [Logger("Função de Recurso")]
        public string FuncaoRecurso { get; set; }
        public ICollection<Upload> Uploads { get; set; }
        [Logger("Categoria Contábil", "CategoriaContabilGestao.Nome")]
        public int? CatalogCategoriaContabilGestaoId { get; set; }
        public CatalogCategoriaContabilGestao CategoriaContabilGestao { get; set; }
        [Logger("Atividade", "Atividade.Nome")]
        public int? CatalogAtividadeId { get; set; }
        public CatalogAtividade Atividade { get; set; }

        [Logger("Valor Total")]
        [NotMapped]
        public decimal? ValorTotalRM {
            get {
                return this.QtdItens * (this.RecursoMaterial != null ? RecursoMaterial.ValorUnitario : 0);
            }
        }
        [Logger("Valor Total")]
        [NotMapped]
        public decimal? ValorTotalRH {
            get {
                return this.QtdHrs * (this.RecursoHumano != null ? RecursoHumano.ValorHora : 0);
            }
        }

    }
    public enum TipoDocumento {
        Cupom = 1,
        Declaracao = 2,
        Fatura = 3,
        Guia = 4,
        Nota = 5,
        Recibo = 6,
        Reserva = 7,
        ReciboSemCNPJ = 8
    }
    public enum StatusRegistro {
        Pendente = 1, Aprovado = 2, Reprovado = 3
    }
    public enum TipoRegistro {
        RH = 1, RM = 2
    }

    public class RegistroObs {
        [Key]
        public int Id { get; set; }
        public int RegistroFinanceiroId { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Logger("Observação interna")]
        public string Texto { get; set; }
    }
}