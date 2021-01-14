using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PeD.Models.Projetos.Xmls {
    public class ProjetoPed {
        public PD_ProjetoBase PD_ProjetoBase { get; set; }
        public PD_Equipe PD_Equipe { get; set; }
        public PD_Recursos PD_Recursos { get; set; }
    }
    public class PD_ProjetoBase {
        private string _avIniANEEL;
        public string AvIniANEEL {
            get => _avIniANEEL == "True" ? "S" : "N";
            set => _avIniANEEL = value;
        }
        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Segmento { get; set; }
        public string CodTema { get; set; }
        public string OutroTema { get; set; }
        public PedSubTemas Subtemas { get; set; }
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
    public class PedSubTemas {
        public List<PedSubTema> Subtema { get; set; }
    }
    public class PedSubTema {
        public string CodSubtema { get; set; }
        public string OutroSubtema { get; set; }
    }
    public class PD_Equipe {
        public PedEmpresas Empresas { get; set; }
        public PedExecutoras Executoras { get; set; }
    }
    public class PedEmpresas {
        public List<PedEmpresa> Empresa { get; set; }
    }
    public class PedEmpresa {
        public string CodEmpresa { get; set; }
        private string _tipoEmpresa { get; set; }
        public string TipoEmpresa {
            get => _tipoEmpresa == "Proponente" ? "P" : "C";
            set => _tipoEmpresa = value;
        }
        public Equipe Equipe { get; set; }
    }
    public class Equipe {
        public List<EquipeEmpresa> EquipeEmpresa { get; set; }
    }
    public class EquipeEmpresa {
        public string NomeMbEqEmp { get; set; }
        private string _cpfMbEqEmp { get; set; }
        public string CpfMbEqEmp {
            get => new Regex(@"[^\d]").Replace(_cpfMbEqEmp, "");
            set => _cpfMbEqEmp = value;
        }
        public string TitulacaoMbEqEmp { get; set; }
        public string FuncaoMbEqEmp { get; set; }
        private string _hhMbEqEmp { get; set; }
        public string HhMbEqEmp {
            get => string.Format("{0:N}", _hhMbEqEmp);
            set => _hhMbEqEmp = value;
        }
        public string MesMbEqEmp { get; set; }
        public string HoraMesMbEqEmp { get; set; }
    }
    public class PedExecutoras {
        public List<PedExecutora> Executora { get; set; }
    }
    public class PedExecutora {
        private string _cnpjExec { get; set; }
        public string CNPJExec {
            get => new Regex(@"[^\d]").Replace(_cnpjExec, "");
            set => _cnpjExec = value;
        }
        public string RazaoSocialExec { get; set; }
        public string UfExec { get; set; }
        public ExecEquipe Equipe { get; set; }
    }
    public class ExecEquipe {
        public List<EquipeExec> EquipeExec { get; set; }
    }
    public class EquipeExec {
        public string NomeMbEqExec { get; set; }
        private string _bRMbEqExec { get; set; }
        public string BRMbEqExec {
            get => _bRMbEqExec == "Brasileiro" ? "S" : "N";
            set => _bRMbEqExec = value;
        }
        private string _docMbEqExec { get; set; }
        public string DocMbEqExec {
            get => new Regex(@"[^\d]").Replace(_docMbEqExec, "");
            set => _docMbEqExec = value;
        }
        public string TitulacaoMbEqExec { get; set; }
        public string FuncaoMbEqExec { get; set; }
        public string HhMbEqExec { get; set; }
        public string MesMbEqExec { get; set; }
        public string HoraMesMbEqExec { get; set; }
    }
    public class PD_Recursos {
        public List<RecursoEmpresa> RecursoEmpresa { get; set; }
        public List<RecursoParceira> RecursoParceira { get; set; }
    }
    public class RecursoEmpresa {
        public string CodEmpresa { get; set; }
        public List<DestRecursosExec> DestRecursosExec { get; set; }
        public DestRecursosEmp DestRecursosEmp { get; set; }
    }
    public class DestRecursosExec {
        private string _cnpjExec { get; set; }
        public string CNPJExec {
            get => new Regex(@"[^\d]").Replace(_cnpjExec, "");
            set => _cnpjExec = value;
        }
        public List<CustoCatContabilExec> CustoCatContabilExec { get; set; }
    }
    public class CustoCatContabilExec {
        public string CatContabil { get; set; }
        private string _custoExec { get; set; }
        public string CustoExec {
            get => decimal.Parse(_custoExec).ToString("N2");
            set => _custoExec = value;
        }
    }
    public class DestRecursosEmp {
        public List<CustoCatContabilEmp> CustoCatContabilEmp { get; set; }
    }
    public class CustoCatContabilEmp {
        public string CatContabil { get; set; }
        private string _custoEmp { get; set; }
        public string CustoEmp {
            get => decimal.Parse(_custoEmp).ToString("N2");
            set => _custoEmp = value;
        }
    }
    public class RecursoParceira {
        private string _cnpjParc { get; set; }
        public string CNPJParc {
            get => new Regex(@"[^\d]").Replace(_cnpjParc, "");
            set => _cnpjParc = value;
        }
        public List<DestRecursosExec> DestRecursosExec { get; set; }
    }

    public enum XmlTipo {
        MOVIMENTACAOFINANCEIRA = 1,
        PROGRAMA = 2,
        PROJETOGESTAO = 3,
        PROJETOPED = 4,
        INTERESSEPROJETOPED = 5,
        INICIOEXECUCAOPROJETO = 6,
        PRORROGAEXECUCAOPROJETO = 7,
        RELATORIOFINALPED = 8,
        RELATORIOFINALGESTAO = 9,
        RELATORIOAUDITORIAPED = 10,
        RELATORIOAUDITORIAGESTAO = 11
    }
}