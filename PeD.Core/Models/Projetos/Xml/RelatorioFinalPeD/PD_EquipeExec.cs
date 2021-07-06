using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class PD_EquipeExec
    {
        public PedExecutoras Executoras { get; set; }
    }

    public class PedExecutoras
    {
        public List<PedExecutora> Executora { get; set; }
    }

    public class PedExecutora
    {
        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJExec { get; set; }
        public string RazaoSocialExec { get; set; }
        public string UfExec { get; set; }
        public ExecEquipe Equipe { get; set; }
    }

    public class ExecEquipe
    {
        public List<EquipeExec> EquipeExec { get; set; }
    }

    public class EquipeExec
    {
        public string NomeMbEqExec { get; set; }

        /// <summary>
        /// Brasileiro?
        /// </summary>
        ///
        [JsonConverter(typeof(YesOrNoConverter))]
        public bool BRMbEqExec { get; set; }

        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string DocMbEqExec { get; set; }

        public string TitulacaoMbEqExec { get; set; }
        public string FuncaoMbEqExec { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:C}")]
        public decimal HhMbEqExec { get; set; }

        public string MesMbEqExec { get; set; }
        public string HoraMesMbEqExec { get; set; }
    }
}