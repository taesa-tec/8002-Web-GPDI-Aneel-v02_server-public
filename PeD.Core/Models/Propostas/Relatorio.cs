using System;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;
using PeD.Core.Extensions;
using TaesaCore.Extensions;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaRelatorios")]
    public class Relatorio : PropostaNode
    {
        public string Content { get; set; }

        public string Hash => Content.Trim().ToSHA256();

        public DateTime DataAlteracao { get; set; }

        public ValidationResult Validacao { get; set; }
        
        public int? FileId { get; set; }
        public FileUpload File { get; set; }
    }
}