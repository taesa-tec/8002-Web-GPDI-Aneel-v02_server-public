using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Converters;

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class PD_Etapa
    {
        [JsonConverter(typeof(NumberConverter), "{0:00}")]
        public decimal EtapaN { get; set; }

        public string Atividades { get; set; }
        public string MesExecEtapa { get; set; }
    }

    public class PD_Etapas
    {
        public List<PD_Etapa> Etapa { get; set; }
    }
}