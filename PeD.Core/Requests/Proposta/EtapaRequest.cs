using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class EtapaRequest : BaseEntity
    {
        public string DescricaoAtividades { get; set; }
        public int ProdutoId { get; set; }
        public List<int> Meses { get; set; }
    }
}