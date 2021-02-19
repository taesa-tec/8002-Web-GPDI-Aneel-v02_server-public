using System;
using PeD.Core.Models.Propostas;

namespace PeD.Core.ApiModels.Propostas
{
    public class PropostaProdutoDto : PropostaNodeDto
    {
        public DateTime Created { get; set; }
        public ProdutoClassificacao Classificacao { get; set; }
        public ProdutoTipo Tipo { get; set; }
        public FaseCadeia FaseCadeia { get; set; }
        public string TipoDetalhado { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}