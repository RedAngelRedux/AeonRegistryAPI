using AeonRegistryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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
            .WithTags("Sites - Public");

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

        publicGroup.MapGet("/{siteId:int}/artifacts", GetPublicArtifactsBySite)
            .WithName(nameof(GetPublicArtifactsBySite))
            .WithSummary("Get Public Artifacts By Site ID")
            .WithDescription("Returns a list of public artifacts associated with the specified site ID.")
            .Produces<List<PublicArtifactResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        #endregion PublicSiteGroup

        #region PrivateSiteGroup
        // define the privateGroup (for Swagger's benefit)
        var privateGroup = route.MapGroup("/api/private/sites")
            .RequireAuthorization()
            .WithSummary("Private Site Endpoints")
            .WithDescription("Endpoints that expose private site data.")
            .WithTags("Sites - Private");

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
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();

        privateGroup.MapDelete("/{id:int}", DeleteSite)
            .WithName(nameof(DeleteSite))
            .WithSummary("Delete a Site")
            .WithDescription("Deletes a site by its ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

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
        
    /// <summary>
    /// Retrieves a list of all public sites.
    /// </summary>
    /// <param name="service">The service used to access site data.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An HTTP 200 OK result containing a list of public site responses.</returns>
    private static async Task<Ok<List<PublicSiteResponse>>> GetAllPublicSites(
        ISiteService service,
        CancellationToken ct)
    {
        return TypedResults.Ok(await service.GetAllPublicSitesAsync(ct));
    }

    /// <summary>
    /// Retrieves a public site by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the public site to retrieve.</param>
    /// <param name="service">The service used to access site data.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A result containing an <see cref="Ok{T}"/> with the public site data if found; otherwise, a <see
    /// cref="NotFound"/> result.</returns>
    private static async Task<Results<Ok<PublicSiteResponse>, NotFound>> GetPublicSiteById(
        int id,
        ISiteService service,
        CancellationToken ct)
    {
        var site = await service.GetPublicSiteByIdAsync(id, ct);

        return (site is null) ? TypedResults.NotFound() : TypedResults.Ok(site);
    }

    /// <summary>
    /// Retrieves a list of all private sites and returns the result in an HTTP 200 OK response.
    /// </summary>
    /// <param name="service">The site service used to access private site data. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an HTTP 200 OK response with a list
    /// of private site responses.</returns>
    private static async Task<Ok<List<PrivateSiteResponse>>> GetAllPrivateSites(
        ISiteService service,
        CancellationToken ct)
    {
        return TypedResults.Ok(await service.GetAllPrivateSitesAsync(ct));
    }

    /// <summary>
    /// Retrieves a private site by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the private site to retrieve.</param>
    /// <param name="service">The service used to access site data.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A result containing an <see cref="Ok{T}"/> with the private site data if found; otherwise, a <see
    /// cref="NotFound"/> result.</returns>
    private static async Task<Results<Ok<PrivateSiteResponse>, NotFound>> GetPrivateSiteById(
        int id,
        ISiteService service,
        CancellationToken ct)
    {
        var site = await service.GetPrivateSiteByIdAsync(id, ct);

        return (site is null) ? TypedResults.NotFound() : TypedResults.Ok(site);
    }

    /// <summary>
    /// Retrieves the list of public artifacts associated with the specified site.
    /// </summary>
    /// <param name="siteId">The unique identifier of the site for which to retrieve public artifacts.</param>
    /// <param name="artifactService">The service used to access artifact data.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A result containing a list of public artifact responses if any are found; otherwise, a not found result.</returns>
    private static async Task<Results<Ok<List<PublicArtifactResponse>>, NotFound>> GetPublicArtifactsBySite(
        int siteId,
        IArtifactService artifactService,
        CancellationToken ct)
    {
        var artifacts = await artifactService.GetPublicArtifactsBySiteAsync(siteId, ct)!;
        if (artifacts is null || artifacts.Count <= 0)
            return TypedResults.NotFound();

        return TypedResults.Ok(artifacts);
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

    #region DeleteHandlers
    /* Delete (D) */

    private static async Task<Results<NoContent, NotFound>> DeleteSite(
        int id,
        ISiteService service,
        CancellationToken ct)
    {
        return (await service.DeleteSiteAsync(id, ct))
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }

    #endregion DeleteHandlers
}


