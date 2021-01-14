using System;
using System.Collections.Generic;
using System.Globalization;

namespace PeD.Models.Projetos.Xmls {
    public class XmlRelatorioFinalGestao {
        public PD_RelFinalBase PD_RelFinalBase { get; set; }
        public PD_Equipe PD_Equipe { get; set; }
        public PD_Etapas PD_Etapas { get; set; }
        public RFG_Recursos PD_Recursos { get; set; }
        public PD_ResultadosCP PD_ResultadosCP { get; set; }
        public PD_ResultadosPC PD_ResultadosPC { get; set; }
    }

    public class RFG_Atividades {
        public List<RFG_Atividade> Atividade { get; set; }
    }
    public class RFG_Atividade {
        public string TipoAtividade { get; set; }
        public string ResAtividade { get; set; }
        private string _custoAtividade { get; set; }
        public string CustoAtividade {
            get => decimal.Parse(_custoAtividade).ToString("N2");
            set => _custoAtividade = value;
        }
    }
    public class PD_ResultadosPC {
        public List<IdPC> IdPC { get; set; }
    }
    public class IdPC {
        public string TipoPC { get; set; }
        private string _confPubPC;
        public string ConfPubPC {
            get => _confPubPC == "True" ? "S" : "N";
            set => _confPubPC = value;
        }
        private DateTime _dataPC { get; set; }
        public string DataPC {
            get => _dataPC.ToString("MMyyyy", CultureInfo.InvariantCulture);
            set => _dataPC = Convert.ToDateTime(value);
        }
        public string NomePC { get; set; }
        public string LinkPC { get; set; }
        public string PaisPC { get; set; }
        public string CidadePC { get; set; }
        public string TituloPC { get; set; }
        public string ArquivoPDF { get; set; }
    }

    public class RFG_Recursos {
        public List<RFG_RecursoEmpresa> RecursoEmpresa { get; set; }
    }
    public class RFG_RecursoEmpresa {
        public string CodEmpresa { get; set; }
        public List<CustoCatContabil<ItemDespesaBase>> CustoCatContabil { get; set; }
    }
}