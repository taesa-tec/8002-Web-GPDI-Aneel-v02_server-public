using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PeD.Models.Projetos.Xmls
{
    public class XmlRelatorioAuditoria
    {
        public PD_RelAuditoriaPED PD_RelAuditoriaPED { get; set; }
    }
    public class PD_RelAuditoriaPED
    {
        public string CodProjeto { get; set; }
        public string ArquivoPDF { get; set; }
        private string _custoTotal{ get; set; }
        public string CustoTotal{
            get => decimal.Parse(_custoTotal).ToString("N2");
            set => _custoTotal = value;
        }
        public List<RA_RecursoEmpresa> RecursoEmpresa { get; set; }
        public List<RA_RecursoParceira> RecursoParceira { get; set; }
    }
    public class RA_RecursoEmpresa
    {
        public string CodEmpresa { get; set; }
        public List<RA_DestRecursosExec> DestRecursosExc { get; set; }
        public List<RA_DestRecursosEmp> DestRecursosEmp { get; set; }
    }
    public class RA_DestRecursosExec
    {
        private string _cnpjExec{ get; set; }
        public string CNPJExec{
            get => new Regex(@"[^\d]").Replace(_cnpjExec, "");
            set => _cnpjExec = value;
        }
        public List<CustoCatContabilExec> CustoCatContabil { get; set; }
    }
    public class RA_DestRecursosEmp
    {
        public List<CustoCatContabilEmp> CustoCatContabil { get; set; }
    }
    public class RA_RecursoParceira
    {
        private string _cnpjParc{ get; set; }
        public string CNPJParc{
            get => new Regex(@"[^\d]").Replace(_cnpjParc, "");
            set => _cnpjParc = value;
        }
        public List<RA_DestRecursosExec> DestRecursosExec { get; set; }
    }
}