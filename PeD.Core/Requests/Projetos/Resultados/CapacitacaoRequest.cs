using System;
using System.ComponentModel.DataAnnotations;
using PeD.Core.Models;
using PeD.Core.Models.Projetos;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class CapacitacaoRequest : BaseEntity
    {
        public int RecursoId { get; set; }
        public string Tipo { get; set; }
        public bool IsConcluido { get; set; }
        public DateTime? DataConclusao { get; set; }
        [MaxLength(20)] public string CnpjInstituicao { get; set; }
        [MaxLength(50)] public string AreaPesquisa { get; set; }
        [MaxLength(200)] public string TituloTrabalhoOrigem { get; set; }
    }
}