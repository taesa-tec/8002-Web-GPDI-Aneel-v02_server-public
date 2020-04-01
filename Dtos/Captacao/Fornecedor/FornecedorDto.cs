using System.Collections.Generic;
using APIGestor.Models.Captacao.Fornecedor;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao.Fornecedor
{
    public class FornecedorDto : BaseEntity
    {
        public string Nome { get; set; }
        public string CNPJ { get; set; }

        public string ResponsavelId { get; set; }
        public ApplicationUserDto Responsavel { get; set; }
        public bool Ativo { get; set; }
        public List<CoExecutorDto> CoExecutores { get; set; }
    }
}