using System;

namespace PeD.Core.Requests.Projetos
{
    public class ProrrogacaoRequest
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public int ProdutoId { get; set; }
    }
}