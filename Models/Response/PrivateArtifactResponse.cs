namespace AeonRegistryAPI.Models.Response;

public class PrivateArtifactResponse
{
    // Entity Properties
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CatalogNumber { get; set; }
    public string? PublicNarrative { get; set; }
    public string? Description { get; set; }
    public DateTime DateDiscovered { get; set; }
    public ArtifactType Type { get; set; }
    public int SiteId { get; set; }

    // Additional Properties
    public string? SiteName { get; set; }
    public string? PrimaryImageurl { get; set; }

}
