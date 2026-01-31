using System.Text.Json.Serialization;

namespace AeonRegistryAPI.Enums;

// This attribute ensures that the enum is serialized/deserialized as a string in JSON
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CatalogStatus
{
    Draft = 1,
    Verified = 2,
    Archived = 3
}
