using System.Collections.Generic;
using PeD.Core.ApiModels.Propostas;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Fornecedores
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