using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    public enum Funcao
    {
        Proponente,
        Executora, // Empresas cadastradas pelo fornecedor
        Cooperada, // Empresa Taesa
    }

    [Table("ProjetoEmpresas")]
    public class Empresa : ProjetoNode
    {
        public int? EmpresaRefId { get; set; }
        public Models.Empresa EmpresaRef { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }

        protected string _codigo;

        public string Codigo
        {
            get => EmpresaRefId != null ? EmpresaRef?.Codigo : _codigo;
            set => _codigo = value;
        }

        public string RazaoSocial { get; set; }
        public string Nome => RazaoSocial;

        public Funcao Funcao { get; set; }
    }
}