using System.Collections.Generic;
using System.Linq;

namespace PeD.Core.Models.Relatorios.Fornecedores
{
    public class AlocacaoRecurso
    {
        public decimal Valor { get; set; }
        public string CategoriaContabil { get; set; }
        public string EmpresaFinanciadora { get; set; }
        public string EmpresaFinanciadoraCodigo { get; set; }
        public string EmpresaRecebedora { get; set; }
        public string EmpresaRecebedoraCodigo { get; set; }
    }
}