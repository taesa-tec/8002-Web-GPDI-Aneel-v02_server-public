namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class PropriedadeIntelectualDepositanteDto
    {
        public int PropriedadeId { get; set; }
        public int? EmpresaId { get; set; }
        public int? CoExecutorId { get; set; }
        public string Depositante { get; set; }
        public decimal Porcentagem { get; set; }
    }
}