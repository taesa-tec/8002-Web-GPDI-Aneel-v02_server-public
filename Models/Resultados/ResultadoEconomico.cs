using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ResultadoEconomico
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get;set; }
        public TipoEconomico Tipo { get; set; }
        [NotMapped]
        public string TipoValor { 
            get => Enum.GetName(typeof(TipoEconomico),Tipo);
        }
        public string Desc { get; set; }
        public string UnidadeBase { get;set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ValorIndicador { get;set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Percentagem { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ValorBeneficio { get;set; }
    }
    public enum TipoEconomico
    {
        PR1=1,PR2=2,PR3=3,PRX=4,QF1=5,QF2=6,QF3=7,QFX=8,GA1=9,GA2=10,GA3=11,GAX=12,NT1=13,NT2=14,NT3=15,NT4=16,NTX=17,ME1=18,ME2=19,MEX=20,EE1=21,EE2=23,EE3=24,EE4=25,EEX=26,OU=27
    }
}