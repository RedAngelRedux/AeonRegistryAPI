namespace AeonRegistryAPI.Models;

public class ArtifactMediaFile
{
    public int Id { get; set; }

    public int ArtifactId { get; set; }

    public Artifact? Artifact { get; set; } = null;

    public string UploaderId { get; set; } = string.Empty; // FK to ApplicationUser

    public ApplicationUser? Uploader { get; set; } = null;

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = "image/jpeg";

    public byte[] Data{ get; set; } = Array.Empty<byte>();

    // optional to mark one media file as primary (e.g. main image)
    public bool IsPrimary { get; set; } = false;
}
