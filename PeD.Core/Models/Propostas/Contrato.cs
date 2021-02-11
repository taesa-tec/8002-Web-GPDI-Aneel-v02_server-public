using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace PeD.Core.Models.Propostas
{
    [Table("PropostaContratos")]
    public class Contrato : PropostaNode
    {
        public int ParentId { get; set; }
        public Models.Contrato Parent { get; set; }
        public string Conteudo { get; set; }
        public List<ContratoRevisao> Revisoes { get; set; }
        public bool Finalizado { get; set; }
    }

    [Table("PropostaContratosRevisao")]
    public class ContratoRevisao : PropostaNode
    {
        public int ContratoId { get; set; }
        public Contrato Parent { get; set; }
        public string Conteudo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}