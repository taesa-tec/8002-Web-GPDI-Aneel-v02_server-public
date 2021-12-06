using System;
using System.ComponentModel.DataAnnotations;
using PeD.Core.Models.Catalogos;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class ProducaoCientifica : ProjetoNode
    {
        public enum TipoProducao
        {
            PN,
            PI,
            AN,
            AI
        }

        public TipoProducao Tipo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool ConfirmacaoPublicacao { get; set; }
        [MaxLength(50)] public string NomeEventoPublicacao { get; set; }

        [MaxLength(50)]
        public string LinkPublicacao { get; set; }

        public int PaisId { get; set; }
        public Pais Pais { get; set; }
        [MaxLength(30)] public string Cidade { get; set; }
        [MaxLength(200)] public string TituloTrabalho { get; set; }

        public int? ArquivoTrabalhoOrigemId { get; set; }
        public FileUpload ArquivoTrabalhoOrigem { get; set; }
    }
}