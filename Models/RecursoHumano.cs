

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class RecursoHumano
    {   
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorHora { get; set; }
        public string NomeCompleto { get; set;}
        public RecursoHumanoTitulacao Titulacao { get; set;}
        public RecursoHumanoFuncao Funcao { get; set;}
        public RecursoHumanoNacionalidade Nacionalidade { get; set;}
        public string CPF { get; set; }
        public string Passaporte { get; set; }
        public string UrlCurriculo { get; set; }
    }
    public enum RecursoHumanoTitulacao
    {   
        DO, ME, ES, SU,TE
    }
    public enum RecursoHumanoFuncao
    {   
        CO, GE, PE, AT, AB, AA
    }
    public enum RecursoHumanoNacionalidade
    {   
        Brasileiro, Estrangeiro
    }

    public class AlocacaoRh
    {
        [Key]
        public int Id { get; set; }
        public int? EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public int? ProjetoId { get; set; }
        public int? RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public int HrsMes1 { get; set;}
        public int HrsMes2 { get; set;}
        public int HrsMes3 { get; set;}
        public int HrsMes4 { get; set;}
        public int HrsMes5 { get; set;}
        public int HrsMes6 { get; set;}
        public string Justificativa { get; set;}
    }
}