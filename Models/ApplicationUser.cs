using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AeonRegistryAPI.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required]
    public string?  LastName { get; set; }

    public string FullName => (MiddleName is not null) 
        ? $"{FirstName} {MiddleName} {LastName}" 
        : $"{FirstName} {LastName}";

    public ResearchSpecialty ResearchSpecialty { get; set; }

    public ICollection<CatalogRecord> SubmittedCatalogRecords { get; set;  } = [];

    public ICollection<CatalogRecord> VerifiedCatalogRecords { get; set;  } = [];

    public ICollection<ArtifactMediaFile> UploadedMediaFiles { get; set; } = [];

}
