using System;

namespace PeD.Core.Requests.Projetos
{
    public class RegistroRhRequest
    {
        public long RecursoHumanoId { get; set; }
        public long Horas { get; set; }
        public string AtividadeRealizada { get; set; }
        public long FinanciadoraId { get; set; }
        public string CoExecutorFinanciadorId { get; set; }
        public DateTimeOffset MesReferencia { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTimeOffset DataDocumento { get; set; }
        public string ObservacaoInterna { get; set; }
    }
}