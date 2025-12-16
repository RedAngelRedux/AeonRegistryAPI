namespace AeonRegistryAPI.Models.Request;

public record CreateSiteRequest(
    [Required, MaxLength(200)] 
    string? Name,

    [Required, MaxLength(100)] 
    string? Location,

    [MaxLength(100)] 
    string? Coordinates,

    double Latitude,

    double Longitude,

    [MaxLength(200)] 
    string? Description,

    [MaxLength(2000)] 
    string? PublicNarrative,

    [MaxLength(2000)] 
    string? AeonNarrative
); 