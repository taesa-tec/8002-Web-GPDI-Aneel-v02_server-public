using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PeD.Core.Converters
{
    public class OnlyDigitsConverter : JsonConverter<string>
    {
        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            writer.WriteValue(Regex.Replace(value, @"\D", ""));
        }

        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return Regex.Replace(reader.ReadAsString() ?? "", @"\D", "");
        }
    }
}