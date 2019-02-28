using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ResultadoInfra
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get;set; }
        public TipoInfra Tipo { get; set; }
        [NotMapped]
        public string TipoValor { 
            get => Enum.GetName(typeof(TipoInfra),Tipo);
        }
        public string CnpjReceptora { get;set; }
        public string NomeLaboratorio { get;set; }
        public string AreaPesquisa { get; set; }
        public string ListaMateriais { get;set; }
    }
    public enum TipoInfra
    {
        LNS=1,LES=2,LNP=3,LEP=4,LNE=5,LEE=6 
    }
}