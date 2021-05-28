namespace PeD.Core.Models.Projetos
{
    public class RegistroFinanceiroRh : RegistroFinanceiro
    {
        public string AtividadeRealizada { get; set; }
        public int RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
    }
}