namespace AeonRegistryAPI.Models.Response;

public class CatalogNoteResponse
{
    public int Id { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime DateSubmitted { get; set; }
}
