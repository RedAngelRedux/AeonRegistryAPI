using System.Text.Json.Serialization;

namespace AeonRegistryAPI.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))] // This attribute ensures that the enum is serialized/deserialized as a string in JSON
public enum ResearchSpecialty
{
    Archaeology = 1,
    Anthropology = 2,
    Biology = 3,
    Chemistry = 4,
    Physics = 5,
    Engineering = 6,
    Medicine = 7,
    Psychology = 8,
    Sociology = 9,
    History = 10,
    Linguistics = 11,
    ArtHistory = 12,
    Theology = 13,
    Philosophy = 14,
    Geology = 15,
    Other = 99
}
