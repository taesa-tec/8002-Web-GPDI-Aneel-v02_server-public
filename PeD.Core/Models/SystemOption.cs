using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PeD.Core.Models
{

    public class SystemOption
    {
        public int Id { get; set; }
        public string Key { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Value { get; set; }

        public void setValue(object Value)
        {
            this.Value = JsonConvert.SerializeObject(Value);
        }

        public T ToObject<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(this.Value);
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }

    }

}