using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class PD_Resultados
    {
        public PD_ResultadosCP PD_ResultadosCP { get; set; }
        public PD_ResultadosCT PD_ResultadosCT { get; set; }
        public PD_ResultadosSA PD_ResultadosSA { get; set; }
        public PD_ResultadosIE PD_ResultadosIE { get; set; }
    }

    #region PD_ResultadosCP

    public class PD_ResultadosCP
    {
        public List<IdCP> IdCP { get; set; }
    }

    public class IdCP
    {
        public string TipoCP { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool ConclusaoCP { get; set; }

        [JsonConverter(typeof(DateXmlConverter))]
        public DateTime DataCP { get; set; }

        public string DocMmbEqCP { get; set; }

        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJInstCP { get; set; }

        public string AreaCP { get; set; }
        public string TituloCP { get; set; }
        public string ArquivoPDF { get; set; }
    }

    #endregion

    #region PD_ResultadosCT

    public class PD_ResultadosCT
    {
        public PD_ResultadosCT_PC PD_ResultadosCT_PC { get; set; }
        public PD_ResultadosCT_IE PD_ResultadosCT_IE { get; set; }
        public PD_ResultadosCT_PI PD_ResultadosCT_PI { get; set; }
    }

    public class PD_ResultadosCT_PC
    {
        public List<IdCT_PC> IdCT_PC { get; set; }
    }

    public class IdCT_PC
    {
        public string TipoCT_PC { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool ConfPubCT_PC { get; set; }

        [JsonConverter(typeof(DateXmlConverter))]
        public DateTime DataCT_PC { get; set; }

        public string NomeCT_PC { get; set; }
        public string LinkCT_PC { get; set; }
        public string PaisCT_PC { get; set; }
        public string CidadeCT_PC { get; set; }
        public string TituloCT_PC { get; set; }
        public string ArquivoPDF { get; set; }
    }

    public class PD_ResultadosCT_IE
    {
        public List<IdCT_IE> IdCT_IE { get; set; }
    }

    public class IdCT_IE
    {
        public string TipoCT_IE { get; set; }

        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJInstBenefCT_IE { get; set; }

        public string NomeLabCT_IE { get; set; }
        public string AreaLabCT_IE { get; set; }
        public string ApoioLabCT_IE { get; set; }
    }

    public class PD_ResultadosCT_PI
    {
        public List<IdCT_PI> IdCT_PI { get; set; }
    }

    public class IdCT_PI
    {
        public string TipoCT_PI { get; set; }

        [JsonConverter(typeof(DateXmlConverter))]
        public DateTime DataCT_PI { get; set; }

        public string NumeroCT_PI { get; set; }
        public string TituloCT_PI { get; set; }
        public Inventores_PI Inventores_PI { get; set; }
        public Depositantes_PI Depositantes_PI { get; set; }
    }

    public class Inventores_PI
    {
        public List<Inventor_PI> Inventor { get; set; }
    }

    public class Inventor_PI
    {
        public string DocMbEqCT_PI { get; set; }
    }

    public class Depositantes_PI
    {
        public List<Depositante_PI> Depositante { get; set; }
    }

    public class Depositante_PI
    {
        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJInstCT_PI { get; set; }

        public decimal PercInstCT_PI { get; set; }
    }

    #endregion

    #region PD_ResultadosSA

    public class PD_ResultadosSA
    {
        public List<IdSA> IdSA { get; set; }
    }

    public class IdSA
    {
        public string TipoISA { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool PossibISA { get; set; }

        public string TxtISA { get; set; }
    }

    #endregion

    #region PD_ResultadosIE

    public class PD_ResultadosIE
    {
        public List<IdIE> IdIE { get; set; }
    }

    public class IdIE
    {
        public string TipoIE { get; set; }
        public string TxtBenefIE { get; set; }
        public string UnidBenefIE { get; set; }
        public decimal BaseBenefIE { get; set; }
        public decimal PerBenefIE { get; set; }
        public decimal VlrBenefIE { get; set; }
    }

    #endregion
}