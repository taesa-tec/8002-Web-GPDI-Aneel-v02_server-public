using System;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoSelecaoFinalizadaDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Proposta { get; set; }
        public string Responsavel { get; set; }
        public DateTime DataAlvo { get; set; }
    }
}