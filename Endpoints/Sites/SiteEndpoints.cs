using AeonRegistryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Endpoints.Sites;

public static class SiteEndpoints
{
    // Endpoint Groups
    public static IEndpointRouteBuilder MapSiteEndpoints(this IEndpointRouteBuilder route)
    {
        // define the publicGrup (for Swagger's benefit)
        var publicGrup = route.MapGroup("/api/public/sites")
            .AllowAnonymous()
            .WithSummary("Public Site Endpoints")
            .WithDescription("Endpoints that expose public site data.")
            .WithTags("Sites - Public");

        // define the endpoints
        publicGrup.MapGet("", GetAllPublicSites)
            .WithName(nameof(GetAllPublicSites))
            .Produces<List<PublicSiteResponse>>(StatusCodes.Status200OK)
            .WithSummary("Get All Sites (Public)")
            .WithDescription("Returns all sites with their public data only.");

        return route;
    }

    // The Handlers
    private static async Task<Ok<List<PublicSiteResponse>>> GetAllPublicSites(ISiteService service,CancellationToken ct)
    {
        return TypedResults.Ok(await service.GetAllPublicSitesAsync(ct));
    }
}
   
