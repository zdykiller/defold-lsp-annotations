using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Converters
{
    public class BoolJsonConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True) return true;
            if (reader.TokenType == JsonTokenType.False) return false;

            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (bool.TryParse(s, out var b)) return b;
                if (int.TryParse(s, out var i)) return i != 0;
                return string.Equals(s, "0") ? false : throw new JsonException($"Cannot convert \"{s}\" to bool.");
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var n))
                return n != 0;

            throw new JsonException($"Unexpected token parsing bool: {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            => writer.WriteBooleanValue(value);
    }
}