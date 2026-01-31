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
    Task<bool> UpdateCatalogRecordAsync(int catalogRecordId, UpdateCatalogRecordRequest request, CancellationToken ct);
    #endregion

    #region Delete Operations
    Task<bool> DeleteCatalogRecordAsync(int catalogRecordId, CancellationToken ct);
    #endregion
}