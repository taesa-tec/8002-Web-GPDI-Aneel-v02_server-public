using System.ComponentModel.DataAnnotations.Schema;
using PeD.Models.Fornecedores;

namespace PeD.Models.Captacao
{
    public class CaptacaoFornecedor
    {
        public int CaptacaoId { get; set; }
        [ForeignKey("CaptacaoId")] public Captacao Captacao { get; set; }
        public int FornecedorId { get; set; }
        [ForeignKey("FornecedorId")] public Fornecedor Fornecedor { get; set; }
    }
}