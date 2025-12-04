using System.Text.Json.Serialization;

namespace AeonRegistryAPI.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))] // This attribute ensures that the enum is serialized/deserialized as a string in JSON
public enum CatalogStatus
{
    Draft = 1,
    Verified = 2,
    Archived = 3
}
