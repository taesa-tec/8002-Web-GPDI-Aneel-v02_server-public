using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class RecursoMaterialDto : BaseEntity
    {
        public string Nome { get; set; }
        public int CategoriaContabilId { get; set; }
        public string CategoriaContabil { get; set; }
        public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set; }
    }
}