using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models.Demandas
{

    public class DemandaFormValues
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public string FormKey { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Data { get; set; }

        public T ToObject<T>()
        {
            if (this.Data != null)
            {
                return JsonConvert.DeserializeObject<T>(this.Data);
            }
            return JsonConvert.DeserializeObject<T>("{}");
        }
        public void SetValue(object Value)
        {
            this.Data = JsonConvert.SerializeObject(Value);
        }

    }

}