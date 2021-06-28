namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class IndicadorEconomicoDto : ProjetoNodeDto
    {
        public enum TipoIndicador
        {
            PR1,
            PR2,
            PR3,
            PRX,
            QF1,
            QF2,
            QF3,
            QFX,
            GA1,
            GA2,
            GA3,
            GAX,
            NT1,
            NT2,
            NT3,
            NT4,
            NTX,
            ME1,
            ME2,
            MEX,
            EE1,
            EE2,
            EE3,
            EE4,
            EEX,
            OU
        }

        public TipoIndicador Tipo { get; set; }
        public string Beneficio { get; set; }
        public string UnidadeBase { get; set; }
         public decimal ValorNumerico { get; set; }
        public decimal PorcentagemImpacto { get; set; }
        public decimal ValorBeneficio { get; set; }
        
        
    }
}