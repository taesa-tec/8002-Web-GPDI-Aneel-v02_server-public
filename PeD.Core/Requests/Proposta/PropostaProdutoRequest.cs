using System;

namespace PeD.Core.Requests.Proposta
{
    public class PropostaProdutoRequest
    {
        public DateTime Created { get; set; }
        public string Classificacao { get; set; }
        public string Tipo { get; set; }
        public string FaseCadeia { get; set; }
        public string TipoDetalhado { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}