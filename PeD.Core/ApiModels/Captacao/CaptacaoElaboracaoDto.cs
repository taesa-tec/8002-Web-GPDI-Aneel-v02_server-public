using System;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoElaboracaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string UsuarioSuprimento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EnvioCaptacao { get; set; }
        public DateTime? Termino { get; set; }
        public DateTime? Cancelamento { get; set; }
    }
}