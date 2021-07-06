using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PeD.Core.Converters;

namespace PeD.Core.Models.Projetos.Xml.RelatorioFinalPeD
{
    public class PD_EquipeEmp
    {
        public PedEmpresas Empresas { get; set; }
    }

    public class PedEmpresas
    {
        public List<PedEmpresa> Empresa { get; set; }
    }

    public class PedEmpresa
    {
        public string CodEmpresa { get; set; }
        private string _tipoEmpresa { get; set; }

        public string TipoEmpresa
        {
            get => _tipoEmpresa == "Proponente" ? "P" : "C";
            set => _tipoEmpresa = value;
        }

        public Equipe Equipe { get; set; }
    }

    public class Equipe
    {
        public List<EquipeEmpresa> EquipeEmpresa { get; set; }
    }

    public class EquipeEmpresa
    {
        public string NomeMbEqEmp { get; set; }
        private string _cpfMbEqEmp { get; set; }

        public string CpfMbEqEmp
        {
            get => new Regex(@"[^\d]").Replace(_cpfMbEqEmp, "");
            set => _cpfMbEqEmp = value;
        }

        public string TitulacaoMbEqEmp { get; set; }
        public string FuncaoMbEqEmp { get; set; }

        [JsonConverter(typeof(NumberConverter), "{0:N}")]
        public decimal HhMbEqEmp { get; set; }

        public string MesMbEqEmp { get; set; }
        public string HoraMesMbEqEmp { get; set; }
    }
}