using System;
using System.Linq;
using System.Collections.Generic;

namespace APIGestor.Models.Demandas
{

    public class DemandaFormFile
    {
        public int Id { get; set; }
        public int DemandaFormId { get; set; }
        public int FileId { get; set; }
        public DemandaFile File { get; set; }
    }
}