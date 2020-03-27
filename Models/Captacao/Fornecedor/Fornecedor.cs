using System.Collections.Generic;

namespace APIGestor.Models.Captacao.Fornecedor
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