using TaesaCore.Models;

namespace PeD.Core.ApiModels.Sistema
{
    public class ClausulaDto : BaseEntity
    {
        public int Ordem { get; set; }
        public string Conteudo { get; set; }
    }
}