using System.Net.NetworkInformation;

namespace AeonRegistryAPI.Models.Response;

public class CatalogRecordResponse
{
    private CatalogStatus _status;

    public int Id { get; set; }
    public int ArtifactId { get; set; }
    public string? ArtifactName { get; set; }
    public string? SubmittedById { get; set; }
    public string? SubmittedByName { get; set; }
    public string? VerifiedById { get; set; }
    public string? VerifiedByName { get; set; }
    public string? Status { get; set; }
    public DateTime DateSubmitted { get; set; }
    public ICollection<CatalogNoteResponse> Notes { get; set; } = [];
}
