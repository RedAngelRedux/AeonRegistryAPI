using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Endpoints.Artifacts;

public static class ArtifactEndpoints
{
    // Groups
    public static IEndpointRouteBuilder MapArtifactEndpoints(this IEndpointRouteBuilder route)
    {
        var publicGroup = route.MapGroup("/api/public/artifacts")
            .WithTags("Artifacts - Public")
            .AllowAnonymous();

        publicGroup.MapGet("/", GetPublicArtifactsHandler)
            .WithName(nameof(GetPublicArtifactsHandler))
            .WithSummary("Get All Public Artifacts")
            .WithDescription("Retrieves a list of all artifacts with public fields and images..")
            .Produces<List<PublicArtifactResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        var privateGroup = route.MapGroup("/api/private/artifacts")
            .WithTags("Artifacts - Private")
            .RequireAuthorization();

        privateGroup.MapGet("/", GetPrivateArtifactsHandler)
            .WithName(nameof(GetPrivateArtifactsHandler))
            .WithSummary("Get All Private Artifacts")
            .WithDescription("Retrieves a list of all artifacts with private fields and images.")
            .Produces<List<PrivateArtifactResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapPost("", CreateArtifactHandler)
            .WithName(nameof(CreateArtifactHandler))
            .WithSummary("Create an Artifact for a Site")
            .WithDescription("Creates a new Artifact, including private fields, for a Site.")
            .Produces<PrivateArtifactResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);            

        return route;
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

    private static async Task<Results<Ok<List<PrivateArtifactResponse>>, NotFound>> GetPrivateArtifactsHandler(
        IArtifactService artifactService,
        CancellationToken ct)
    {
        var artifacts = await artifactService.GetPrivateArtifactsAsync(ct);
        if(artifacts is null || artifacts.Count <= 0)
            return TypedResults.NotFound();
        return TypedResults.Ok(artifacts);
    }

    private static async Task<Results<Created<PrivateArtifactResponse>,NotFound>> CreateArtifactHandler(
        IArtifactService artifactService,
        CreateArtifactRequest request,
        CancellationToken ct)
    {
        var createdArtifact = await artifactService.CreateArtifactAsync(request, ct);
        if (createdArtifact is null)
            return TypedResults.NotFound();

        return TypedResults.Created($"/api/private/artifacts/{createdArtifact.Id}", createdArtifact);
    }
}
