using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class PropriedadeIntelectualRequest : BaseEntity
    {
        public string Tipo { get; set; }
        public DateTime PedidoData { get; set; }
        [MaxLength(15)] public string PedidoNumero { get; set; }
        [MaxLength(200)] public string TituloINPI { get; set; }
        public List<int> Inventores { get; set; }
        public List<PropriedadeIntelectualDepositanteRequest> Depositantes { get; set; }
    }
}