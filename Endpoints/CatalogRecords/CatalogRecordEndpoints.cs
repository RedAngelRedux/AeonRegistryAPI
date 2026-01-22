using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

        privateGroup.MapGet("/{artifactId:int}", GetCatalogRecordsByArtifactHandler)
            .WithName(nameof(GetCatalogRecordsByArtifactHandler))
            .WithSummary("Get Private Catalog Record by Artifact ID")
            .WithDescription("Retrieves a private catalog record by its associated artifact's unique identifier.")
            .Produces<CatalogRecordResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        return route;
    }

    // handlers
    private static async Task<Results<Ok<List<CatalogRecordResponse>>, NotFound>> GetCatalogRecordsByArtifactHandler(
    [FromServices] ICatalogRecordService catalogRecordService,
    int artifactId,
    CancellationToken ct)
    {
        var catalogRecord = await catalogRecordService.GetCatalogRecordsByArtifactIdAsync(artifactId, ct);
        return (catalogRecord is null) ? TypedResults.NotFound() : TypedResults.Ok(catalogRecord);
    }
}
