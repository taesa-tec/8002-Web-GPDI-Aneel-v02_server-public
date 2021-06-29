namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class IndicadorEconomicoDto : ProjetoNodeDto
    {
        public string Tipo { get; set; }
        public string Beneficio { get; set; }
        public string UnidadeBase { get; set; }
        public decimal ValorNumerico { get; set; }
        public decimal PorcentagemImpacto { get; set; }
        public decimal ValorBeneficio { get; set; }
    }
}