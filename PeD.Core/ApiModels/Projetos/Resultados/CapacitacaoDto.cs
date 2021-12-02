using System;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class CapacitacaoDto : ProjetoNodeDto
    {
        public int RecursoId { get; set; }
        public string Recurso { get; set; }
        public string Tipo { get; set; }
        public bool IsConcluido { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string CnpjInstituicao { get; set; }
        public string AreaPesquisa { get; set; }
        public string TituloTrabalhoOrigem { get; set; }
        public int? ArquivoTrabalhoOrigemId { get; set; }
    }
}