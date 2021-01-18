namespace PeD.Core.Models.Propostas
{
    public class EtapaProdutos : PropostaNode
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }
    }
}