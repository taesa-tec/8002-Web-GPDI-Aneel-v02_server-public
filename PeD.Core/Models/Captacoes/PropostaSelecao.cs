using System;
using TaesaCore.Models;

namespace PeD.Core.Models.Captacoes
{
    public class PropostaSelecao : BaseEntity
    {
        public string UsuarioRefinamentoId { get; set; }
        public ApplicationUser UsuarioRefinamento { get; set; }

        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }

        public DateTime DataAlvo { get; set; }

        public int ArquivoComprobatoriId { get; set; }
        public FileUpload ArquivoComprobatorio { get; set; }
    }
}