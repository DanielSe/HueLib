using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueLib.Converters
{
    internal class DictionaryToArrayConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsArray)
                return false;

            var et = typeToConvert.GetElementType();
            if (et == null || !et.IsSubclassOf(typeof(HueEntity)))
                return false;

            return true;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(DictionaryConverter<>).MakeGenericType(typeToConvert.GetElementType());
            return (JsonConverter) Activator.CreateInstance(converterType);
        }

        private class DictionaryConverter<T> : JsonConverter<T[]>
        {
            public override T[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                var items = doc.RootElement.EnumerateObject()
                    .Select(prop => JsonSerializer.Deserialize<T>(InjectId(prop), options))
                    .ToArray();
                return items;
            }

            private static string InjectId(JsonProperty property)
            {
                var id = property.Name;
                var raw = property.Value.GetRawText();
                
                var sb = new StringBuilder();
                sb.Append("{\"$__id\":\"");
                sb.Append(id);
                sb.Append("\",");
                sb.Append(raw.Substring(1));
                return sb.ToString();
            }

            public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}