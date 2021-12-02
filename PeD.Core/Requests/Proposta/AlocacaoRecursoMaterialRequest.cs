using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class AlocacaoRecursoMaterialRequest : BaseEntity
    {
        public int RecursoId { get; set; }
        public int EtapaId { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public int EmpresaRecebedoraId { get; set; }
        public int Quantidade { get; set; }
        public string Justificativa { get; set; }
        public Dictionary<short, short> HoraMeses { get; set; }
    }
}