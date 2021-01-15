using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Fornecedores;

namespace PeD.Core.Models.Captacoes
{
    public class CaptacaoFornecedor
    {
        public int CaptacaoId { get; set; }
        [ForeignKey("CaptacaoId")] public Captacao Captacao { get; set; }
        public int FornecedorId { get; set; }
        [ForeignKey("FornecedorId")] public Fornecedor Fornecedor { get; set; }
    }
}