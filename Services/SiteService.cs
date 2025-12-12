using AeonRegistryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AeonRegistryAPI.Services;

public class SiteService(ApplicationDbContext db) : ISiteService
{
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
}
