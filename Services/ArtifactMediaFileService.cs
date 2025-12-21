
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services;

public class ArtifactMediaFileService(ApplicationDbContext db) : IArtifactMediaFileService
{
    public async Task<ArtifactMediaFile?> CreateArtifactMediaFileAsync(
        int artifactId, 
        IFormFile file, 
        bool isPrimary, 
        CancellationToken ct)
    {
        var artifact = await db.Artifacts.FindAsync([artifactId], ct);
        if (artifact is null)return null;

        // Validate file type and size as needed
        if(file is null || file.Length == 0)
            throw new ArgumentException("Invalid file.");

        // Check for primary file constraint
        if(isPrimary)
        {
            var existingPrimary = await db.ArtifactMediaFiles
                .Where(amf => amf.ArtifactId == artifactId && amf.IsPrimary)
                .ToListAsync(ct);
            foreach(var primary in existingPrimary)
            {
                primary.IsPrimary = false;
            }
        }

        // Convert IFormFile to byte array
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        var fieData = ms.ToArray();

        // Create and save the media file
        var mediaFile = new ArtifactMediaFile
        {
            ArtifactId = artifactId,
            FileName = file.FileName,
            ContentType = file.ContentType ?? "application/octet-stream",
            Data = fieData,
            IsPrimary = isPrimary
        };

        db.ArtifactMediaFiles.Add(mediaFile);
        await db.SaveChangesAsync(ct);

        return mediaFile;
    }
}
