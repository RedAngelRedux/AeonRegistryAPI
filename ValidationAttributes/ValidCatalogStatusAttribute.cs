using System.ComponentModel.DataAnnotations;
using AeonRegistryAPI.Enums;

namespace AeonRegistryAPI.ValidationAttributes;

/// <summary>
/// Validates that a string value represents a valid CatalogStatus enum value.
/// Supports case-insensitive matching.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ValidCatalogStatusAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "Invalid catalog status '{0}'. Valid values are: {1}";

    public ValidCatalogStatusAttribute() : base(DefaultErrorMessage)
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Allow null values - let [Required] handle null validation
        if (value == null)
            return ValidationResult.Success;

        // If already a CatalogStatus enum, it's valid
        if (value is CatalogStatus)
            return ValidationResult.Success;

        // If it's a string, attempt to parse it
        if (value is string statusString)
        {
            if (string.IsNullOrWhiteSpace(statusString))
                return new ValidationResult("Status cannot be empty or whitespace.");

            if (Enum.TryParse<CatalogStatus>(statusString, ignoreCase: true, out _))
                return ValidationResult.Success;

            var validValues = string.Join(", ", Enum.GetNames(typeof(CatalogStatus)));
            var errorMessage = string.Format(ErrorMessage ?? DefaultErrorMessage, statusString, validValues);
            return new ValidationResult(errorMessage);
        }

        return new ValidationResult($"Status must be a string or {nameof(CatalogStatus)} enum value.");
    }
}
