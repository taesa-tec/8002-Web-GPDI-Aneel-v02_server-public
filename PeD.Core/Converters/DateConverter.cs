using Newtonsoft.Json.Converters;

namespace PeD.Core.Converters
{
    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }

    public class DateXmlConverter : IsoDateTimeConverter
    {
        public DateXmlConverter()
        {
            DateTimeFormat = "yyyyMMdd";
        }
    }
}