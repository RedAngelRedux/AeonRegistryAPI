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
}
