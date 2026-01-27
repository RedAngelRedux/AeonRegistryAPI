namespace AeonRegistryAPI.Services.Interfaces;

public interface ICatalogRecordService
{
    Task<CatalogRecordResponse?> GetCatalogRecordByIdAsync(int catalogRecordId, CancellationToken ct);
    Task<List<CatalogRecordResponse>> GetCatalogRecordsByArtifactIdAsync(int artifactId, CancellationToken ct);
}