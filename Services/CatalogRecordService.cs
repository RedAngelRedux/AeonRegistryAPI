
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services;

public class CatalogRecordService(
    ApplicationDbContext db) 
    : ICatalogRecordService
{
    public async Task<List<CatalogRecordResponse>> GetCatalogRecordsByArtifactIdAsync(int artifactId, CancellationToken ct)
    {
        // Confirm that the artifact exists
        var exists = await db.Artifacts
            .AsNoTracking()
            .AnyAsync(a => a.Id == artifactId, ct);

        if (!exists)
            return [];

        return await db.CatalogRecords
            .AsNoTracking()
            .Where(cr => cr.ArtifactId == artifactId)
            .Select(cr => new CatalogRecordResponse
            {
                Id = cr.Id,
                ArtifactId = cr.ArtifactId,
                ArtifactName = cr.Artifact!.Name == null ? string.Empty : cr.Artifact.Name,
                SubmittedById = cr.SubmittedById,
                SubmittedByName = cr.SubmittedBy!.FullName == null ? string.Empty : cr.SubmittedBy.FullName,
                VerifiedById = cr.VerifiedById,
                VerifiedByName = cr.VerifiedBy!.FullName == null ? string.Empty : cr.VerifiedBy.FullName,
                Status = cr.Status.ToString(),                
                Notes = cr.Notes.Select(n => new CatalogNoteResponse
                {
                    Id = n.Id,
                    AuthorId = n.AuthorId,
                    AuthorName = n.Author!.FullName == null ? string.Empty : n.Author.FullName,
                    Content = n.Content,
                    DateSubmitted = n.DateSubmitted
                }).ToList()
            })            
            .ToListAsync(ct);
    }
}
