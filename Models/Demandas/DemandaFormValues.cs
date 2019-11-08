using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace APIGestor.Models.Demandas
{

    public class DemandaFormValues
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string FormKey { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Data { get; set; }

        [NotMapped]
        public JObject Object { get { return this.ToJObject(); } }

        public List<DemandaFormFile> Files { get; set; }

        public JObject ToJObject()
        {
            if (this.Data != null)
            {
                return JsonConvert.DeserializeObject<JObject>(this.Data);
            }
            return JsonConvert.DeserializeObject<JObject>("{}");
        }
        public void SetValue(object Value)
        {
            this.Data = JsonConvert.SerializeObject(Value);
        }

    }

}