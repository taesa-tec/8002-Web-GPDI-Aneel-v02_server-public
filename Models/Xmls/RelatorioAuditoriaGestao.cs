using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace APIGestor.Models
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
            get => string.Format("{0:N}",_custoTotal);
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
            get => string.Format("{0:N}",_custoEmpresa);
            set => _custoEmpresa = value;
        }
    } 

}