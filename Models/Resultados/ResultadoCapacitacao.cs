using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ResultadoCapacitacao
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get;set; }
        public int RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public TipoCapacitacao Tipo { get; set; }
        [NotMapped]
        public string TipoValor { 
            get => Enum.GetName(typeof(TipoCapacitacao),Tipo);
        }
        public bool? Conclusao { get; set; }
        public DateTime DataConclusao { get;set; }
        public string CnpjInstituicao { get;set; }
        public string AreaPesquisa { get; set; }
        public string TituloTrabalho { get;set; }
        public ICollection<Upload> Uploads { get; set; }
    }
    public enum TipoCapacitacao{
        PD=1,DO=2,ME=3,ES=4
    }
}