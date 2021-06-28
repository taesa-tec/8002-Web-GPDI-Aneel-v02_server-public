using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class SocioambientalRequest : BaseEntity
    {
        public string Tipo { get; set; }
        public bool ResultadoPositivo { get; set; }
        [MaxLength(500)] public string DescricaoResultado { get; set; }
    }
}