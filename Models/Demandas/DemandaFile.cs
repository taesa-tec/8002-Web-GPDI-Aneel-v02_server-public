using System;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Exceptions.Demandas;

namespace APIGestor.Models.Demandas
{
    public class DemandaFile : FileUpload
    {
        public int DemandaId { get; set; }
        public Demanda Demanda { get; set; }
    }
}
