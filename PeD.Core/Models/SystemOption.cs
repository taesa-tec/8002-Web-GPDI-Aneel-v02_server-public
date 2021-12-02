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

        public void SetValue(object value)
        {
            Value = JsonConvert.SerializeObject(value);
        }

        public T ToObject<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Value);
            }
            catch (System.Exception)
            {
                return default;
            }
        }

    }

}