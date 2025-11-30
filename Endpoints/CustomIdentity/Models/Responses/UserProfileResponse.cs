namespace AeonRegistryAPI.Endpoints.CustomIdentity.Models.Responses;

public record UserProfileResponse(
    string? Id,
    string? UserName,
    string? Email,
    string? LastName,
    string? FirstName,
    string? MiddleName,
    string? FullName,
    string? PhoneNumber
);