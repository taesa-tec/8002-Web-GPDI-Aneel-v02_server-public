using System;
using System.Collections.Generic;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoDto
    {
        public string Observacoes { get; set; }
        public DateTime DataTermino { get; set; }
        public string Status { get; set; }
        public List<int> FornecedorsSugeridos { get; set; }
        public List<int> Files { get; set; }
    }
}