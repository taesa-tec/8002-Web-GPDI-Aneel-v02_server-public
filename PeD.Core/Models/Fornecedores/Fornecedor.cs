using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Fornecedores
{
    public class Fornecedor : Empresa
    {
        public string ResponsavelId { get; set; }
        public ApplicationUser Responsavel { get; set; }
    }
}