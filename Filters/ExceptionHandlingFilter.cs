
using System.Diagnostics.Eventing.Reader;

namespace AeonRegistryAPI.Filters;

public class ExceptionHandlingFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception ex)
        {
            var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            // TODO:  Replace with a "professional" logger
            Console.WriteLine($"Exception caught in filter:  {ex.Message}");
            // Return a generic error response
            return Results.Problem(
                detail: env.IsDevelopment() ? ex.ToString() : null,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred.");
        }

    }
}
