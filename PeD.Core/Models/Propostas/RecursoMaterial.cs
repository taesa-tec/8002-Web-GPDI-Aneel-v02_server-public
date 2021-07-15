using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaRecursosMateriais")]
    public class RecursoMaterial : PropostaNode
    {
        public string Nome { get; set; }
        public int CategoriaContabilId { get; set; }
        public Catalogos.CategoriaContabil CategoriaContabil { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set; }
    }

    [Table("PropostaRecursosMateriaisAlocacao")]
    public class AlocacaoRm : Propostas.Alocacao
    {
        public int RecursoId { get; set; }
        public RecursoMaterial Recurso { get; set; }
        public int? EmpresaRecebedoraId { get; set; }
        public Empresa EmpresaRecebedora { get; set; }

        [Column(TypeName = "decimal(18, 2)")] public decimal Quantidade { get; set; }

        public override decimal Valor => Quantidade * (Recurso?.ValorUnitario ?? 0);
    }
}