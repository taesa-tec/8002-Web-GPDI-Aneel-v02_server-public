using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APIGestor.Attributes;

namespace APIGestor.Models.Projetos.Resultados {
    public class ResultadoInfra {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Tipo", "TipoValor")]
        public TipoInfra Tipo { get; set; }
        [NotMapped]
        public string TipoValor {
            get => Enum.GetName(typeof(TipoInfra), Tipo);
        }
        [Logger("Cnpj Receptora")]
        public string CnpjReceptora { get; set; }
        [Logger("Nome do laboratório")]
        public string NomeLaboratorio { get; set; }
        [Logger("Área de pesquisa")]
        public string AreaPesquisa { get; set; }
        [Logger("Lista de materiais")]
        public string ListaMateriais { get; set; }
    }
    public enum TipoInfra {
        LNS = 1, LES = 2, LNP = 3, LEP = 4, LNE = 5, LEE = 6
    }
}