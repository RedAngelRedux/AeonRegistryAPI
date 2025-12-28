using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Endpoints.Artifacts;

public static class ArtifactEndpoints
{
    // Groups
    public static IEndpointRouteBuilder MapArtifactEndpoints(this IEndpointRouteBuilder routes)
    {
        var publicGroup = routes.MapGroup("/api/public/artifacts")
            .WithTags("Artifacts - Public")
            .AllowAnonymous();

        publicGroup.MapGet("/", GetPublicArtifactsHandler)
            .WithName(nameof(GetPublicArtifactsHandler))
            .WithSummary("Get All Public Artifacts")
            .WithDescription("Retrieves a list of all artifacts with public fields and images..")
            .Produces<List<PublicArtifactResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        return publicGroup;
    }

    // Handlers
    private static async Task<Results<Ok<List<PublicArtifactResponse>>, NotFound>> GetPublicArtifactsHandler(
        IArtifactService artifactService,
        CancellationToken ct)
    {
        var artifacts = await artifactService.GetPublicArtifactsAsync(ct);
        if(artifacts is null || artifacts.Count <= 0)
            return TypedResults.NotFound();

        return TypedResults.Ok(artifacts);
    }
}
