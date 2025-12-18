using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Filters;

public class ExceptionHandlingFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (KeyNotFoundException ex)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return TypedResults.Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (ArgumentException ex)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return TypedResults.Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return TypedResults.Problem(detail: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
