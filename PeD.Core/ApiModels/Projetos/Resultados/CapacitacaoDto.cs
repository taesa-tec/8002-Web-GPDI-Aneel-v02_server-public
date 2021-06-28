using System;
using PeD.Core.Models;
using PeD.Core.Models.Projetos;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class CapacitacaoDto : ProjetoNodeDto
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
         public string CnpjInstituicao { get; set; }
         public string AreaPesquisa { get; set; }
         public string TituloTrabalhoOrigem { get; set; }

        public int ArquivoTrabalhoOrigemId { get; set; }
        public FileUpload ArquivoTrabalhoOrigem { get; set; }
    }
}