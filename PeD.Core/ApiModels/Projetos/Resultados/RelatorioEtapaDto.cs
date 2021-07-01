using System;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class RelatorioEtapaDto : ProjetoNodeDto
    {
        public bool HasAtividadeCadastrada => !string.IsNullOrWhiteSpace(AtividadesRealizadas);

        public int EtapaId { get; set; }
        public EtapaDto Etapa { get; set; }
        public string AtividadesRealizadas { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}