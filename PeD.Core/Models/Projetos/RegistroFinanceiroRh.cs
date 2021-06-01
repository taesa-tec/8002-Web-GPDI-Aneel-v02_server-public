using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    public class RegistroFinanceiroRh : RegistroFinanceiro
    {
        public string AtividadeRealizada { get; set; }
        public int RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal Horas { get; set; }
    }
}