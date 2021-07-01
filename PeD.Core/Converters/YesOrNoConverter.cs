using System;
using Newtonsoft.Json;

namespace PeD.Core.Converters
{
    public class YesOrNoConverter : JsonConverter<bool>
    {
        public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
        {
            writer.WriteValue(value ? "S" : "N");
        }

        public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return reader.ReadAsString() == "S";
        }
    }
}