using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace zobo.Configuration.Input
{
    class TargetDefinitionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TargetDefinition);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.None:
                    return TargetDefinition.None;
                case JsonToken.String:
                    return TargetDefinition.GetTargetDefinition(serializer.Deserialize<string>(reader));
                case JsonToken.StartArray:
                    return TargetDefinition.GetTargetDefinition(serializer.Deserialize<List<string>>(reader));
                case JsonToken.StartObject:
                    var unnamedWhitelists = serializer.Deserialize<Dictionary<string, TargetDefinition.TargetWhitelist>>(reader);
                    var namedWhitelists = unnamedWhitelists.ToList().ConvertAll(entry =>
                    {
                        return new TargetDefinition.TargetWhitelist(entry.Key, entry.Value?.AllowedPorts, entry.Value?.AllowedAddresses);
                    }).ToList();
                    return TargetDefinition.GetTargetDefinition(namedWhitelists);
                default:
                    throw new ArgumentException($"Unexpected json type {reader.TokenType} for SourceDefinition!");
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Not implemented yet");
        }
    }
}