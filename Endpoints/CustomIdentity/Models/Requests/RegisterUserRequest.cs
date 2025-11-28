
namespace AeonRegistryAPI.Endpoints.CustomIdentity.Models.Requests;

public record RegisterUserRequest(
    [Required, EmailAddress] string Email,
    [Required] string FirstName,
    [Required] string LastName
);