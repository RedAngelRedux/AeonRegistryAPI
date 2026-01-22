namespace AeonRegistryAPI.Models.Response;

public class CatalogNoteOnlyResponse
{
    public int Id { get; set; }

    [Required]
    public int CatalogRecordId { get; set; }

    public string AuthorId { get; set; } = string.Empty; // FK to ApplicationUser
    public ApplicationUser? Author { get; set; } = null;

    [Required, MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
}
