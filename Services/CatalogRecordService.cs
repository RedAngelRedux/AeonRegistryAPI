
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
    public async Task<bool> UpdateCatalogRecordAsync(int catalogRecordId, UpdateCatalogRecordRequest request, CancellationToken ct)
    {
        // Retrieve the catalog record
        CatalogRecord? catalogRecord = await db.CatalogRecords
            .FirstOrDefaultAsync(cr => cr.Id == catalogRecordId, ct);

        if (catalogRecord == null)
            return false;

        // Update the catalog record
        catalogRecord.Status = request.Status;
        catalogRecord.VerifiedById = (string.IsNullOrWhiteSpace(request.VerifiedById) ? null : request.VerifiedById);

        // Validate VerifiedById if set
        if (catalogRecord.VerifiedById != null)
        {
            var userExists = await db.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == catalogRecord.VerifiedById, ct);
            if (!userExists)
                return false;
        }

        // Save changes
        await db.SaveChangesAsync(ct);

        // return success status
        return true;
    }
    #endregion

    #region Delete Operations
    public async Task<bool> DeleteCatalogRecordAsync(int catalogRecordId, CancellationToken ct)
    {
        var catalogRecord = await db.CatalogRecords.FindAsync([catalogRecordId], ct);
        if (catalogRecord == null)
            return false;

        db.CatalogRecords.Remove(catalogRecord);
        await db.SaveChangesAsync(ct);
        return true;
    }

    #endregion
}