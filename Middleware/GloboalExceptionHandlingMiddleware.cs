namespace AeonRegistryAPI.Middleware;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Middleware that catches ALL exceptions including BadHttpRequestException from model binding.
/// Must be registered BEFORE UseExceptionHandler in Program.cs.
/// </summary>
public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            // Handle cases where no exception was thrown but status code indicates an error
            // This catches BadHttpRequestException which is handled internally by ASP.NET Core
            if (context.Response.StatusCode is >= 400 and < 600)
            {
                // Response body may already be written, so only handle if not
                if (!context.Response.HasStarted)
                {
                    await HandleErrorResponseAsync(context, context.Response.StatusCode,
                        "An error occurred processing your request.");
                }
            }
        }
        catch (BadHttpRequestException ex)
        {
            _logger.LogError(ex, "BadHttpRequestException occurred");
            await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error");
            await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest,
                "Invalid JSON format in request body.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument");
            await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access");
            await HandleExceptionAsync(context, ex, StatusCodes.Status403Forbidden);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string? customMessage = null)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleFromStatusCode(statusCode),
            Detail = customMessage ?? exception.Message,
            Instance = context.Request.Path
        };

        return context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static Task HandleErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleFromStatusCode(statusCode),
            Detail = message,
            Instance = context.Request.Path
        };

        return context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static string GetTitleFromStatusCode(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Not Found",
        StatusCodes.Status409Conflict => "Conflict",
        StatusCodes.Status422UnprocessableEntity => "Unprocessable Entity",
        StatusCodes.Status500InternalServerError => "Internal Server Error",
        StatusCodes.Status503ServiceUnavailable => "Service Unavailable",
        _ => "An Error Occurred"
    };
}
