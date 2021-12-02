using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.ProjetoPeD
{
    public class ProjetoPed : BaseXml
    {
        [JsonIgnore] public override XmlTipo Tipo => XmlTipo.PROJETOPED;
        public PD_ProjetoBase PD_ProjetoBase { get; set; }
        public PD_Equipe PD_Equipe { get; set; }
        public PD_Recursos PD_Recursos { get; set; }
    }

    public class PD_ProjetoBase
    {
        [JsonConverter(typeof(YesOrNoConverter))]
        public bool AvIniANEEL { get; set; }

        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Segmento { get; set; }
        public string CodTema { get; set; }
        public string OutroTema { get; set; }
        public SubTemas Subtemas { get; set; }
        public string FaseInovacao { get; set; }
        public string TipoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Motivacao { get; set; }
        public string Originalidade { get; set; }
        public string Aplicabilidade { get; set; }
        public string Relevancia { get; set; }
        public string RazoabCustos { get; set; }
        public string PesqCorrelata { get; set; }
    }

    public class SubTemas
    {
        public List<SubTema> Subtema { get; set; }
    }

    public class SubTema
    {
        public string CodSubtema { get; set; }
        public string OutroSubtema { get; set; }
    }

    public class PD_Equipe
    {
        public Empresas Empresas { get; set; }
        public Executoras Executoras { get; set; }
    }

    public class Empresas
    {
        public List<Empresa> Empresa { get; set; }
    }

    public class Empresa
    {
        public string CodEmpresa { get; set; }

        /// <summary>
        /// Verdadeiro se for empresa proponente
        /// </summary>
        [JsonConverter(typeof(YesOrNoConverter), "P", "C")]
        public bool TipoEmpresa { get; set; }

        public Equipe Equipe { get; set; }
    }

    public class Equipe
    {
        public List<EquipeEmpresa> EquipeEmpresa { get; set; }
    }

    public class EquipeEmpresa
    {
        public string NomeMbEqEmp { get; set; }

        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CpfMbEqEmp { get; set; }

        public string TitulacaoMbEqEmp { get; set; }
        public string FuncaoMbEqEmp { get; set; }
    }

    public class Executoras
    {
        public List<Executora> Executora { get; set; }
    }

    public class Executora
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

        public bool BRMbEqExec { get; set; }

        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string DocMbEqExec { get; set; }

        public string TitulacaoMbEqExec { get; set; }
        public string FuncaoMbEqExec { get; set; }
    }

    public class PD_Recursos
    {
        public List<RecursoEmpresa> RecursoEmpresa { get; set; }
        public List<RecursoParceira> RecursoParceira { get; set; }
    }

    public class RecursoEmpresa
    {
        public string CodEmpresa { get; set; }
        public List<DestRecursosExec> DestRecursosExec { get; set; }
        public DestRecursosEmp DestRecursosEmp { get; set; }
    }

    public class DestRecursosExec
    {
        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJExec { get; set; }

        public List<CustoCatContabilExec> CustoCatContabilExec { get; set; }
    }

    public class CustoCatContabilExec
    {
        public string CatContabil { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:N2}")]
        public decimal CustoExec { get; set; }
    }

    public class DestRecursosEmp
    {
        public List<CustoCatContabilEmp> CustoCatContabilEmp { get; set; }
    }

    public class CustoCatContabilEmp
    {
        public string CatContabil { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:N2}")]
        public decimal CustoEmp { get; set; }
    }

    public class RecursoParceira
    {
        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJParc { get; set; }

        public List<DestRecursosExec> DestRecursosExec { get; set; }
    }
}