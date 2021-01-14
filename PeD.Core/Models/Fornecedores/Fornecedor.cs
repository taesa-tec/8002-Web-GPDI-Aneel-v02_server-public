using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Fornecedores
{
    public class Fornecedor : BaseEntity
    {
        public string Nome { get; set; }
        public string CNPJ { get; set; }

        public string ResponsavelId { get; set; }
        public ApplicationUser Responsavel { get; set; }
        public bool Ativo { get; set; }
        public List<CoExecutor> CoExecutores { get; set; }
    }
}