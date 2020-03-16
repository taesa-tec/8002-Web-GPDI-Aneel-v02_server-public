using System;

namespace APIGestor.Dtos
{
    public class DemandaFormHistoricoDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Content { get; set; }

        public int FormValuesId { get; set; }
    }
}