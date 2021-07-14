using System;
using PeD.Core.Models.Projetos;

namespace PeD.Core.Requests.Projetos
{
    public class RegistroRhRequest
    {
        public int EtapaId { get; set; }
        public int RecursoHumanoId { get; set; }
        public int FinanciadoraId { get; set; }
        public int Horas { get; set; }
        public string AtividadeRealizada { get; set; }
        public DateTime MesReferencia { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public string ObservacaoInterna { get; set; }
    }
}