using AeonRegistryAPI.Endpoints.CustomIdentity.Models.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AeonRegistryAPI.Endpoints.CustomIdentity;

public static class CustomIdentityEndpoints
{
    public static IEndpointRouteBuilder MapCustomIdentityEndpoints(this IEndpointRouteBuilder route)
    {
        // Make a Grouop for Custom Identity Endpoints
        var group = route.MapGroup("/api/auth")
            .WithTags("Admin");

        // Map Endpoints in the group to their handlers
        group.MapPost("/register-admin", CustomIdentityHandlers.RegisterUser)
            .WithName("RegisterAdmin")
            .WithSummary("Register a User")
            .WithDescription("Registerrs a user must have admin role.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        //.RequireAuthorization("AdminPolicy");
        ;

        group.MapPost("reset-password", CustomIdentityHandlers.ResetPassword)
            .WithName("ResetPassword")
            .WithDescription("Custom Reset Password for a user")
            .WithSummary("Custom Reset Password")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("forgot-password", CustomIdentityHandlers.ForgotPassword)
            .WithName("ForgotPassword")
            .WithDescription("Custom Forgot Password flow")
            .WithSummary("Custom Forgot Password")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);


        // TODO: Step 3:  Implement Route Handlers in CustomIdentityHandlers class        

        // TODO: Step 4:  Add OpenAPI metadata to each endpoint

        // TODO: Step 5:  Return the modified IEndpointRouteBuilder        
        return route;
    }
}