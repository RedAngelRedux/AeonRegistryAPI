namespace AeonRegistryAPI.Services.Interfaces;

public interface ICatalogRecordService
{   // CRUD Operations

    #region Create Operations
    Task<CatalogRecordResponse?> CreateCatalogRecordAsync(CreateCatalogRecordRequest request, string submittedById, CancellationToken ct);
    #endregion

    #region Read Operations
    Task<CatalogRecordResponse?> GetCatalogRecordByIdAsync(int catalogRecordId, CancellationToken ct);
    Task<List<CatalogRecordResponse>> GetCatalogRecordsByArtifactIdAsync(int artifactId, CancellationToken ct);
    #endregion

    #region Update Operations
    #endregion

    #region Delete Operations
    #endregion
}