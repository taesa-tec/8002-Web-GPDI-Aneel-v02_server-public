using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PeD.Core.Models.Demandas
{
    public class DemandaFormValues
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string FormKey { get; set; }
        public int Revisao { get; set; }

        public DateTime LastUpdate { get; set; }

        [Column(TypeName = "varchar(max)")] public string Data { get; set; }

        private JObject _jObject;

        [NotMapped]
        public JObject Object
        {
            get
            {
                if (_jObject == null)
                {
                    _jObject = ToJObject();
                }

                return _jObject;
            }
        }

        public List<DemandaFormFile> Files { get; set; }

        public string Html { get; set; }

        public List<DemandaFormHistorico> Historico { get; set; }

        public JObject ToJObject()
        {
            if (Data != null)
            {
                return JsonConvert.DeserializeObject<JObject>(Data);
            }

            return JsonConvert.DeserializeObject<JObject>("{}");
        }

        public void SetValue(object Value)
        {
            Data = JsonConvert.SerializeObject(Value);
        }
    }
}