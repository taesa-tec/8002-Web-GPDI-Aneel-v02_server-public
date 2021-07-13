using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    public enum CoExecutorFuncao
    {
        Executora,
        Proponente,
        Cooperada,
        Interveniente
    }

    [Table("PropostaCoExecutores")]
    public class CoExecutor : PropostaNode
    {
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
        public string Codigo { get; set; }

        public CoExecutorFuncao Funcao { get; set; }
    }
}