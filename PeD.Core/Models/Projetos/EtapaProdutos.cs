using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoEtapasProdutos")]
    public class EtapaProdutos : ProjetoNode
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }
    }
}