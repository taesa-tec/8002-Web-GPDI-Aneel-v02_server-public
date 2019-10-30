using System;
using System.Linq;
using System.Collections.Generic;

namespace APIGestor.Models.Demandas
{

    public class DemandaFile
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
    }
}