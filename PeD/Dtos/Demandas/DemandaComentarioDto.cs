using System;
using PeD.Models;

namespace PeD.Dtos.Demandas
{
    public class DemandaComentarioDto
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string UserId { get; set; }
        public string User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}