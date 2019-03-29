using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace APIGestor.Models
{
    public class XmlProjetoGestao
    {
        public PD_ProjetoGestao PD_ProjetoGestao { get; set; }
    }
    public class PD_ProjetoGestao
    {
        public GstEmpresas Empresas { get; set; }
        public GstAtividades Atividades { get; set; }
        private int _duracao;
        public int Duracao
        {
            get => _duracao = 24;
            set => _duracao = value;
        }
        private string _cpfGerente { get; set; }
        public string CpfGerente{
            get => new Regex(@"[^\d]").Replace(_cpfGerente, "");
            set => _cpfGerente = value;
        }
    }

    public class GstEmpresas
    {
        public List<GstEmpresa> Empresa { get; set; }
    }
    public class GstEmpresa
    {
        private string _tipoEmpresa { get; set; }

        public string TipoEmpresa{
            get => _tipoEmpresa=="Proponente" ? "P" : "C";
            set => _tipoEmpresa = value;
        }
        public string CodEmpresa { get; set; }
        public GstEquipe Equipe { get; set; }
        public GstCustosContabeis CustosContabeis { get; set; }
    }
    public class GstEquipe
    {
        public List<GstEquipeGestao> EquipeGestao{ get; set; }
    }
    public class GstEquipeGestao
    {
        public string NomeMbEqEmp { get; set; }
        private string _cpfMbEqEmp { get; set; }
        public string CpfMbEqEmp{
            get => new Regex(@"[^\d]").Replace(_cpfMbEqEmp, "");
            set => _cpfMbEqEmp = value;
        }
    }
    public class GstCustosContabeis
    {
        public List<GstCustoCatContabil> CustoCatContabil { get; set; }
    }
    public class GstCustoCatContabil
    {
        public string CategoriaContabil { get; set; }
        private string _custoEmpresa{ get; set; }
        public string CustoEmpresa{
            get => string.Format("{0:N}",_custoEmpresa);
            set => _custoEmpresa = value;
        }
    } 
    public class GstAtividades
    {
        public List<GstAtividade> Atividade { get; set; }
    }
    public class GstAtividade
    {
        public string TipoAtividade { get; set; }
        public string DescAtividade { get; set; }
        private string _custoAtividade{ get; set; }
        public string CustoAtividade{
            get => string.Format("{0:N}",_custoAtividade);
            set => _custoAtividade = value;
        }
    }
}