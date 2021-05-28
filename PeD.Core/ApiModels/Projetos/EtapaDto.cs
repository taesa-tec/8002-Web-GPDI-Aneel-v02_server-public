using System.Collections.Generic;

namespace PeD.Core.ApiModels.Projetos
{
    public class EtapaDto : ProjetoNodeDto
    {
        public string DescricaoAtividades { get; set; }

        public int ProdutoId { get; set; }
        public string Produto { get; set; }

        public List<int> Meses { get; set; }
        public short Ordem { get; set; }
    }
}