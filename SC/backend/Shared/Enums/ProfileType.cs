using System.Text.Json.Serialization;

namespace backend.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProfileType
{
    Student = 0,
    Company = 1
}