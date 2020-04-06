using System;
using System.Collections.Generic;
using TaesaCore.Models;

namespace APIGestor.Models.Captacao
{
    public class PropostaConfiguracao : BaseEntity
    {
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
        public string Consideracoes { get; set; }
        public DateTime DataMaxima { get; set; }
        public List<CaptacaoFornecedor> Fornecedores { get; set; }
    }
}