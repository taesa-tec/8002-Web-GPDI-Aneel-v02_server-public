using System;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoElaboracaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Equipe { get; set; }
        public DateTime EnvioCaptacao { get; set; }
    }
}