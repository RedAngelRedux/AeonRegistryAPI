namespace AeonRegistryAPI.Models.Request;

public record UpdateCatalogRecordRequest 
{
    public string? VerifiedById { get; init; }
    public CatalogStatus Status { get; set; }
};
