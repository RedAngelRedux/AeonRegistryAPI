namespace AeonRegistryAPI.Models;

public class CatalogRecord
{
    private CatalogStatus _status;

    public int Id { get; set; }

    [Required]
    public int ArtifactId { get; set; }

    public Artifact? Artifact { get; set; } = null;

    [Required]
    public string SubmittedById { get; set; } = string.Empty; // FK to ApplicationUser

    public ApplicationUser? SubmittedBy { get; set; } = null;

    public string? VerifiedById { get; set; } = string.Empty;  // FK to ApplicationUser

    public ApplicationUser? VerifiedBy { get; set; } = null;

    [Required]
    public CatalogStatus Status { get => _status; set => _status = value; }

    public string StatusString => Status.ToString();

    [Required]
    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

    public ICollection<CatalogNote> Notes { get; set; } = [];

    /// <summary>
    /// Attempts to set the Status from a string value with case-insensitive matching.
    /// </summary>
    /// <param name="statusName">The status string (e.g., "Draft", "Verified", "Archived")</param>
    /// <param name="errorMessage">Out parameter containing error message if parsing fails</param>
    /// <returns>True if status was set successfully; false otherwise</returns>
    public bool TrySetStatusFromString(string statusName, out string? errorMessage)
    {
        errorMessage = null;

        if (string.IsNullOrWhiteSpace(statusName))
        {
            errorMessage = "Status cannot be empty or whitespace.";
            return false;
        }

        if (Enum.TryParse<CatalogStatus>(statusName, ignoreCase: true, out var parsedStatus))
        {
            Status = parsedStatus;
            return true;
        }

        var validValues = string.Join(", ", Enum.GetNames(typeof(CatalogStatus)));
        errorMessage = $"Invalid status '{statusName}'. Valid values are: {validValues}";
        return false;
    }
}
