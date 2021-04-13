using System;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class PropostaSelecaoDto : BaseEntity
    {
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public short Duracao { get; set; }
        public bool ContratoFinalizado { get; set; }
        public bool PlanoFinalizado { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataTermino { get; set; }
        public DateTime? DataResposta { get; set; }
    }
}