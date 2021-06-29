using System;
using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class Capacitacao : ProjetoNode
    {
        public enum TipoCapacitacao
        {
            PD,
            DO,
            ME,
            ES
        }

        public int RecursoId { get; set; }
        public RecursoHumano Recurso { get; set; }
        public TipoCapacitacao Tipo { get; set; }
        public bool IsConcluido { get; set; }
        public DateTime? DataConclusao { get; set; }
        [MaxLength(20)] public string CnpjInstituicao { get; set; }
        [MaxLength(50)] public string AreaPesquisa { get; set; }
        [MaxLength(200)] public string TituloTrabalhoOrigem { get; set; }

        public int? ArquivoTrabalhoOrigemId { get; set; }
        public FileUpload ArquivoTrabalhoOrigem { get; set; }
    }
}