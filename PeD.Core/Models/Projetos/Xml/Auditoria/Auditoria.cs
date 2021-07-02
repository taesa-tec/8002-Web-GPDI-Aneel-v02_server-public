using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.Auditoria
{
    public class Auditoria : BaseXml
    {
        [JsonIgnore] public override XmlTipo Tipo => XmlTipo.RELATORIOAUDITORIAPED;
        public PD_RelAuditoriaPED PD_RelAuditoriaPED { get; set; }
    }

    public class PD_RelAuditoriaPED
    {
        public string CodProjeto { get; set; }
        public string ArquivoPDF { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:C}")]
        public decimal CustoTotal { get; set; }

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
        public string CNPJExec { get; set; }
        public List<CustoCatContabil> CustoCatContabil { get; set; }
    }

    public class CustoCatContabil
    {
        public string CatContabil { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:C}")]
        public decimal CustoExec { get; set; }
    }

    public class DestRecursosEmp
    {
        public List<CustoCatContabil> CustoCatContabil { get; set; }
    }


    public class RecursoParceira
    {
        public string CNPJParc { get; set; }
        public List<DestRecursosExec> DestRecursosExec { get; set; }
    }
}