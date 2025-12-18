using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services;

public class SiteService(ApplicationDbContext db) : ISiteService
{

    /* POST Endpoints (C) */
    public async Task<PrivateSiteResponse> CreateSiteAsync(CreateSiteRequest request, CancellationToken ct)
    {
        // Initialize new Site entity with request data
        var site = new Site
        {
            Name = request.Name,
            Location = request.Location,
            Coordinates = request.Coordinates,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Description = request.Description,
            PublicNarrative = request.PublicNarrative,
            AeonNarrative = request.AeonNarrative
        };

        // Add to database and save changes
        db.Sites.Add(site);
        await db.SaveChangesAsync(ct);

        return new PrivateSiteResponse
        (
            site.Id,
            site.Name!,
            site.Location!,
            site.Coordinates,
            site.Latitude,
            site.Longitude,
            site.Description,
            site.PublicNarrative,
            site.AeonNarrative
        );
    }

    /* GET Endpoints (u) */
    public async Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct)
    {
        return await db.Sites
            .AsNoTracking()
            .Select(s => new PublicSiteResponse
            (
                s.Id,
                s.Name!,
                s.Location!,
                s.Coordinates,
                s.Latitude,
                s.Longitude,
                s.Description,
                s.PublicNarrative
            )).ToListAsync(cancellationToken: ct);
    }

    public async Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int id, CancellationToken ct)
    {
        return await db.Sites
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new PublicSiteResponse
            (
                s.Id,
                s.Name!,
                s.Location!,
                s.Coordinates,
                s.Latitude,
                s.Longitude,
                s.Description,
                s.PublicNarrative
            )).FirstOrDefaultAsync(cancellationToken: ct);
    }

    public async Task<List<PrivateSiteResponse>> GetAllPrivateSitesAsync(CancellationToken ct)
    {
        return await db.Sites
            .AsNoTracking()
            .Select(s => new PrivateSiteResponse
            (
                s.Id,
                s.Name!,
                s.Location!,
                s.Coordinates,
                s.Latitude,
                s.Longitude,
                s.Description,
                s.PublicNarrative,
                s.AeonNarrative
            )).ToListAsync(cancellationToken: ct);
    }

    public async Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int id, CancellationToken ct)
    {
        return await db.Sites
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new PrivateSiteResponse
            (
                s.Id,
                s.Name!,
                s.Location!,
                s.Coordinates,
                s.Latitude,
                s.Longitude,
                s.Description,
                s.PublicNarrative,
                s.AeonNarrative
            )).FirstOrDefaultAsync(cancellationToken: ct);
    }

    /* PUT Endpoints (U) */
    public async Task<bool> UpdateSiteAsync(int id, UpdateSiteRequest request, CancellationToken ct)
    {

        var existingSite = await db.Sites.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        if (existingSite == null) 
            return false;

        existingSite.Name = request.Name;
        existingSite.Location = request.Location;
        existingSite.Coordinates = request.Coordinates;
        existingSite.Latitude = request.Latitude;
        existingSite.Longitude = request.Longitude;
        existingSite.Description = request.Description;
        existingSite.PublicNarrative = request.PublicNarrative;
        existingSite.AeonNarrative = request.AeonNarrative;

        db.Sites.Update(existingSite);
        await db.SaveChangesAsync(ct);

        return true;
    }

    /* DELETE Endpoints (D) */
    public async Task<bool> DeleteSiteAsync(int id, CancellationToken ct)
    {
        var existingSite = await db.Sites.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        if (existingSite == null)
            return false;

        db.Sites.Remove(existingSite);
        await db.SaveChangesAsync(ct);

        return true;
    }



}
