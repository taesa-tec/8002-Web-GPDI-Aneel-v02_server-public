using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoEtapas")]
    public class Etapa : ProjetoNode
    {
        public string DescricaoAtividades { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public List<int> Meses { get; set; }
        public short Ordem { get; set; }
        
        public IEnumerable<Alocacao> Alocacoes { get; set; }
    }
}