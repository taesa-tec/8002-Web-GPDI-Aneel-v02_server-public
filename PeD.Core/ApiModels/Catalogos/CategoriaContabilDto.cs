using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class CategoriaContabilDto : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<CategoriaContabilAtividadeDto> Atividades { get; set; }
    }
}