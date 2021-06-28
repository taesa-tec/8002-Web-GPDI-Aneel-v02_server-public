using System.ComponentModel.DataAnnotations;
using PeD.Core.Models.Projetos;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class RelatorioEtapaRequest  : BaseEntity
    {
        public int EtapaId { get; set; }
        [MaxLength(300)] public string AtividadesRealizadas { get; set; }
    }
}