using System;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoElaboracaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string UsuarioSuprimento { get; set; }
        public DateTime EnvioCaptacao { get; set; }
    }
}