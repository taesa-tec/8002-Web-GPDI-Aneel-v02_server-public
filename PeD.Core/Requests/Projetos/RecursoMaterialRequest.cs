using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos
{
    public class RecursoMaterialRequest : BaseEntity
    {
        public string Nome { get; set; }
        public int CategoriaContabilId { get; set; }
        public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set; }
    }
}