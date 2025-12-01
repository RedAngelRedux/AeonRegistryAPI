using AeonRegistryAPI.Endpoints.CustomIdentity.Models.Requests;
using AeonRegistryAPI.Endpoints.CustomIdentity.Models.Responses;
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
        // TODO:  Replace RequireAuthroizatoin with AdminPoliy
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

        group.MapGet("/manage/profile", CustomIdentityHandlers.GetProfileInfo)
            .WithName("GetProfileInfo")
            .WithDescription("Get current user's profile information.")
            .WithSummary("Get the current users' profile")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        group.MapPut("/manage/profile", CustomIdentityHandlers.UpdateProfileInfo)
            .WithName("UpdateProfileInfo")
            .WithDescription("Update current user's profile information.")
            .WithSummary("Update the current users' profile")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        group.MapGet("/manage/users", CustomIdentityHandlers.ListAllUsers)
            .WithName("ListAllUsers")
            .WithDescription("Retrieve a list of all registered users.")
            .WithSummary("List all users")
            .Produces<IEnumerable<UserProfileResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
            

        // TODO:  Replace RequireAuthroizatoin with AdminPoliy
        //.RequireAuthorization("AdminPolicy");


        // TODO: Step 3:  Implement Route Handlers in CustomIdentityHandlers class        

        // TODO: Step 4:  Add OpenAPI metadata to each endpoint

        // TODO: Step 5:  Return the modified IEndpointRouteBuilder        
        return route;
    }
}