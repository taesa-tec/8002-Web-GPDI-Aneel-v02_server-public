using PeD.Core.Models.Projetos;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class RelatorioEtapaDto : ProjetoNodeDto
    {
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public string AtividadesRealizadas { get; set; }
    }
}