using System;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoPendenteDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Criador { get; set; }
        public DateTime Aprovacao { get; set; }
    }
}