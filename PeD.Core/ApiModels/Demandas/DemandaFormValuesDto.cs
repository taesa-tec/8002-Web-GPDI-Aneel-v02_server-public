using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PeD.Core.Models.Demandas;

namespace PeD.Core.ApiModels.Demandas
{
    public class DemandaFormValuesDto
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string FormKey { get; set; }
        public int Revisao { get; set; }
        public DateTime LastUpdate { get; set; }
        public JObject Data { get; set; }
        public List<DemandaFormFileDto> Files { get; set; }
    }
}