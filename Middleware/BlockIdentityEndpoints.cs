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
            await context.Response.WriteAsync("The Endpoint Does Not Exist.");
            return;
        }
        await _next(context);
    }
}
