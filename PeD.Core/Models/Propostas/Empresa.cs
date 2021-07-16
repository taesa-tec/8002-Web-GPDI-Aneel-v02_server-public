using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    public enum Funcao
    {
        Executora,
        Cooperada,
    }

    [Table("PropostaEmpresas")]
    public class Empresa : PropostaNode
    {
        public bool Required { get; set; }
        public int? EmpresaRefId { get; set; }
        public Models.Empresa EmpresaRef { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
        public string Codigo { get; set; }

        public Funcao Funcao { get; set; }
        public string Nome => EmpresaRef?.Nome ?? RazaoSocial;
    }
}