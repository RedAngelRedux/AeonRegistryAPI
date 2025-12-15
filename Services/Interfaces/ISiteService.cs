
namespace AeonRegistryAPI.Services.Interfaces;

public interface ISiteService
{    // CancellationToken for async operation is "free" with .NET

    /* POST Endpoints (C) */
    Task<PrivateSiteResponse> CreateSiteAsync(CreateSiteRequest request, CancellationToken ct);

    /* GET Endpoints (R) */
    Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct);  

    Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int id, CancellationToken ct);

    Task<List<PrivateSiteResponse>> GetAllPrivateSitesAsync(CancellationToken ct);

    Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int id, CancellationToken ct);

    /* PUT Endpoints (U) */

    /* DELETE Endpoints (D) */

}
