namespace PeD.Projetos.Models.Projetos.Xmls
{
    public class ProrrogacaoProjeto
    {
        public PD_PrazoExecProjeto PD_PrazoExecProjeto { get; set; }
    }
    public class PD_PrazoExecProjeto
    {
        public ProProjeto Projeto { get; set; }
    }
    public class ProProjeto
    {
        public string CodProjeto { get; set; }
        public int Duracao { get; set; }
    }
}