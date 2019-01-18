

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class RecursoMaterial
    {   
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        public string Nome { get; set;}
        public CategoriaContabil CategoriaContabil { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorUnitario { get; set; }
        public string Especificacao { get; set; }
    }
    public enum CategoriaContabil
    {   
        RH, ST, MC, MP, VD, OU
    }

    public class AlocacaoRm
    {
        [Key]
        public int Id { get; set; }
        public int? EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public int? ProjetoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }

        public int? EmpresaFinanciadoraId { get; set; }
        [ForeignKey("EmpresaFinanciadoraId")]
        public Empresa EmpresaFinanciadora { get; set; }
        public int? EmpresaRecebedoraId { get; set; }
        [ForeignKey("EmpresaRecebedoraId")]
        public Empresa EmpresaRecebedora { get; set; }
        public int Qtd { get; set; }
        public string Justificativa { get; set;}
    }
}