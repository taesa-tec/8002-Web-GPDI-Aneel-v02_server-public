using System.Collections.Generic;
using System.Linq;
using PeD.Core.Models.Propostas;

namespace PeD.Core.Models.Relatorios.Fornecedores
{
    public class AlocacaoRecurso
    {
        public decimal Valor { get; set; }
        public string CategoriaContabil { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public string EmpresaFinanciadora { get; set; }
        public Funcao EmpresaFinanciadoraFuncao { get; set; }
        public int EmpresaRecebedoraId { get; set; }
        public string EmpresaRecebedora { get; set; }
        public Funcao EmpresaRecebedoraFuncao { get; set; }
    }
}