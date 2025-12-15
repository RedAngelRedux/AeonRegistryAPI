namespace AeonRegistryAPI.Services.Interfaces;

public interface ISiteService
{
    // CancellationToken for async operation is "free" with .NET
    Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct);  

    Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int id, CancellationToken ct);

    Task<List<PrivateSiteResponse>> GetAllPrivateSitesAsync(CancellationToken ct);
}
