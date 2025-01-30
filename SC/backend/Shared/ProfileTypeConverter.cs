using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Shared.Enums;

public class ProfileTypeConverter : JsonConverter<ProfileType>
{
    public override ProfileType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)  // Handle 0, 1
        {
            return (ProfileType)reader.GetInt32();
        }
        else if (reader.TokenType == JsonTokenType.String)  // Handle "Student", "Company"
        {
            if (Enum.TryParse<ProfileType>(reader.GetString(), true, out var result))
                return result;
        }
        throw new JsonException($"Invalid ProfileType value: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, ProfileType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());  // Always serialize as a string
    }
}