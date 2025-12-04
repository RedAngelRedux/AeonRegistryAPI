namespace AeonRegistryAPI.Models;

public class CatalogRecord
{
    public int Id { get; set; }

    [Required]
    public int ArtifactId { get; set; }

    public Artifact? Artifact { get; set; } = null;

    [Required]
    public string SubmittedById { get; set; } = string.Empty; // FK to ApplicationUser

    public ApplicationUser? SubmittedBy { get; set; } = null;

    public string VerifiedById { get; set; } = string.Empty;  // FK to ApplicationUser

    public ApplicationUser? VerifiedBy { get; set; } = null;

    [Required]
    public CatalogStatus Status { get; set; } // in DTO Enums.CatalogStatus.Draft.ToString();

    [Required]
    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

    public ICollection<CatalogNote> Notes { get; set; } = [];
}
