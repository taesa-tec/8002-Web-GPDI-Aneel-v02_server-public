using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaesaCore.Models;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class PropriedadeIntelectual : ProjetoNode
    {
        public enum TipoPropriedade
        {
            PI,
            PU,
            RS,
            RD
        }

        public TipoPropriedade Tipo { get; set; }
        public DateTime PedidoData { get; set; }
        [MaxLength(15)] public string PedidoNumero { get; set; }
        [MaxLength(200)] public string TituloINPI { get; set; }
        public List<PropriedadeIntelectualInventor> Inventores { get; set; }
        public List<PropriedadeIntelectualDepositante> Depositantes { get; set; }
    }

    public class PropriedadeIntelectualInventor
    {
        public int PropriedadeId { get; set; }
        public PropriedadeIntelectual Propriedade { get; set; }

        public RecursoHumano Recurso { get; set; }
        public int RecursoId { get; set; }
    }

    public class PropriedadeIntelectualDepositante : BaseEntity
    {
        public int PropriedadeId { get; set; }
        public PropriedadeIntelectual Propriedade { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [Column(TypeName = "decimal(5,2)")] public decimal Porcentagem { get; set; }
    }
}