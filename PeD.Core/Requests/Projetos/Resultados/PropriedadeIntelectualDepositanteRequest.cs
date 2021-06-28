using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class PropriedadeIntelectualDepositanteRequest
    {
        public int? EmpresaId { get; set; }
        public int? CoExecutorId { get; set; }
        [Column(TypeName = "decimal(5,2)")] public decimal Porcentagem { get; set; }
    }
}