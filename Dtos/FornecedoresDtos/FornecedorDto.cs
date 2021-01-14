using System.Collections.Generic;
using TaesaCore.Models;

namespace APIGestor.Dtos.FornecedoresDtos
{
    public class FornecedorDto : BaseEntity
    {
        public string Nome { get; set; }
        public string CNPJ { get; set; }

        public string ResponsavelId { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelEmail { get; set; }
        public bool Ativo { get; set; }
        public List<CoExecutorDto> CoExecutores { get; set; }
    }
}