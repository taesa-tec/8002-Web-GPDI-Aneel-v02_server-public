using System;

namespace APIGestor.Models.Demandas
{
    public class DemandaFormHistorico
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Content { get; set; }

        public int FormValuesId { get; set; }
        public DemandaFormValues FormValues { get; set; }
    }
}