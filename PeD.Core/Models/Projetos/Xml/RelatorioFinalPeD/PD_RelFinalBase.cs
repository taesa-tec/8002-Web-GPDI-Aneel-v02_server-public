using System;
using System.Globalization;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class PD_RelFinalBase
    {
        public string CodProjeto { get; set; }
        public string ArquivoPDF { get; set; }

        [JsonConverter(typeof(DateXmlConverter))]
        public DateTime DataIniODS { get; set; }

        [JsonConverter(typeof(DateXmlConverter))]
        public DateTime DataFimODS { get; set; }

        public string TxDifTec { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool ProdPrev { get; set; }


        [JsonConverter(typeof(YesOrNoConverter))]
        public bool AplicPrev { get; set; }

        public string ProdJust { get; set; }
        public string ProdEspTec { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool TecPrev { get; set; }

        public string TecJust { get; set; }
        public string TecDesc { get; set; }

        public string AplicJust { get; set; }
        public string AplicFnc { get; set; }
        public string AplicAbrang { get; set; }
        public string AplicAmbito { get; set; }
    }
}