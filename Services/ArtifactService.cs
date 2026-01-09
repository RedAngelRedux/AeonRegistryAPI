
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AeonRegistryAPI.Services;

public class ArtifactService(
    ApplicationDbContext db) 
    : IArtifactService
{
    public async Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct)
    {
       return await db.Artifacts.AsNoTracking().Include(a => a.Site).Include(a => a.MediaFiles).Select(a => new PublicArtifactResponse
       {
           Id = a.Id,
           Name = a.Name,
           CatalogNumber = a.CatalogNumber,
           PublicNarrative = a.PublicNarrative,
           DateDiscovered = a.DateDiscovered,
           Type = a.Type,
           SiteId = a.SiteId,
           SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
           PrimaryImageurl = a.MediaFiles.Where(m => m.IsPrimary).Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
       }).ToListAsync(ct);
    }

    public async Task<List<PublicArtifactResponse>> GetPublicArtifactsBySiteAsync(int siteId, CancellationToken ct)
    {
        // Verify site exists
        var siteExists = db.Sites.AsNoTracking().Any(s => s.Id == siteId);
        if (!siteExists) 
            throw new KeyNotFoundException($"Site with ID {siteId} not found.");

        // Query artifacts for the site
        return await db.Artifacts
            .AsNoTracking()
            .Where(a => a.SiteId == siteId)
            .Include(a => a.Site)
            .Include(a => a.MediaFiles)
            .Select(a => new PublicArtifactResponse
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type,
                SiteId = a.SiteId,
                SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
                PrimaryImageurl = a.MediaFiles
                                    .Where(m => m.IsPrimary)
                                    .Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
            }).ToListAsync(ct);
    }

    public async Task<List<PrivateArtifactResponse>> GetPrivateArtifactsAsync(CancellationToken ct)
    {
        return await db.Artifacts.AsNoTracking().Include(a => a.Site).Include(a => a.MediaFiles).Select(a => new PrivateArtifactResponse
        {
            Id = a.Id,
            Name = a.Name,
            CatalogNumber = a.CatalogNumber,
            PublicNarrative = a.PublicNarrative,
            Descriiption = a.Description,
            DateDiscovered = a.DateDiscovered,
            Type = a.Type,
            SiteId = a.SiteId,
            SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
            PrimaryImageurl = a.MediaFiles.Where(m => m.IsPrimary).Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
        }).ToListAsync(ct);
    }

    public async Task<List<PrivateArtifactResponse>> GetPrivateArtifactsBySiteAsync(int siteId, CancellationToken ct)
    {
        // Verify site exists
        var siteExists = db.Sites.AsNoTracking().Any(s => s.Id == siteId);
        if (!siteExists)
            throw new KeyNotFoundException($"Site with ID {siteId} not found.");

        // Query artifacts for the site
        return await db.Artifacts
            .AsNoTracking()
            .Where(a => a.SiteId == siteId)
            .Include(a => a.Site)
            .Include(a => a.MediaFiles)
            .Select(a => new PrivateArtifactResponse
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                Descriiption = a.Description,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type,
                SiteId = a.SiteId,
                SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
                PrimaryImageurl = a.MediaFiles
                                    .Where(m => m.IsPrimary)
                                    .Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
            }).ToListAsync(ct);

    }

    public async Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct)
    {
        var site = await db.Sites.FindAsync([request.SiteId, ct], cancellationToken: ct);
        if (site == null)
            return null;

        if (!Enum.TryParse<ArtifactType>(request.Type,true,out var artifactType))
            throw new ArgumentException($"Invalid artifact type: {request.Type}");

        var artifact = new Artifact
        {
            Name = request.Name,
            CatalogNumber = request.CatalogNumber,
            Description = request.Description,
            PublicNarrative = request.PublicNarrative,
            Type = artifactType,
            SiteId = request.SiteId,
            DateDiscovered = request.DateDiscovered
        };

        db.Artifacts.Add(artifact);
        await db.SaveChangesAsync(ct);

        return new PrivateArtifactResponse
        {
            Id = artifact.Id,
            Name = artifact.Name,
            CatalogNumber = artifact.CatalogNumber,
            PublicNarrative = artifact.PublicNarrative,
            Descriiption = artifact.Description,
            DateDiscovered = artifact.DateDiscovered,
            Type = artifact.Type,
            SiteId = artifact.SiteId,
            SiteName = site.Name,
            PrimaryImageurl = string.Empty
        };
    }
}
