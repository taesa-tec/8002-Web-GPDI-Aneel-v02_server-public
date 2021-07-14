using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class AlocacaoRecursoHumanoRequest : BaseEntity
    {
        public int RecursoId { get; set; }
        public int EtapaId { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public string Justificativa { get; set; }
        public Dictionary<short, short> HoraMeses { get; set; }
    }
}