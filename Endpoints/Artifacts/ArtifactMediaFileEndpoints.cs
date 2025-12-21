
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AeonRegistryAPI.Endpoints.Artifacts;

public static class ArtifactMediaFileEndpoints
{
    /* Groups and Mappings */

    /// <summary>
    /// Adds endpoint mappings for artifact media file image APIs to the specified route builder.
    /// </summary>
    /// <remarks>This method registers public API endpoints for accessing artifact image files, including a
    /// GET endpoint for retrieving the primary image by artifact media file ID. Endpoints are grouped under
    /// '/api/public/artifacts/images' and are tagged as 'Artifact Images'. An exception handling filter is applied to
    /// the group.</remarks>
    /// <param name="routes">The endpoint route builder to which the artifact media file endpoints will be added.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> instance provided in <paramref name="routes"/>, with artifact media
    /// file endpoints configured.</returns>
    public static IEndpointRouteBuilder MapArtifactMediaFileEndpoints(this IEndpointRouteBuilder routes)
    {
        var publicGroup = 
            routes.MapGroup("/api/public/artifacts/images")
            .WithTags("Artifact Images");

        publicGroup
            .MapGet("/{id:int}", GetArtifacImaget)
            .WithName(nameof(GetArtifacImaget))
            .WithSummary("Get Primary Artifact Image By ID")
            .WithDescription("Returns the primary image for the specified ArtifactMediaFile ID.")
            .Produces<FileContentHttpResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        return routes;
    }

    /* Handlers */

    /// <summary>
    /// Retrieves the image file content for a specified artifact media file identifier.
    /// </summary>
    /// <remarks>If the image is found, a 'Cache-Control' header is set to allow public caching for up to one
    /// day. The method does not return the artifact itself, but the associated media file content.</remarks>
    /// <param name="id">The unique identifier of the artifact media file to retrieve. This is not the artifact's identifier, but the
    /// identifier of the associated media file.</param>
    /// <param name="db">The database context used to access artifact media files.</param>
    /// <param name="response">The HTTP response object to which cache control headers will be added if the image is found.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A result containing the file content of the artifact image if found; otherwise, a not found result.</returns>
    private static async Task<Results<FileContentHttpResult, NotFound>> GetArtifacImaget(
        int id, // This is the ArtifactMediaFile.Id, not the Artifact.Id
        ApplicationDbContext db,
        HttpResponse response,
        CancellationToken cancellationToken)
    {
        var image = await db.ArtifactMediaFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(amf => amf.Id == id, cancellationToken);

        if (image == null || image?.Data == null || image.Data.Length == 0)
            return TypedResults.NotFound();

        response.Headers.CacheControl = "public,max-age=86400"; // Cache for 1 days

        return TypedResults.File(image.Data, image.ContentType);
    }
}
