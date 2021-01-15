using TaesaCore.Models;

namespace PeD.Core.Models
{
    public class Contrato : BaseEntity
    {
        public string Titulo { get; set; }
        public string Header { get; set; }
        public string Conteudo { get; set; }
        public string Footer { get; set; }
    }
}