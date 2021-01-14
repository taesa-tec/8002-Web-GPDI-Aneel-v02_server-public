using PeD.Models.Fornecedores;
using TaesaCore.Models;

namespace PeD.Models.Captacao
{
    public class ContratoCoExecutor : BaseEntity
    {
       

        public int CoExecutorId { get; set; }
        public CoExecutor CoExecutor { get; set; }
        public int PropostaId { get; set; }
        public PropostaFornecedor PropostaFornecedor { get; set; }
        public string Conteudo { get; set; }
        public Contrato.Status Status { get; set; }
    }
}