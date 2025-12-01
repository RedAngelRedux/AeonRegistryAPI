namespace AeonRegistryAPI.Endpoints.CustomIdentity.Models.Requests;

public record UpdateUserProfileRequest(
    [Required] string? FirstName,
    string? MiddleName,
    [Required] string? LastName,
    string? PhoneNumber
);
