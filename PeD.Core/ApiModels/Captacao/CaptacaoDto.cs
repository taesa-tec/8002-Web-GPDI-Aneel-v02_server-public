using System;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string CriadorId { get; set; }
        public string Criador { get; set; }
        public string UsuarioSuprimentoId { get; set; }
        public string UsuarioSuprimento { get; set; }
        public DateTime? Termino { get; set; }
        public string Status { get; set; }
        public int ConvidadosTotal { get; set; }
        public int PropostaTotal { get; set; }
    }
}