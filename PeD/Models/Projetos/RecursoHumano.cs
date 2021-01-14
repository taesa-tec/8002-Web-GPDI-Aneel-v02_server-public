using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Attributes;

namespace PeD.Models.Projetos {
    public class RecursoHumano {
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        [Logger("Empresa", "Empresa.NomeEmpresa")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [Logger("Custo Hora")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorHora { get; set; }
        [Logger("Nome Completo")]
        public string NomeCompleto { get; set; }
        [Logger("Titulo", "TitulacaoTexto")]
        public RecursoHumanoTitulacao Titulacao { get; set; }
        [NotMapped]
        public string TitulacaoValor { get => Enum.GetName(typeof(RecursoHumanoTitulacao), Titulacao); }
        [NotMapped]
        public string TitulacaoTexto {
            get {
                switch(Titulacao) {
                    case RecursoHumanoTitulacao.DO:
                        return "Doutor";
                    case RecursoHumanoTitulacao.ES:
                        return "Especialista";
                    case RecursoHumanoTitulacao.ME:
                        return "Mestre";
                    case RecursoHumanoTitulacao.SU:
                        return "Superior";
                    case RecursoHumanoTitulacao.TE:
                        return "Técnico";
                    default:
                        return "";
                }
            }
        }

        [Logger("Função", "FuncaoTexto")]
        public RecursoHumanoFuncao Funcao { get; set; }
        [NotMapped]
        public string FuncaoValor { get => Enum.GetName(typeof(RecursoHumanoFuncao), Funcao); }

        [NotMapped]
        public string FuncaoTexto {
            get {
                switch(Funcao) {
                    case RecursoHumanoFuncao.AA:
                        return "Auxiliar Administrativo";
                    case RecursoHumanoFuncao.AB:
                        return "Auxiliar Técnico Bolsista";
                    case RecursoHumanoFuncao.AT:
                        return "Auxiliar Técnico";
                    case RecursoHumanoFuncao.CO:
                        return "Coordenador";
                    case RecursoHumanoFuncao.GE:
                        return "Gerente";
                    case RecursoHumanoFuncao.PE:
                        return "Pesquisador";
                    default:
                        return "";
                }
            }
        }
        [Logger("Nacionalidade", "NacionalidadeValor")]
        public RecursoHumanoNacionalidade Nacionalidade { get; set; }
        [NotMapped]
        public string NacionalidadeValor { get => Enum.GetName(typeof(RecursoHumanoNacionalidade), Nacionalidade); }
        [Logger]
        public string CPF { get; set; }
        [Logger]
        public string Passaporte { get; set; }
        [Logger("Url do currículo")]
        public string UrlCurriculo { get; set; }

        [Logger("Gerente do Projeto")]
        public bool? GerenteProjeto { get; set; }
    }
    public enum RecursoHumanoTitulacao {
        DO, ME, ES, SU, TE
    }
    public enum RecursoHumanoFuncao {
        CO, GE, PE, AT, AB, AA
    }
    public enum RecursoHumanoNacionalidade {
        Brasileiro, Estrangeiro
    }

    public class AlocacaoRh {
        [Key]
        public int Id { get; set; }
        [Logger("Etapa", "Etapa.Desc")]
        public int? EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public int? ProjetoId { get; set; }
        [Logger("Recurso Humano", "RecursoHumano.NomeCompleto")]
        public int? RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        [Logger("Empresa", "Empresa.NomeEmpresa")]
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        [Logger("Horas por mês 1")]
        public int HrsMes1 { get; set; }
        [Logger("Horas por mês 2")]
        public int HrsMes2 { get; set; }
        [Logger("Horas por mês 3")]
        public int HrsMes3 { get; set; }
        [Logger("Horas por mês 4")]
        public int HrsMes4 { get; set; }
        [Logger("Horas por mês 5")]
        public int HrsMes5 { get; set; }
        [Logger("Horas por mês 6")]
        public int HrsMes6 { get; set; }
        [Logger("Horas por mês 7")]
        public int? HrsMes7 { get; set; }
        [Logger("Horas por mês 8")]
        public int? HrsMes8 { get; set; }
        [Logger("Horas por mês 9")]
        public int? HrsMes9 { get; set; }
        [Logger("Horas por mês 10")]
        public int? HrsMes10 { get; set; }
        [Logger("Horas por mês 11")]
        public int? HrsMes11 { get; set; }
        [Logger("Horas por mês 12")]
        public int? HrsMes12 { get; set; }
        [Logger("Horas por mês 13")]
        public int? HrsMes13 { get; set; }
        [Logger("Horas por mês 14")]
        public int? HrsMes14 { get; set; }
        [Logger("Horas por mês 15")]
        public int? HrsMes15 { get; set; }
        [Logger("Horas por mês 16")]
        public int? HrsMes16 { get; set; }
        [Logger("Horas por mês 17")]
        public int? HrsMes17 { get; set; }
        [Logger("Horas por mês 18")]
        public int? HrsMes18 { get; set; }
        [Logger("Horas por mês 19")]
        public int? HrsMes19 { get; set; }
        [Logger("Horas por mês 20")]
        public int? HrsMes20 { get; set; }
        [Logger("Horas por mês 21")]
        public int? HrsMes21 { get; set; }
        [Logger("Horas por mês 22")]
        public int? HrsMes22 { get; set; }
        [Logger("Horas por mês 23")]
        public int? HrsMes23 { get; set; }
        [Logger("Horas por mês 24")]
        public int? HrsMes24 { get; set; }
        [Logger("Justificativa")]
        public string Justificativa { get; set; }

        [NotMapped]
        public int HrsTotais {
            get {
                int t = 0;

                for(int i = 1; i <= 24; i++) {
                    var hrs = this.GetType().GetProperty("HrsMes" + i).GetValue(this);
                    t += hrs != null ? (int)hrs : 0;
                }

                return t;
            }
        }
    }
}