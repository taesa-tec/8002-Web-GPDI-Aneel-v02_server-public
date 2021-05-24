using System;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;
using TaesaCore.Extensions;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoRelatorios")]
    public class Relatorio : ProjetoNode
    {
        public string Content { get; set; }

        public string Hash => Content.Trim().ToMD5();

        public DateTime DataAlteracao { get; set; }

        public ValidationResult Validacao { get; set; }
        
        public int? FileId { get; set; }
        public FileUpload File { get; set; }
    }
}