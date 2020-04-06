using System;
using System.Collections.Generic;
using TaesaCore.Models;

namespace APIGestor.Models.Captacao
{
    public class Captacao : BaseEntity
    {
        public enum CaptacaoStatus
        {
            Pendente,
            Elaboracao,
            Fornecedor,
            Cancelada
        }

        public string Observacoes { get; set; }
        public DateTime DataTermino { get; set; }
        public CaptacaoStatus Status { get; set; }
        public PropostaConfiguracao Configuracao { get; set; }
        public List<CaptacaoSugestaoFornecedor> FornecedoresSugeridos { get; set; }
        public List<CaptacaoArquivo> Files { get; set; }
    }
}