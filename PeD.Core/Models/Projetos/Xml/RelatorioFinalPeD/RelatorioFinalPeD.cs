using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class RelatorioFinalPeD : BaseXml
    {
        [JsonIgnore] public override XmlTipo Tipo => XmlTipo.RELATORIOFINALPED;


        public PD_RelFinalBase PD_RelFinalBase { get; set; }
        public PD_EquipeEmp PD_EquipeEmp { get; set; }
        public PD_EquipeExec PD_EquipeExec { get; set; }
        public PD_Etapas PD_Etapas { get; set; }
        public PD_Recursos PD_Recursos { get; set; }
        public PD_Resultados PD_Resultados { get; set; }
    }

    public class ItemDespesaBase
    {
        public string NomeItem { get; set; }
        public string JustificaItem { get; set; }
        public int? QtdeItem { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:N2}")]
        public decimal ValorIndItem { get; set; }

        [JsonConverter(typeof(YesOrNoConverter), "N","I")]// (N)acional (I)nternacional
        public bool TipoItem { get; set; }
    }

    public class ItemDespesa : ItemDespesaBase
    {
        [JsonConverter(typeof(YesOrNoConverter))]
        public bool ItemLabE { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool ItemLabN { get; set; }
    }
}