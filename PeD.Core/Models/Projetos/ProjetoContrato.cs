using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoContratos")]
    public class ProjetoContrato : ProjetoNode
    {
        
        public int ParentId { get; set; }
        [ForeignKey("ParentId")] public Contrato Parent { get; set; }
        public string Conteudo { get; set; }
        public List<ProjetoContratoRevisao> Revisoes { get; set; }
        public bool Finalizado { get; set; }

        public int? FileId { get; set; }
        public FileUpload File { get; set; }

        public override string ToString()
        {
            return Parent?.Titulo;
        }

        public ProjetoContratoRevisao ToRevisao() => new ProjetoContratoRevisao()
        {
            Conteudo = Conteudo,
            ParentId = Id,
            ProjetoId = ProjetoId,
            CreatedAt = DateTime.Now
        };
    }

    [Table("ProjetoContratosRevisao")]
    public class ProjetoContratoRevisao : ProjetoNode
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ParentId { get; set; }
        public ProjetoContrato Parent { get; set; }
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