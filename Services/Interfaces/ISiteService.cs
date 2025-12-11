namespace AeonRegistryAPI.Services.Interfaces;

public interface ISiteService
{
    // CancellationToken for async operation is "free" with .NET
    Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct);  
}
