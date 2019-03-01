using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace APIGestor.Models
{
    public class XmlRelatorioFinal
    {
        public PD_RelFinalBase PD_RelFinalBase { get; set; }
        public PD_EquipeEmp PD_EquipeEmp { get; set; }
        public PD_EquipeExec PD_EquipeExec { get; set; }
        public PD_Etapas PD_Etapas { get; set; }
        public RF_Recursos PD_Recursos { get; set; }
        public PD_Resultados PD_Resultados { get; set; }
    }
    public class PD_RelFinalBase
    {
        public string CodProjeto { get; set; }
        public string ArquivoPDF { get; set; }
        private DateTime _dataIniODS{ get; set; }
        public string DataIniODS{
            get => _dataIniODS.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            set => _dataIniODS = Convert.ToDateTime(value);
        }
        private DateTime _dataFimODS{ get; set; }
        public string DataFimODS{
            get => _dataFimODS.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            set => _dataFimODS = Convert.ToDateTime(value);
        }
        private string _prodPrev;
        public string ProdPrev
        {
            get => _prodPrev=="True" ? "S" : "N";
            set => _prodPrev = value;
        }
        public string ProdJust { get; set; }
        public string ProdEspTec { get; set; }
        private string _tecPrev;
        public string TecPrev
        {
            get => _tecPrev=="True" ? "S" : "N";
            set => _tecPrev = value;
        }
        public string TecJust { get; set; }
        public string TecDesc { get; set; }
        private string _aplicPrev;
        public string AplicPrev
        {
            get => _aplicPrev=="True" ? "S" : "N";
            set => _aplicPrev = value;
        }
        public string AplicJust { get; set; }
        public string AplicFnc { get; set; }
        public string AplicAbrang { get; set; }
        public string AplicAmbito { get; set; }
        public string TxDifTec { get; set; }
    }
    public class PD_EquipeEmp
    {
        public PedEmpresas Empresas { get; set; }
    }
    public class PD_EquipeExec
    {
        public PedExecutoras Executoras { get; set; }
    }
    
    public class PD_Etapas
    {
        public List<PD_Etapa> Etapa { get; set; }
    }
    public class PD_Etapa
    {
        private string _etapaN{ get; set; }
        public string EtapaN{
            get => string.Format("{0:D2}",_etapaN);
            set => _etapaN = value;
        }
        public string Atividades { get; set; }
        public string MesExecEtapa { get; set; }
    }
 
    public class PD_Resultados
    {
        public PD_ResultadosCP PD_ResultadosCP { get; set; }
        public PD_ResultadosCT PD_ResultadosCT { get; set; }
        public PD_ResultadosSA PD_ResultadosSA { get; set; }
        public PD_ResultadosIE PD_ResultadosIE { get; set; }
        
    }
    public class PD_ResultadosCP
    {
        public IdCP IdCP { get; set; }
    }
    public class IdCP
    {
        public string TipoCP { get; set; }
        private string _conclusaoCP;
        public string ConclusaoCP
        {
            get => _conclusaoCP=="True" ? "S" : "N";
            set => _conclusaoCP = value;
        }
        private DateTime _dataCP{ get; set; }
        public string DataCP{
            get => _dataCP.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            set => _dataCP = Convert.ToDateTime(value);
        }
        public string DocMmbEqCP { get; set; }
        private string _cnpjInstCP{ get; set; }
        public string CNPJInstCP{
            get => new Regex(@"[^\d]").Replace(_cnpjInstCP, "");
            set => _cnpjInstCP = value;
        }
        public string AreaCP { get; set; }
        public string TituloCP { get; set; }
        public string ArquivoPDF { get; set; }
    }
    public class PD_ResultadosCT
    {
        public PD_ResultadosCT_PC PD_ResultadosCT_PC { get; set; }
        public PD_ResultadosCT_IE PD_ResultadosCT_IE { get; set; }
        public PD_ResultadosCT_PI PD_ResultadosCT_PI { get; set; }
    }
    public class PD_ResultadosCT_PC
    {
        public List<IdCT_PC> IdCT_PC { get; set; }
    }
    public class IdCT_PC
    {
        public string TipoCT_PC { get; set; }
        private string _confPubCT_PC;
        public string ConfPubCT_PC
        {
            get => _confPubCT_PC=="True" ? "S" : "N";
            set => _confPubCT_PC = value;
        }
        private DateTime _dataCT_PC{ get; set; }
        public string DataCT_PC{
            get => _dataCT_PC.ToString("MMyyyy", CultureInfo.InvariantCulture);
            set => _dataCT_PC = Convert.ToDateTime(value);
        }
        public string NomeCT_PC { get; set; }
        public string LinkCT_PC { get; set; }
        public string PaisCT_PC { get; set; }
        public string CidadeCT_PC { get; set; }
        public string TituloCT_PC { get; set; }
        public string ArquivoPDF { get; set; }
    }
    public class PD_ResultadosCT_IE
    {
        public List<IdCT_IE> IdCT_IE { get; set; }
    }
    public class IdCT_IE
    {
        public string TipoCT_IE { get; set; }
        private string _cnpjInstBenefCT_IE{ get; set; }
        public string CNPJInstBenefCT_IE{
            get => new Regex(@"[^\d]").Replace(_cnpjInstBenefCT_IE, "");
            set => _cnpjInstBenefCT_IE = value;
        }
        public string NomeLabCT_IE { get; set; }
        public string AreaLabCT_IE { get; set; }
        public string ApoioLabCT_IE { get; set; }
    }
    public class PD_ResultadosCT_PI
    {
        public List<IdCT_PI> IdCT_PI { get; set; }
    }
    public class IdCT_PI
    {
        public string TipoCT_PI { get; set; }
        private DateTime _dataCT_PI{ get; set; }
        public string DataCT_PI{
            get => _dataCT_PI.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            set => _dataCT_PI = Convert.ToDateTime(value);
        }
        public string NumeroCT_PI { get; set; }
        public string TituloCT_PI { get; set; }
        public Inventores_PI Inventores_PI { get; set; }
        public Depositantes_PI Depositantes_PI { get; set; }
    }
    public class Inventores_PI
    {
        public List<Inventor_PI> Inventor { get; set; }
    }
    public class Inventor_PI
    {
        public string DocMbEqCT_PI { get; set; } 
    }
    public class Depositantes_PI
    {
        public List<Depositante_PI> Depositante { get; set; }
    }
    public class Depositante_PI
    {
        private string _cnpjInstCT_PI{ get; set; }
        public string CNPJInstCT_PI{
            get => new Regex(@"[^\d]").Replace(_cnpjInstCT_PI, "");
            set => _cnpjInstCT_PI = value;
        }
        public string PercInstCT_PI { get; set; } 
    }
    public class PD_ResultadosSA
    {
        public List<IdSA> IdSA { get; set; }
    }
    public class IdSA
    {
        public string TipoISA { get; set; }  
        private string _possibISA;
        public string PossibISA
        {
            get => _possibISA=="True" ? "S" : "N";
            set => _possibISA = value;
        }
        public string TxtISA { get; set; }
    }
    public class PD_ResultadosIE
    {
        public List<IdIE> IdIE { get; set; }
    }
    public class IdIE
    {
        public string TipoIE { get; set; }  
        public string TxtBenefIE { get; set; }
        public string UnidBenefIE { get; set; }
        private string _baseBenefIE{ get; set; }
        public string BaseBenefIE{
            get => string.Format("{0:N}",_baseBenefIE);
            set => _baseBenefIE = value;
        }
        public string PerBenefIE { get; set; }
        public string VlrBenefIE { get; set; }
    }
    public class RF_Recursos
    {
        public List<RF_RecursoEmpresa> RecursoEmpresa { get; set; }
        public List<RF_RecursoParceira> RecursoParceira { get; set; }
    }
    public class RF_RecursoEmpresa
    {
        public string CodEmpresa { get; set; }
        public DestRecursos DestRecursos { get; set; }
    }
    public class DestRecursos
    {
        public List<RF_DestRecursosExec> DestRecursosExec { get; set; }
        public List<RF_DestRecursosEmp> DestRecursosEmp { get; set; }
    }
    public class RF_DestRecursosExec
    {
        private string _cnpjExec{ get; set; }
        public string CNPJExec{
            get => new Regex(@"[^\d]").Replace(_cnpjExec, "");
            set => _cnpjExec = value;
        }
        public List<CustoCatContabil> CustoCatContabil { get; set; }
    }
    public class CustoCatContabil
    {
        public string CategoriaContabil { get; set; }
        public List<ItemDespesa> ItemDespesa { get; set; }
    }
    public class ItemDespesa
    {
        public string NomeItem { get; set; }
        public string JustificaItem { get; set; }
        public int? QtdeItem { get; set; }
        private string _valorIndItem{ get; set; }
        public string ValorIndItem{
            get => string.Format("{0:N}",_valorIndItem);
            set => _valorIndItem = value;
        }
        private string _tipoItem;
        public string TipoItem
        {
            get => _tipoItem=="True" ? "N" : "I";
            set => _tipoItem = value;
        }
        private string _itemLabE;
        public string ItemLabE
        {
            get => _itemLabE=="True" ? "S" : "N";
            set => _itemLabE = value;
        }
        private string _itemLabN;
        public string ItemLabN
        {
            get => _itemLabN=="True" ? "S" : "N";
            set => _itemLabN = value;
        }
    }
    public class RF_DestRecursosEmp
    {
        public List<CustoCatContabil> CustoCatContabil { get; set; }
    }
    public class RF_RecursoParceira
    {
        private string _cnpjParc{ get; set; }
        public string CNPJParc{
            get => new Regex(@"[^\d]").Replace(_cnpjParc, "");
            set => _cnpjParc = value;
        }
        public List<RF_DestRecursosExec> DestRecursosExec { get; set; }
    }
}