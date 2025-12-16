using AeonRegistryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using AeonRegistryAPI.Filters;
using AeonRegistryAPI.Models.Request;

namespace AeonRegistryAPI.Endpoints.Sites;

public static class SiteEndpoints
{
    // Endpoint Groups
    public static IEndpointRouteBuilder MapSiteEndpoints(this IEndpointRouteBuilder route)
    {
        #region PublicSiteGroup
        // define the publicGroup (for Swagger's benefit)
        var publicGroup = route.MapGroup("/api/public/sites")
            .AllowAnonymous()
            .WithSummary("Public Site Endpoints")
            .WithDescription("Endpoints that expose public site data.")
            .WithTags("Sites - Public")
            .AddEndpointFilter<ExceptionHandlingFilter>();

        // define the endpoints
        publicGroup.MapGet("", GetAllPublicSites)
            .WithName(nameof(GetAllPublicSites))
            .WithSummary("Get All Sites (Public)")
            .WithDescription("Returns all sites with their public data only.")
            .Produces<List<PublicSiteResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

        publicGroup.MapGet("/{id:int}", GetPublicSiteById)
            .WithName(nameof(GetPublicSiteById))
            .WithSummary("Get Site By ID (Public)")
            .WithDescription("Returns a site by its ID with public data only.")
            .Produces<PublicSiteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        #endregion PublicSiteGroup

        #region PrivateSiteGroup
        // define the privateGroup (for Swagger's benefit)
        var privateGroup = route.MapGroup("/api/private/sites")
            .RequireAuthorization()
            .WithSummary("Private Site Endpoints")
            .WithDescription("Endpoints that expose private site data.")
            .WithTags("Sites - Private")
            .AddEndpointFilter<ExceptionHandlingFilter>();

        privateGroup.MapPost("", CreateSite)
            .WithName(nameof(CreateSite))
            .WithSummary("Create a New Site")
            .WithDescription("Creates a new site with the provided data and returns the new site's details.")
            .Accepts<CreateSiteRequest>("application/json")
            .Produces<PrivateSiteResponse>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapGet("", GetAllPrivateSites)
            .WithName(nameof(GetAllPrivateSites))
            .WithSummary("Private Site Endpoints")
            .WithDescription("Endpoints that expose private site data.")
            .WithTags("Sites - Private")
            .Produces<List<PrivateSiteResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapGet("/{id:int}", GetPrivateSiteById)
            .WithName(nameof(GetPrivateSiteById))
            .WithSummary("Get Site By ID (Private)")
            .WithDescription("Returns a site by its ID with Private data only.")
            .Produces<PrivateSiteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapPut("/{id:int}", UpdateSite)
            .WithName(nameof(UpdateSite))
            .WithSummary("Update an Existing Site")
            .WithDescription("Updates an existing site with the provided data.")
            .Accepts<UpdateSiteRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();

        #endregion PrivateSiteGroup

        return route;
    }

    /* The Handlers */

    #region PostHandlers
    /* Create (C) */

    private static async Task<Results<Created<PrivateSiteResponse>, ValidationProblem>> CreateSite(
     CreateSiteRequest request,
     ISiteService service,
     CancellationToken ct)
    {
        if (request is null)
        {
            var value = new[] { "Request is required" };
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Request", value }
            });
        }

        var created = await service.CreateSiteAsync(request, ct);

        return TypedResults.Created($"/api/private/sites/{created.Id}", created);
    }
    #endregion PostHandlers

    #region GetHandlers
    /* Read (R) */

    private static async Task<Ok<List<PublicSiteResponse>>> GetAllPublicSites(
        ISiteService service,
        CancellationToken ct)
    {
        return TypedResults.Ok(await service.GetAllPublicSitesAsync(ct));
    }

    private static async Task<Results<Ok<PublicSiteResponse>, NotFound>> GetPublicSiteById(
        int id,
        ISiteService service,
        CancellationToken ct)
    {
        var site = await service.GetPublicSiteByIdAsync(id, ct);

        return (site is null) ? TypedResults.NotFound() : TypedResults.Ok(site);
    }

    private static async Task<Ok<List<PrivateSiteResponse>>> GetAllPrivateSites(
        ISiteService service,
        CancellationToken ct)
    {
        return TypedResults.Ok(await service.GetAllPrivateSitesAsync(ct));
    }

    private static async Task<Results<Ok<PrivateSiteResponse>, NotFound>> GetPrivateSiteById(
        int id,
        ISiteService service,
        CancellationToken ct)
    {
        var site = await service.GetPrivateSiteByIdAsync(id, ct);

        return (site is null) ? TypedResults.NotFound() : TypedResults.Ok(site);
    }
    #endregion GetHandlers

    #region PutHandlers
    /* Update (U) */

    private static async Task<Results<NoContent, NotFound>> UpdateSite(
        int id,
        UpdateSiteRequest request,
        ISiteService service,
        CancellationToken ct)
    {
        return (await service.UpdateSiteAsync(id, request, ct)) 
            ? TypedResults.NoContent() 
            : TypedResults.NotFound();
    }

    #endregion PutHandlers

}


