
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AeonRegistryAPI.Services;

public class ArtifactService(
    ApplicationDbContext db) 
    : IArtifactService
{

    #region Create
    // The C in CRUD
    public async Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct)
    {
        var site = await db.Sites.FindAsync([request.SiteId, ct], cancellationToken: ct);
        if (site == null)
            return null;

        if (!Enum.TryParse<ArtifactType>(request.Type, true, out var artifactType))
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
            Description = artifact.Description,
            DateDiscovered = artifact.DateDiscovered,
            Type = artifact.Type,
            SiteId = artifact.SiteId,
            SiteName = site.Name,
            PrimaryImageurl = string.Empty
        };
    }
    #endregion

    #region Read
    // The R in CRUD
    public async Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct)
    {
        return await db.Artifacts
             .AsNoTracking()
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
                 PrimaryImageurl = a.MediaFiles.Where(m => m.IsPrimary).Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
             }).ToListAsync(ct);
    }

    public async Task<PublicArtifactResponse?> GetPublicArtifactByIdAsync(int artifactId, CancellationToken ct)
    {
        return await db.Artifacts
            .AsNoTracking()
            .Where(a => a.Id == artifactId)
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
                PrimaryImageurl = a.MediaFiles.Where(m => m.IsPrimary).Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
            }).FirstOrDefaultAsync(ct);
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
        return await db.Artifacts
            .AsNoTracking()
            .Select(a => new PrivateArtifactResponse
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                Description = a.Description,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type,
                SiteId = a.SiteId,
                SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
                PrimaryImageurl = a.MediaFiles.Where(m => m.IsPrimary).Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
            })
            .ToListAsync(ct);
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
            .Select(a => new PrivateArtifactResponse
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                Description = a.Description,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type,
                SiteId = a.SiteId,
                SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
                PrimaryImageurl = a.MediaFiles
                                    .Where(m => m.IsPrimary)
                                    .Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
            }).ToListAsync(ct);

    }

    public async Task<PrivateArtifactResponse?> GetPrivateArtifactByIdAsync(int artifactId, CancellationToken ct)
    {
        return await db.Artifacts
            .AsNoTracking()
            .Where(a => a.Id == artifactId)
            .Select(a => new PrivateArtifactResponse
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                Description = a.Description,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type,
                SiteId = a.SiteId,
                SiteName = (a.Site != null) ? a.Site.Name : String.Empty,
                PrimaryImageurl = a.MediaFiles.Where(m => m.IsPrimary).Select(m => $"api/public/artifacts/images/{m.Id}").FirstOrDefault()
            }).FirstOrDefaultAsync(ct);
    }
    #endregion

    #region Update
    // The U in CRUD

    public async Task<bool> UpdateArtifactAsync(int artifactId, UpdateArtifactRequest request, CancellationToken ct)
    {
        // Validate Artifact type before checking database
        if (!Enum.TryParse<ArtifactType>(request.Type, true, out var artifactType))
            throw new ArgumentException($"Invalid artifact type: {request.Type}");

        // Validate site exists
        var siteExists = await db.Sites.AsNoTracking().AnyAsync(s => s.Id == request.SiteId, cancellationToken: ct);
        if (!siteExists) 
            return false;            

        // Retrieve artifact
        var artifact = await db.Artifacts.FindAsync([artifactId, ct], cancellationToken: ct);
        if (artifact == null)
            return false;

        // Apply Updates
        artifact.Name = request.Name;
        artifact.CatalogNumber = request.CatalogNumber;
        artifact.Description = request.Description;
        artifact.PublicNarrative = request.PublicNarrative;
        artifact.Type = artifactType;
        artifact.SiteId = request.SiteId;
        artifact.DateDiscovered = request.DateDiscovered;
        
        await db.SaveChangesAsync(ct);

        return true;
    }

    #endregion

    #region Delete
    // The D in CRUD

    public async Task<bool> DeleteArtifactAsync(int artifactId, CancellationToken ct)
    {
        // Validate Artifact exists
        var artifactExists = await db.Artifacts.AsNoTracking().AnyAsync(a => a.Id == artifactId, cancellationToken: ct);
        if (!artifactExists)
            return false;

        // Get the artifact and its dependencies
        var artifact = await db.Artifacts
            .Include(a => a.MediaFiles)
            //.Include(a => a.CatalogRecords)
            .FirstOrDefaultAsync(artifact => artifact.Id == artifactId, cancellationToken: ct);
        if (artifact == null)
            return false;

        // Remove dependent MediaFiles
        if(artifact.MediaFiles != null && artifact.MediaFiles.Count > 0)
        {
            db.ArtifactMediaFiles.RemoveRange(artifact.MediaFiles);
        }

        // TODO:  Decide how to handle CatalogRecords on Artifact deletion since we also have CatalogNotes linked to CatalogRecords
        //// Remove dependent CatalogRecords
        //if(artifact.CatalogRecords != null && artifact.CatalogRecords.Count > 0)
        //{
        //    db.CatalogRecords.RemoveRange(artifact.CatalogRecords);
        //}

        db.Artifacts.Remove(artifact);

        await db.SaveChangesAsync(ct);

        return true;
    }

    #endregion
}
