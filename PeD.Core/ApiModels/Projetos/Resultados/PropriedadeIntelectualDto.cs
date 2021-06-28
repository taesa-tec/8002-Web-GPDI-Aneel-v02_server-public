using System;
using System.Collections.Generic;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class PropriedadeIntelectualDto : ProjetoNodeDto
    {
        public string Tipo { get; set; }
        public DateTime PedidoData { get; set; }
        public string PedidoNumero { get; set; }
        public string TituloINPI { get; set; }
        public List<RecursoHumanoDto> Inventores { get; set; }
        public List<PropriedadeIntelectualDepositanteDto> Depositantes { get; set; }
    }
}