namespace PeD.Core.ApiModels.Projetos
{
    public class CoExecutorDto : ProjetoNodeDto
    {
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
        public string Funcao { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
    }
}