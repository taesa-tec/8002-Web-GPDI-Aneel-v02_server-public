using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ResultadoSocioAmbiental
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        public TipoIndicador Tipo { get;set; }
        [NotMapped]
        public string TipoValor { 
            get => Enum.GetName(typeof(TipoIndicador),Tipo);
        }
        public bool? Positivo { get;set; }
        public bool? TecnicaPrevista { get; set; }
        public string Desc { get;set; }
    }
    public enum TipoIndicador
    {
        ISA1=1,ISA2=2,ISA3=3,ISA4=4
    }
}