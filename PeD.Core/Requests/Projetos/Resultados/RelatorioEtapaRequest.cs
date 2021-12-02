using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class RelatorioEtapaRequest : BaseEntity
    {
        [MaxLength(300)] public string AtividadesRealizadas { get; set; }
    }
}