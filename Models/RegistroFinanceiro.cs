

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class RegistroFinanceiro
    {   
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        public TipoRegistro? Tipo { get; set; }
        [NotMapped]
        public string TipoValor { get => Tipo == null ? null : Enum.GetName(typeof(TipoRegistro),Tipo); }
        public StatusRegistro? Status { get; set; }
        [NotMapped]
        public string StatusValor { get => Status == null ? null : Enum.GetName(typeof(StatusRegistro),Status); }
        public int? RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        [Column(TypeName="date")]
        public DateTime? Mes { get; set; }
        public int? QtdHrs { get; set; }
        public int? EmpresaFinanciadoraId { get; set; }
        [ForeignKey("EmpresaFinanciadoraId")]
        public Empresa EmpresaFinanciadora { get; set; }
        public TipoDocumento? TipoDocumento { get; set; }
        [NotMapped]
        public string TipoDocumentoValor { get => TipoDocumento == null ? null : Enum.GetName(typeof(TipoDocumento),TipoDocumento); }
        public string NumeroDocumento { get; set; }
        [Column(TypeName="date")]
        public DateTime? DataDocumento { get; set; }
        public string AtividadeRealizada { get; set; }
        public List<RegistroObs> ObsInternas { get; set; }
        public string NomeItem { get; set; }
        public int? RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        public int? EmpresaRecebedoraId { get; set; }
        [ForeignKey("EmpresaRecebedoraId")]
        public Empresa EmpresaRecebedora { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }
        public CategoriaContabil? CategoriaContabil { get; set; }
        [NotMapped]
        public string CategoriaContabilValor { get => CategoriaContabil == null ? null : Enum.GetName(typeof(CategoriaContabil),CategoriaContabil); }

        public bool? EquiparLabExistente { get; set; }
        public bool? EquiparLabNovo { get; set; }
        public bool? ItemNacional { get; set; }
        public int? QtdItens { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set;}
        public string FuncaoRecurso { get; set;}
        public ICollection<Upload> Uploads { get; set; }
        public int? CatalogCategoriaContabilGestaoId { get; set; }
        public CatalogCategoriaContabilGestao CategoriaContabilGestao { get; set; }
        public int? CatalogAtividadeId { get; set; }
        public CatalogAtividade Atividade { get; set; }

    }
    public enum TipoDocumento
    {
        Cupom = 1,
        Declaracao = 2,
        Fatura = 3,
        Guia = 4,
        Nota = 5,
        Recibo = 6,
        Reserva = 7
    }
    public enum StatusRegistro
    {   
        Pendente = 1, Aprovado = 2, Reprovado = 3
    }
    public enum TipoRegistro
    {   
        RH = 1, RM = 2
    }

    public class RegistroObs
    {
        [Key]
        public int Id { get; set; }
        public int RegistroFinanceiroId { get; set; }
        public DateTime Created { get; set; }
        public string UserId{ get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string Texto { get; set; }
    }
}