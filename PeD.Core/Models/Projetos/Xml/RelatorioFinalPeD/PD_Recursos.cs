using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class PD_Recursos
    {
        public List<RecursoEmpresa> RecursoEmpresa { get; set; }
        public List<RecursoParceira> RecursoParceira { get; set; }
    }

    public class RecursoEmpresa
    {
        public string CodEmpresa { get; set; }
        public DestRecursos DestRecursos { get; set; }
    }

    public class RecursoParceira
    {
        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJParc { get; set; }

        public List<DestRecursosExec> DestRecursosExec { get; set; }
    }

    public class DestRecursos
    {
        public List<DestRecursosExec> DestRecursosExec { get; set; }
        public DestRecursosEmp DestRecursosEmp { get; set; }
    }

    public class CustoCatContabil<T>
    {
        public string CategoriaContabil { get; set; }
        public List<T> ItemDespesa { get; set; }
    }

    public class DestRecursosExec
    {
        [JsonConverter(typeof(OnlyDigitsConverter))]
        public string CNPJExec { get; set; }

        public List<CustoCatContabil<ItemDespesa>> CustoCatContabil { get; set; }
    }

    public class DestRecursosEmp
    {
        public List<CustoCatContabil<ItemDespesa>> CustoCatContabil { get; set; }
    }
}