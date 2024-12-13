using System.Text.Json;
using System.Text.Json.Serialization;
using NeerCore.Exceptions;

namespace Fleet.Api.Core;

public class CustomJsonStringEnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Handle both enum and nullable enum types
        return typeToConvert.IsEnum || (Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new EnumConverter(typeToConvert);
    }

    private class EnumConverter : JsonConverter<object>
    {
        private readonly string[] _enumNames;
        private readonly Array _enumValues;

        public EnumConverter(Type enumType)
        {
            enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            _enumNames = Enum.GetNames(enumType);
            _enumValues = Enum.GetValues(enumType);
        }

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var enumTypeName = enumType.Name;

            if (reader.TokenType == JsonTokenType.Null)
            {
                if (Nullable.GetUnderlyingType(typeToConvert) != null)
                {
                    // Return null for nullable enum
                    return null;
                }

                throw new ValidationFailedException($"Enum value is required for '{enumTypeName}'");
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new ValidationFailedException($"Unexpected token parsing enum '{enumTypeName}'. Expected String, got {reader.TokenType}");
            }

            var enumText = reader.GetString();
            for (var i = 0; i < _enumNames.Length; i++)
            {
                if (string.Equals(_enumNames[i], enumText, StringComparison.OrdinalIgnoreCase))
                {
                    return _enumValues.GetValue(i);
                }
            }

            throw new ValidationFailedException($"Invalid value '{enumText}' for '{enumTypeName}'");
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}