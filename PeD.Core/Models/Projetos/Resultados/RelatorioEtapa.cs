using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class RelatorioEtapa : ProjetoNode
    {
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        [MaxLength(300)]
        public string AtividadesRealizadas { get; set; }
    }
}