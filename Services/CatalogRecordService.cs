
using AeonRegistryAPI.Services.Selectors;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services;

public class CatalogRecordService(
    ApplicationDbContext db) 
    : ICatalogRecordService
{
    #region Create Operations
    public async Task<CatalogRecordResponse?> CreateCatalogRecordAsync(CreateCatalogRecordRequest request, string submittedById, CancellationToken ct)
    {
        // Validate the artifact exists
        var artifact = await db.Artifacts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.ArtifactId, ct);
        if (artifact == null) 
            return null;

        // Create the catalog record
        var catalogRecord = new CatalogRecord
        {
            ArtifactId = request.ArtifactId,
            SubmittedById = submittedById,
            Status = Enum.Parse<CatalogStatus>(request.Status),
            DateSubmitted = DateTime.UtcNow
        };
        db.CatalogRecords.Add(catalogRecord);
        await db.SaveChangesAsync(ct);

        return await db.CatalogRecords
            .AsNoTracking()
            .Where(cr => cr.Id == catalogRecord.Id)
            .Select(CatalogRecordSelectors.ToResponse)
            .FirstOrDefaultAsync(ct);
    }
    #endregion

    #region Read Operations
    public async Task<CatalogRecordResponse?> GetCatalogRecordByIdAsync(int catalogRecordId, CancellationToken ct)
    {
        return await db.CatalogRecords
            .AsNoTracking()
            .Where(cr => cr.Id == catalogRecordId)
            .Select(CatalogRecordSelectors.ToResponse)
            .FirstOrDefaultAsync(ct);        
    }

    public async Task<List<CatalogRecordResponse>> GetCatalogRecordsByArtifactIdAsync(int artifactId, CancellationToken ct)
    {
        // Confirm that the artifact exists
        var exists = await db.Artifacts
            .AsNoTracking()
            .AnyAsync(a => a.Id == artifactId, ct);

        if (!exists) 
            return [];
        else 
            return await  db.CatalogRecords
                .AsNoTracking()
                .Where(cr => cr.ArtifactId == artifactId)
                .Select(CatalogRecordSelectors.ToResponse)
                .ToListAsync(ct);
    }
    #endregion

    #region Update Operations
    #endregion

    #region Delete Operations
    #endregion
}