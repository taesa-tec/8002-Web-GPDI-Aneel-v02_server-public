using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Fornecedores
{
    public class Fornecedor : Empresa
    {
        public new CategoriaEmpresa Categoria { get; } = CategoriaEmpresa.Fornecedor;
        public string ResponsavelId { get; set; }
        public ApplicationUser Responsavel { get; set; }
    }
}