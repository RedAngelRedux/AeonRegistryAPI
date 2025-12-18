using Microsoft.AspNetCore.Mvc;

namespace AeonRegistryAPI.Middleware;

public class BlockIdentityEndpoints(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private static readonly string[] _blockedPaths =
        [
        "/api/auth/register",
        "/api/auth/forgotpassword",
        "/api/auth/resetpassword",
        "/api/auth/manage",
        "/api/auth/manage/info",
        ];

    public async Task InvokeAsync(HttpContext context)
    {
        var requestPath = context.Request.Path.Value?.ToLower();
        if (requestPath != null && _blockedPaths.Contains(requestPath))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "The endpoint does not exist.",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
            return;
        }

        // Let GlobalExceptionHandlingMiddleware handle any downstream exceptions
        await _next(context);
    }
}
