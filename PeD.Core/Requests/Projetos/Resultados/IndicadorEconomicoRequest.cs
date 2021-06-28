using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Projetos;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class IndicadorEconomicoRequest : BaseEntity
    {
        public string Tipo { get; set; }
        [MaxLength(400)] public string Beneficio { get; set; }
        [MaxLength(10)] public string UnidadeBase { get; set; }
        public decimal ValorNumerico { get; set; }
        public decimal PorcentagemImpacto { get; set; }
        public decimal ValorBeneficio { get; set; }
    }
}