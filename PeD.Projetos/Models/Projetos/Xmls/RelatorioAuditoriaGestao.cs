using System.Collections.Generic;

namespace PeD.Projetos.Models.Projetos.Xmls
{
    public class XmlRelatorioAuditoriaGestao
    {
        public PD_RelAuditoriaPG PD_RelAuditoriaPG { get; set; }
    }
    public class PD_RelAuditoriaPG
    {
        public string CodProjeto { get; set; }
        public string ArquivoPDF { get; set; }
        private string _custoTotal{ get; set; }
        public string CustoTotal{
            get => decimal.Parse(_custoTotal).ToString("N2");
            set => _custoTotal = value;
        }
        public List<RAG_RecursoEmpresa> RecursoEmpresa { get; set; }
    }
    public class RAG_RecursoEmpresa
    {
        public string CodEmpresa { get; set; }
        public List<RAG_CustoCatContabil> CustoCatContabil { get; set; }
    }
    public class RAG_CustoCatContabil
    {
        public string CatContabil { get; set; }
        private string _custoEmpresa{ get; set; }
        public string CustoEmpresa{
            get => decimal.Parse(_custoEmpresa).ToString("N2");
            set => _custoEmpresa = value;
        }
    } 

}