using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;

namespace PeD.Core.Models.Projetos.Resultados {
    public class ResultadoIntelectual {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Tipo", "TipoValor")]
        public TipoIntelectual Tipo { get; set; }
        [NotMapped]
        public string TipoValor {
            get => Enum.GetName(typeof(TipoIntelectual), Tipo);
        }
        [Logger("Data do pedido")]
        public DateTime DataPedido { get; set; }
        [Logger("Número do pedido")]
        public string NumeroPedido { get; set; }
        [Logger]
        public string Titulo { get; set; }
        [Logger]
        public List<ResultadoIntelectualInventor> Inventores { get; set; }
        [Logger]
        public List<ResultadoIntelectualDepositante> Depositantes { get; set; }
    }
    public enum TipoIntelectual {
        PI = 1, PU = 2, RS = 3, RD = 4
    }
    public class ResultadoIntelectualInventor {
        [Key]
        public int Id { get; set; }
        public int ResultadoIntelectualId { get; set; }
        [Logger("Inventor", "RecursoHumano.NomeCompleto")]
        public int RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
    }
    public class ResultadoIntelectualDepositante {
        [Key]
        public int Id { get; set; }
        public int ResultadoIntelectualId { get; set; }
        [Logger("Empresa Depositante", "Empresa.NomeEmpresa")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        [Logger("% Depositante")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Entidade { get; set; }
    }
}