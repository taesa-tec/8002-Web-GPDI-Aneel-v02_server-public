using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class Tema : BaseEntity
    {
        public int? ParentId { get; set; }
        public Tema Parent { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public ICollection<Tema> SubTemas { get; set; }
        public int Order { get; set; }
    }
}