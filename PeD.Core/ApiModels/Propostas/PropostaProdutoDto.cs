using System;

namespace PeD.Core.ApiModels.Propostas
{
    public class PropostaProdutoDto : PropostaNodeDto
    {
        public DateTime Created { get; set; }
        public string Classificacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public string TipoId { get; set; }
        public string ProdutoTipo { get; set; }
        public string FaseCadeiaId { get; set; }
        public string FaseCadeia { get; set; }

        public int TipoDetalhadoId { get; set; }
        public string TipoDetalhado { get; set; }
    }
}