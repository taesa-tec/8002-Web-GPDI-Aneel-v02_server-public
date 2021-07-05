using System;
using Newtonsoft.Json;

namespace PeD.Core.Converters
{
    public class YesOrNoConverter : JsonConverter<bool>
    {
        protected string Yes, No;

        public YesOrNoConverter() : this("S", "N")
        {
        }

        public YesOrNoConverter(string yes, string no)
        {
            Yes = yes;
            No = no;
        }

        public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
        {
            writer.WriteValue(value ? Yes : No);
        }

        public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return reader.ReadAsString() == Yes;
        }
    }
}