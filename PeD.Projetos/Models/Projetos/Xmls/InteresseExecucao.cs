namespace PeD.Projetos.Models.Projetos.Xmls
{
    public class InteresseExecucao
    {
        public PD_InteresseProjeto PD_InteresseProjeto { get; set; }
    }
    public class PD_InteresseProjeto
    {
        public InteresseProjeto Projeto{ get; set; }
    }
    public class InteresseProjeto
    {
        public string CodProjeto { get; set; }
        private string _interesse{ get; set; }
        public string Interesse{
            get => "S";
            set => _interesse = value;
        }
    }
}