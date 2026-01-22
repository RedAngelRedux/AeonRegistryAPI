namespace AeonRegistryAPI.Services.Interfaces;

public interface ICatalogRecordService
{
    Task<List<CatalogRecordResponse>> GetCatalogRecordsByArtifactIdAsync(int artifactId, CancellationToken ct);
}