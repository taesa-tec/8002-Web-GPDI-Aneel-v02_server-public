using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class TemaDto : BaseEntity
    {
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<TemaDto> SubTemas { get; set; }
        public int Order { get; set; }
    }
}