using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoRecursosMateriais")]
    public class RecursoMaterial : ProjetoNode
    {
        public string Nome { get; set; }
        public int CategoriaContabilId { get; set; }
        public Catalogos.CategoriaContabil CategoriaContabil { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set; }

        [Table("ProjetoRecursosMateriaisAlocacao")]
        public class AlocacaoRm : Alocacao
        {
            public int RecursoId { get; set; }
            public RecursoMaterial Recurso { get; set; }
            public int? EmpresaRecebedoraId { get; set; }
            public Empresa EmpresaRecebedora { get; set; }

            public int? CoExecutorRecebedorId { get; set; }
            public CoExecutor CoExecutorRecebedor { get; set; }
            [Column(TypeName = "decimal(18, 2)")] public decimal Quantidade { get; set; }

            public override decimal Valor => Quantidade * (Recurso?.ValorUnitario ?? 0);
        }
    }
}