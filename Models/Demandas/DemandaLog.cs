using System;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Exceptions.Demandas;
using System.ComponentModel.DataAnnotations;

namespace APIGestor.Models.Demandas
{

    public class DemandaLog : Log
    {
        public int DemandaId { get; set; }

        public Demanda Demanda { get; set; }
    }
}