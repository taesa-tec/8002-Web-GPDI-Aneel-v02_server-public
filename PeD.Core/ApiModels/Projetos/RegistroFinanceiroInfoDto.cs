namespace PeD.Core.ApiModels.Projetos
{
    public class RegistroFinanceiroInfoDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int ProjetoId { get; set; }
        public string Status { get; set; }
        public string TipoDocumento { get; set; }
        public string Recurso { get; set; }
        public int? RecursoHumanoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public int? FinanciadoraId { get; set; }
        public int? CoExecutorFinanciadorId { get; set; }
        public int? RecebedoraId { get; set; }
        public int? CoExecutorRecebedorId { get; set; }
        public int? CategoriaContabilId { get; set; }
        public string CategoriaContabil { get; set; }
        public string Financiador { get; set; }
        public string Recebedor { get; set; }
        public decimal Valor { get; set; }
        public decimal Custo { get; set; }
        public decimal QuantidadeHoras { get; set; }
    }
}