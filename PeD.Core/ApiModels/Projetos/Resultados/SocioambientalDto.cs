namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class SocioambientalDto : ProjetoNodeDto
    {
        public enum TipoIndicador
        {
            ISA1,
            ISA2,
            ISA3,
            ISA4
        }

        public TipoIndicador Tipo { get; set; }
        public bool ResultadoPositivo { get; set; }
        public string DescricaoResultado { get; set; }
    }
}