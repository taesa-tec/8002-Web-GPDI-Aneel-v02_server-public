using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class IndicadorEconomico : ProjetoNode
    {
        public enum TipoIndicador
        {
            PR1,
            PR2,
            PR3,
            PRX,
            QF1,
            QF2,
            QF3,
            QFX,
            GA1,
            GA2,
            GA3,
            GAX,
            NT1,
            NT2,
            NT3,
            NT4,
            NTX,
            ME1,
            ME2,
            MEX,
            EE1,
            EE2,
            EE3,
            EE4,
            EEX,
            OU
        }

        public TipoIndicador Tipo { get; set; }
        [MaxLength(400)]
        public string Beneficio { get; set; }
        [MaxLength(10)]
        public string UnidadeBase { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal ValorNumerico { get; set; }
        [Column(TypeName = "decimal(5,2)")]public decimal PorcentagemImpacto { get; set; }
        [Column(TypeName = "decimal(18,2)")]public decimal ValorBeneficio { get; set; }
        
        
    }
}