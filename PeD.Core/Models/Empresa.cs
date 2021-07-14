using TaesaCore.Models;

namespace PeD.Core.Models
{
    public class Empresa : BaseEntity
    {
        public enum CategoriaEmpresa
        {
            Taesa = 1,
            Fornecedor,
            CoExecutor
        }

        public string UF { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public string Cnpj { get; set; }
        public bool Ativo { get; set; }
        public CategoriaEmpresa Categoria { get; set; }
        public string Codigo => Valor; // Alias
    }
}