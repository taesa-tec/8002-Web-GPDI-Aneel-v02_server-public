using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Catalogos
{
    public class SubTema
    {
        [Key] public int SubTemaId { get; set; }
        public int TemaId { get; set; }
        public Tema Tema { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public int Order { get; set; }
    }
}