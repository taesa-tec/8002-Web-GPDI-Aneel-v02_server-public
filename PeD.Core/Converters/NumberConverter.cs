using System;
using System.Linq;
using Newtonsoft.Json;

namespace PeD.Core.Converters
{
    public class NumberConverter : JsonConverter
    {
        public string Format { get; set; }

        public NumberConverter(string format)
        {
            Format = format;
        }

        public NumberConverter() : this("{0:N1}")
        {
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null || !CanConvert(value.GetType()))
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(string.Format(Format, value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (!(existingValue is null) && CanConvert(objectType))
                return reader.ReadAsDecimal();
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return new[] {typeof(int), typeof(decimal), typeof(short), typeof(float)}.Any(t => t == objectType);
        }
    }
}