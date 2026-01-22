using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Endpoints.Artifacts;

public static class ArtifactEndpoints
{
    // Groups
    public static IEndpointRouteBuilder MapArtifactEndpoints(this IEndpointRouteBuilder route)
    {
        #region public group
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

        publicGroup.MapGet("/{artifactId:int}", GetPublicArtifactByIdHandler)
            .WithName(nameof(GetPublicArtifactByIdHandler))
            .WithSummary("Get Public Artifact by ID")
            .WithDescription("Retrieves a public artifact by its unique identifier.")
            .Produces<PublicArtifactResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        #endregion

        #region private group
        var privateGroup = route.MapGroup("/api/private/artifacts")
            .WithTags("Artifacts - Private")
            .RequireAuthorization();

        privateGroup.MapPost("", CreateArtifactHandler)
            .WithName(nameof(CreateArtifactHandler))
            .WithSummary("Create an Artifact for a Site")
            .WithDescription("Creates a new Artifact, including private fields, for a Site.")
            .Produces<PrivateArtifactResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapGet("/", GetPrivateArtifactsHandler)
            .WithName(nameof(GetPrivateArtifactsHandler))
            .WithSummary("Get All Private Artifacts")
            .WithDescription("Retrieves a list of all artifacts with private fields and images.")
            .Produces<List<PrivateArtifactResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        
        privateGroup.MapGet("/{artifactId:int}", GetPrivateArtifactByIdHandler)
            .WithName(nameof(GetPrivateArtifactByIdHandler))
            .WithSummary("Get Private Artifact by ID")
            .WithDescription("Retrieves a private artifact by its unique identifier.")
            .Produces<PrivateArtifactResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapPut("/{artifactId:int}", UpdateArtifactHandler)
            .WithName(nameof(UpdateArtifactHandler))
            .WithSummary("Update an Artifact by ID")
            .WithDescription("Updates an existing Artifact's details by its unique identifier.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapDelete("/{artifactId:int}", DeleteArtifactHandler)
            .WithName(nameof(DeleteArtifactHandler))
            .WithSummary("Delete an Artifact by ID")
            .WithDescription("Deletes an existing Artifact by its unique identifier.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        #endregion

        return route;
    }

    // Handlers

    #region public handlers
    private static async Task<Results<Ok<List<PublicArtifactResponse>>, NotFound>> GetPublicArtifactsHandler(
        IArtifactService artifactService,
        CancellationToken ct)
    {
        var artifacts = await artifactService.GetPublicArtifactsAsync(ct);
        if(artifacts is null || artifacts.Count <= 0)
            return TypedResults.NotFound();

        return TypedResults.Ok(artifacts);
    }

    private static async Task<Results<Ok<PublicArtifactResponse>, NotFound>> GetPublicArtifactByIdHandler(
        IArtifactService artifactService,
        int artifactId,
        CancellationToken ct)
    {
        var artifact = await artifactService.GetPublicArtifactByIdAsync(artifactId, ct);
        
        return (artifact is null) ? TypedResults.NotFound() : TypedResults.Ok(artifact);
    }
    #endregion

    #region private handlers
    private static async Task<Results<Created<PrivateArtifactResponse>, NotFound>> CreateArtifactHandler(
    IArtifactService artifactService,
    CreateArtifactRequest request,
    CancellationToken ct)
    {
        var createdArtifact = await artifactService.CreateArtifactAsync(request, ct);
        if (createdArtifact is null)
            return TypedResults.NotFound();

        return TypedResults.Created($"/api/private/artifacts/{createdArtifact.Id}", createdArtifact);
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

    private static async Task<Results<Ok<PrivateArtifactResponse>, NotFound>> GetPrivateArtifactByIdHandler(
        IArtifactService artifactService,
        int artifactId,
        CancellationToken ct)
    {
        var artifact = await artifactService.GetPrivateArtifactByIdAsync(artifactId, ct);        
        return (artifact is null) ? TypedResults.NotFound() : TypedResults.Ok(artifact);
    }

    private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateArtifactHandler(
        IArtifactService artifactService,
        int artifactId,
        UpdateArtifactRequest request,
        CancellationToken ct)
    {
        var updated = await artifactService.UpdateArtifactAsync(artifactId, request, ct);
        return (updated) ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteArtifactHandler(
        IArtifactService artifactService,
        int artifactId,
        CancellationToken ct)
    {
        var deleted = await artifactService.DeleteArtifactAsync(artifactId, ct);
        return (deleted) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    #endregion
}
