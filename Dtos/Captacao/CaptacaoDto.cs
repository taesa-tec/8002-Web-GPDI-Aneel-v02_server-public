using System;
using System.Collections.Generic;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string CriadorId { get; set; }
        public string Criador { get; set; }
        public string UsuarioSuprimentoId { get; set; }
        public string UsuarioSuprimento { get; set; }
        public DateTime DataTermino { get; set; }
        public string Status { get; set; }
        public int ConvidadosTotal { get; set; }
        public int PropostaTotal { get; set; }
    }
}