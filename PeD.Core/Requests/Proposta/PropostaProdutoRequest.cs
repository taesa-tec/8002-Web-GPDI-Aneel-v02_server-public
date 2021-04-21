using System;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class PropostaProdutoRequest : BaseEntity
    {
        public string Classificacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public string TipoId { get; set; }
        public string FaseCadeiaId { get; set; }
        public int TipoDetalhadoId { get; set; }
    }
}