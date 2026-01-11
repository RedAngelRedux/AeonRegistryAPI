namespace AeonRegistryAPI.Services.Interfaces;

public interface IArtifactService
{
    /// <summary>
    /// Asynchronously retrieves a list of public artifacts.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of public artifact responses.
    /// The list is empty if no public artifacts are found.</returns>
    Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct);

    Task<PublicArtifactResponse?> GetPublicArtifactByIdAsync(int artifactId, CancellationToken ct);
    

    /// <summary>
    /// Asynchronously retrieves a list of public artifacts associated with the specified site.
    /// </summary>
    /// <param name="siteId">The unique identifier of the site for which to retrieve public artifacts.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of public artifact responses
    /// for the specified site. The list may be empty if no public artifacts are found.  null will be returned
    /// if the site does not exist.</returns>
    Task<List<PublicArtifactResponse>> GetPublicArtifactsBySiteAsync(int siteId, CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves a list of private artifacts accessible to the current user.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of private artifact
    /// responses. The list is empty if no private artifacts are available.</returns>
    Task<List<PrivateArtifactResponse>> GetPrivateArtifactsAsync(CancellationToken ct);

    Task<PrivateArtifactResponse?> GetPrivateArtifactByIdAsync(int artifactId, CancellationToken ct);

    Task<List<PrivateArtifactResponse>> GetPrivateArtifactsBySiteAsync(int siteId, CancellationToken ct);

    Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct);
}
