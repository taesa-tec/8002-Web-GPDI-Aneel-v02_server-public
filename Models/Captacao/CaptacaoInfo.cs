using System;
using TaesaCore.Models;

namespace APIGestor.Models.Captacao
{
    public class CaptacaoInfo : BaseEntity
    {
        public string Titulo { get; set; }
        public string CriadorId { get; set; }
        public string Criador { get; set; }
        public string UsuarioSuprimentoId { get; set; }
        public string UsuarioSuprimento { get; set; }
        public Captacao.CaptacaoStatus Status { get; set; }
        public int TotalConvidados { get; set; }
        public int TotalPropostas { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EnvioCaptacao { get; set; }
        public DateTime? Termino { get; set; }
        public DateTime? Cancelamento { get; set; }
    }
}