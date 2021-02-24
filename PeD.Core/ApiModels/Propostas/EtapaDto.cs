using System.Collections.Generic;

namespace PeD.Core.ApiModels.Propostas
{
    public class EtapaDto : PropostaNodeDto
    {
        public string DescricaoAtividades { get; set; }

        public int ProdutoId { get; set; }
        public string Produto { get; set; }

        public List<int> Meses { get; set; }
        public int Ordem { get; set; }
    }
}