namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class SocioambientalDto : ProjetoNodeDto
    {
        public string Tipo { get; set; }
        public bool ResultadoPositivo { get; set; }
        public string DescricaoResultado { get; set; }
    }
}