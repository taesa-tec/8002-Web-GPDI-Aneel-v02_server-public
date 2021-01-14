using System;

namespace PeD.Dtos
{
    public class DemandaFormHistoricoListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public int Revisao { get; set; }
        public int FormValuesId { get; set; }
    }

    public class DemandaFormHistoricoDto : DemandaFormHistoricoListItemDto
    {
        public string Content { get; set; }
    }
}