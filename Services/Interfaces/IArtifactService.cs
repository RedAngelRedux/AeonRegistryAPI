namespace AeonRegistryAPI.Services.Interfaces;

public interface IArtifactService
{
    #region Create
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct);
    #endregion

    #region Read
    /// <summary>
    /// Asynchronously retrieves a list of public artifacts.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of public artifact responses.
    /// The list is empty if no public artifacts are found.</returns>
    Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artifactId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artifactId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<PrivateArtifactResponse?> GetPrivateArtifactByIdAsync(int artifactId, CancellationToken ct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="siteId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<List<PrivateArtifactResponse>> GetPrivateArtifactsBySiteAsync(int siteId, CancellationToken ct);
    #endregion

    #region Update
    
    /// <summary>
    /// Asynchronously updates the specified artifact with new data.
    /// </summary>
    /// <param name="artifactId">The unique identifier of the artifact to update. Must be a valid, existing artifact ID.</param>
    /// <param name="request">An object containing the updated artifact data. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update was
    /// successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateArtifactAsync(int artifactId, UpdateArtifactRequest request, CancellationToken ct);
    #endregion

    #region Delete
    // The D in CRUD

    Task<bool> DeleteArtifactAsync(int artifactId, CancellationToken ct);
    #endregion
}
