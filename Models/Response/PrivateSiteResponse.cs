namespace AeonRegistryAPI.Models.Response;

public record PrivateSiteResponse
(
    int Id,
    string Name,
    string Location,
    string? Coordinates,
    double Latitude,
    double Longitude,
    string? Description,
    string? PublicNarrative,
    string? AeonNarrative
);
