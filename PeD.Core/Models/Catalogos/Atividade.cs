using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Catalogos
{
    public class Atividade
    {
        [Key] public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}