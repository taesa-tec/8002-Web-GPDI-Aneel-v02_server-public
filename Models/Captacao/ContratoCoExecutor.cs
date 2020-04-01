using APIGestor.Models.Fornecedores;
using TaesaCore.Models;

namespace APIGestor.Models.Captacao
{
    public class ContratoCoExecutor : BaseEntity
    {
       

        public int CoExecutorId { get; set; }
        public CoExecutor CoExecutor { get; set; }
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
        public string Conteudo { get; set; }
        public Contrato.Status Status { get; set; }
    }
}