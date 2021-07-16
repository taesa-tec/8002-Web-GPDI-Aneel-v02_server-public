using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class AlocacaoRecursoHumanoDto : BaseEntity
    {
        public int RecursoId { get; set; }
        public string Recurso { get; set; }
        public int EtapaId { get; set; }
        public short Etapa { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public string EmpresaFinanciadora { get; set; }
        public string Justificativa { get; set; }
        public Dictionary<short, short> HoraMeses { get; set; }
        public decimal Valor { get; set; }
    }
}