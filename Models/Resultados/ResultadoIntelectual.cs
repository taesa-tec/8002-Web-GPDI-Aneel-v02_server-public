using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ResultadoIntelectual
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get;set; }
        public TipoIntelectual Tipo { get; set; }
        [NotMapped]
        public string TipoValor { 
            get => Enum.GetName(typeof(TipoIntelectual),Tipo);
        }
        public DateTime DataPedido { get;set; }
        public string NumeroPedido { get;set; }
        public string Titulo { get; set; }
        public List<ResultadoIntelectualInventor> Inventores { get;set; }
        public List<ResultadoIntelectualDepositante> Depositantes { get;set; }
    }
    public enum TipoIntelectual
    {
        PI=1,PU=2,RS=3,RD=4 
    }
    public class ResultadoIntelectualInventor{
        [Key]
        public int Id { get; set; }
        public int ResultadoIntelectualId { get; set; }
        public int RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
    }
    public class ResultadoIntelectualDepositante{
        [Key]
        public int Id { get; set; }
        public int ResultadoIntelectualId { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Entidade { get; set; }
    }
}