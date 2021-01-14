using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Attributes;

namespace PeD.Models.Projetos {
    public class Etapa {

        [Key]
        public int Id { get; set; }

        [Logger]
        public string Nome { get; set; }
        public int ProjetoId { get; set; }
        [Logger("Duração")]
        public int Duracao { get; set; }
        [Logger("Descrição")]
        public string Desc { get; set; }
        [Logger("Atividades realizadas")]
        public string AtividadesRealizadas { get; set; }
        [Logger("Data de início")]
        [Column(TypeName = "date")]
        public DateTime? DataInicio { get; set; }
        [Logger("Data de final")]
        [Column(TypeName = "date")]
        public DateTime? DataFim { get; set; }
        public List<EtapaProduto> EtapaProdutos { get; set; }
        public List<EtapaMes> EtapaMeses { get; set; }

    }
    public class EtapaProduto {
        [Key]
        public int Id { get; set; }
        public int EtapaId { get; set; }
        public int? ProdutoId { get; set; }
    }
    public class EtapaMes {
        [Key]
        public int Id { get; set; }
        public int EtapaId { get; set; }
        public DateTime Mes { get; set; }
    }
}