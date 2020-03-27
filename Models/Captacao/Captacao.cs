using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public List<CaptacaoFornecedor> FornecedorsSugeridos { get; set; }
        public List<CaptacaoFile> Files { get; set; }
    }
}