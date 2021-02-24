using System.Collections.Generic;

namespace PeD.Core.Requests.Proposta
{
    public class EtapaRequest
    {
        public int Id { get; set; }
        public string DescricaoAtividades { get; set; }
        public int ProdutoId { get; set; }
        public List<int> Meses { get; set; }
    }
}