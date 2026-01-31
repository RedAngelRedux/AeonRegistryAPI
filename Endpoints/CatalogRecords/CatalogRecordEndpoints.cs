using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AeonRegistryAPI.Endpoints.CatalogRecords;

public static class CatalogRecordEndpoints
{
    // Groups
    public static IEndpointRouteBuilder MapCatalogRecordEndpoints(this IEndpointRouteBuilder route)
    {
        // Implementation of CatalogRecord endpoints goes here
        var privateGroup = route.MapGroup("/api/private/catalogrecords")
            .WithTags("Catalog Records - Private")
            .RequireAuthorization();

        #region Create Routes

        privateGroup.MapPost("/", CreateCatalogRecordHandler)
            .WithName(nameof(CreateCatalogRecordHandler))
            .WithSummary("Create Private Catalog Record")
            .WithDescription("Creates a private catalog record associated with the specified artifact's unique identifier and submitted by the authenticated user.")
            .Produces<CatalogRecordResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        #endregion

        #region Read Routes

        privateGroup.MapGet("/artifact/{artifactId:int}", GetCatalogRecordsByArtifactHandler)
            .WithName(nameof(GetCatalogRecordsByArtifactHandler))
            .WithSummary("Get Private Catalog Record by Artifact ID")
            .WithDescription("Retrieves a private catalog record by its associated artifact's unique identifier.")
            .Produces<CatalogRecordResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapGet("/{Id:int}", GetCatalogRecordByIdHandler)
            .WithName(nameof(GetCatalogRecordByIdHandler))
            .WithSummary("Get Private Catalog Record by ID")
            .WithDescription("Retrieves a private catalog record by its unique identifier.")
            .Produces<CatalogRecordResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        #endregion

        #region Update Routes
        privateGroup.MapPut("/{Id:int}", UpdateCatalogRecordHandler)
            .WithName(nameof(UpdateCatalogRecordHandler))
            .WithSummary("Update Private Catalog Record")
            .WithDescription("Updates an existing private catalog record identified by its unique identifier.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        #endregion

        #region Delete Routes
        #endregion

        return route;
    }

    // handlers

    #region Create Operations
    private static async Task<Results<Ok<CatalogRecordResponse>, BadRequest>> CreateCatalogRecordHandler(
        [FromServices] ICatalogRecordService catalogRecordService,
        [FromBody] CreateCatalogRecordRequest request,
        HttpContext httpContext,
        CancellationToken ct
    )
    {
        // Get the user ID from the HttpContext
        // We can assume the user is Authenticated due to the RequireAuthorization on the route group
        string? submittedByUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(submittedByUserId))
        {
            return TypedResults.BadRequest();
        }

        var catalogRecord = await catalogRecordService.CreateCatalogRecordAsync(request, submittedByUserId, ct);
        return (catalogRecord is null) ? TypedResults.BadRequest() : TypedResults.Ok(catalogRecord);
    }
    #endregion

    #region Read Operations
    private static async Task<Results<Ok<List<CatalogRecordResponse>>, NotFound>> GetCatalogRecordsByArtifactHandler(
    [FromServices] ICatalogRecordService catalogRecordService,
    int artifactId,
    CancellationToken ct)
    {
        var catalogRecord = await catalogRecordService.GetCatalogRecordsByArtifactIdAsync(artifactId, ct);
        return (catalogRecord is null) ? TypedResults.NotFound() : TypedResults.Ok(catalogRecord);
    }

    private static async Task<Results<Ok<CatalogRecordResponse>, NotFound>> GetCatalogRecordByIdHandler(
    [FromServices] ICatalogRecordService catalogRecordService,
    [FromRoute] int Id,
    CancellationToken ct)
    {
        var catalogRecord = await catalogRecordService.GetCatalogRecordByIdAsync(Id, ct);
        return (catalogRecord is null) ? TypedResults.NotFound() : TypedResults.Ok(catalogRecord);
    }
    #endregion

    #region Update Operations
    private static async Task<Results<NoContent, NotFound>> UpdateCatalogRecordHandler(
        [FromServices] ICatalogRecordService catalogRecordService,
        [FromRoute] int Id,
        [FromBody] UpdateCatalogRecordRequest request,
        CancellationToken ct)
    {
        var success = await catalogRecordService.UpdateCatalogRecordAsync(Id, request, ct);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    #endregion

    #region Delete Operations
    private static async Task<Results<NoContent, NotFound>> DeleteCatalogRecordHandler(
        [FromServices] ICatalogRecordService catalogRecordService,
        [FromRoute] int Id,
        CancellationToken ct)
    {
        throw new NotImplementedException();
        //var success = await catalogRecordService.DeleteCatalogRecordAsync(Id, ct);
        //return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    #endregion
}