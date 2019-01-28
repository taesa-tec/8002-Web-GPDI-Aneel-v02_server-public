

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
        public TipoRegistro Tipo { get; set; }
        public StatusRegistro Status { get; set; }
        public int? RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public string Mes { get; set; }
        public string QtdHrs { get; set; }
        public int? EmpresaFinanciadoraId { get; set; }
        [ForeignKey("EmpresaFinanciadoraId")]
        public Empresa EmpresaFinanciadora { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        [Column(TypeName="date")]
        public DateTime DataDocumento { get; set; }
        public string AtividadeRealizada { get; set; }
        public ICollection<RegistroObs> ObsIternas { get; set; }
        public string NomeItem { get; set; }
        public int? RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        public int? EmpresaRecebedoraId { get; set; }
        [ForeignKey("EmpresaRecebedoraId")]
        public Empresa EmpresaRecebedora { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }
        public CategoriaContabil CategoriaContabil { get; set; }
        public bool EquiparLabExistente { get; set; }
        public bool EquiparLabNovo { get; set; }
        public bool ItemNacional { get; set; }
        public int QtdItens { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set;}
        public string FuncaoRecurso { get; set;}
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
        public int? RegistroFinanceiroId { get; set; }
        public DateTime DataCriacao { get; set; }
        public string UserId{ get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string Texto { get; set; }
    }
}