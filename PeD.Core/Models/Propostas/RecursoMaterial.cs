using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaRecursosMateriais")]
    public class RecursoMaterial : PropostaNode
    {
        public string Nome { get; set; }
        public CategoriaContabil CategoriaContabil { get; set; }
        public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set; }

        public class Alocacao : Propostas.Alocacao
        {
            public int RecursoId { get; set; }
            public RecursoMaterial Recurso { get; set; }
            public int EmpresaRecebedoraId { get; set; }
            public Empresa EmpresaRecebedora { get; set; }
            public decimal Quantidade { get; set; }
        }
    }
}