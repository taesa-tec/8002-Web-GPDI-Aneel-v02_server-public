using System;
using TaesaCore.Models;

namespace PeD.Dtos.Captacao
{
    public class CaptacaoElaboracaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string UsuarioSuprimento { get; set; }
        public DateTime EnvioCaptacao { get; set; }
    }
}