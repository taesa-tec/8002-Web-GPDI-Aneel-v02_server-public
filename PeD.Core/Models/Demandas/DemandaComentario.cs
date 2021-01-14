using System;

namespace PeD.Core.Models.Demandas
{

    public class DemandaComentario
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}