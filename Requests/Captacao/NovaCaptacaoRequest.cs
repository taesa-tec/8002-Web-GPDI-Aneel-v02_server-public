using System;
using System.Collections.Generic;

namespace APIGestor.Requests.Captacao
{
    public class NovaCaptacaoRequest
    {
        public int Id { get; set; }
        public string Observacoes { get; set; }
        public List<int> FornecedorsSugeridos { get; set; }
    }
}