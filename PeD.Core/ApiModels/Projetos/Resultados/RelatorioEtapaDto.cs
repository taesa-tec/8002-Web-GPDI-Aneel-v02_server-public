namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class RelatorioEtapaDto : ProjetoNodeDto
    {
        public int EtapaId { get; set; }
        public EtapaDto Etapa { get; set; }
        public string AtividadesRealizadas { get; set; }
    }
}