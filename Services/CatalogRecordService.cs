
using AeonRegistryAPI.Services.Selectors;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services;

public class CatalogRecordService(
    ApplicationDbContext db) 
    : ICatalogRecordService
{
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
}