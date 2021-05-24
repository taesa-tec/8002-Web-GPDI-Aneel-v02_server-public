using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    public enum CoExecutorFuncao
    {
        Executora,
        Proponente,
        Cooperada,
        Interveniente
    }

    [Table("ProjetoCoExecutores")]
    public class CoExecutor : ProjetoNode
    {
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }

        public CoExecutorFuncao Funcao { get; set; }
    }
}