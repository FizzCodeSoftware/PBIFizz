using Newtonsoft.Json;
using System.Globalization;

namespace FizzCode.PBIFizz;

public class DecimalJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(decimal)
            || objectType == typeof(decimal?)
            || objectType == typeof(double)
            || objectType == typeof(double?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if(value is double)
            writer.WriteRawValue(((double)value).ToString("F2", CultureInfo.InvariantCulture));
        if (value is decimal)
            writer.WriteRawValue(((decimal)value).ToString("F2", CultureInfo.InvariantCulture));
    }
}
