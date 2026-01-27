
namespace AeonRegistryAPI.Services.Selectors;

using System.Linq.Expressions;

public static class CatalogRecordSelectors
{
    public static readonly Expression<Func<CatalogRecord, CatalogRecordResponse>> ToResponse =
        cr => new CatalogRecordResponse
        {
            Id = cr.Id,
            ArtifactId = cr.ArtifactId,
            ArtifactName = cr.Artifact!.Name ?? string.Empty,
            SubmittedById = cr.SubmittedById,
            SubmittedByName = cr.SubmittedBy!.FullName ?? string.Empty,
            VerifiedById = cr.VerifiedById,
            VerifiedByName = cr.VerifiedBy!.FullName ?? string.Empty,
            Status = cr.Status.ToString(),
            Notes = cr.Notes.Select(n => new CatalogNoteResponse
            {
                Id = n.Id,
                AuthorId = n.AuthorId,
                AuthorName = n.Author!.FullName ?? string.Empty,
                Content = n.Content,
                DateSubmitted = n.DateSubmitted
            }).ToList()
        };
}


