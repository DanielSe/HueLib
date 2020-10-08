using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueLib.Converters
{
    internal class ObjectConstructorConverter : JsonConverterFactory
    {
        
        private readonly Bridge _bridge;

        public ObjectConstructorConverter(Bridge bridge)
        {
            _bridge = bridge;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(HueEntity));
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(ConverterImpl<>).MakeGenericType(typeToConvert);
            return (JsonConverter) Activator.CreateInstance(converterType, _bridge);
        }
            
        private class ConverterImpl<T> : JsonConverter<T>
        {
            private readonly Bridge _bridge;

            public ConverterImpl(Bridge bridge)
            {
                _bridge = bridge;
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var id = default(string);
                using var doc = JsonDocument.ParseValue(ref reader);
                var element = doc.RootElement;

                if (element.ValueKind != JsonValueKind.Object)
                    throw new InvalidOperationException("Must be object type");
                
                if (element.TryGetProperty("$__id", out var idProperty))
                {
                    id = idProperty.GetString();
                }

                var constructor = typeof(T)
                    .GetConstructors()
                    .OrderByDescending(x => x.GetParameters().Length)
                    .First();

                var parameters = constructor.GetParameters();
                var args = new List<object>();
                if (typeof(T).IsSubclassOf(typeof(BridgeEntity)))
                {
                    args.Add(_bridge);
                    args.Add(id);
                    parameters = parameters.Skip(2).ToArray();
                }

                foreach (var parameterInfo in parameters)
                {
                    if (element.TryGetProperty(parameterInfo.Name.ToLowerInvariant(), out var jsonProperty))
                    {
                        args.Add(JsonSerializer.Deserialize(jsonProperty.GetRawText(), parameterInfo.ParameterType, options));
                    }
                    else
                    {
                        args.Add(parameterInfo.ParameterType.IsValueType
                            ? Activator.CreateInstance(parameterInfo.ParameterType)
                            : null);
                    }
                }

                return (T) constructor.Invoke(args.ToArray());
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}