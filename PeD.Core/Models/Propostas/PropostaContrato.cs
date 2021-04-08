using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace PeD.Core.Models.Propostas
{
    [Table("PropostaContratos")]
    public class PropostaContrato : PropostaNode
    {
        
        public int ParentId { get; set; }
        [ForeignKey("ParentId")] public Contrato Parent { get; set; }
        public string Conteudo { get; set; }
        public List<PropostaContratoRevisao> Revisoes { get; set; }
        public bool Finalizado { get; set; }

        public int? FileId { get; set; }
        public FileUpload File { get; set; }

        public override string ToString()
        {
            return Parent?.Titulo;
        }

        public PropostaContratoRevisao ToRevisao() => new PropostaContratoRevisao()
        {
            Conteudo = Conteudo,
            ParentId = Id,
            PropostaId = PropostaId,
            CreatedAt = DateTime.Now
        };
    }

    [Table("PropostaContratosRevisao")]
    public class PropostaContratoRevisao : PropostaNode
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ParentId { get; set; }
        public PropostaContrato Parent { get; set; }
        public string Conteudo { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            var p = Parent?.ToString() ?? "";
            var d = CreatedAt.ToShortDateString();
            return $"{p} - {d}".Trim('-', ' ');
        }
    }
}