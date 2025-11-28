using AeonRegistryAPI.Endpoints.CustomIdentity.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AeonRegistryAPI.Endpoints.CustomIdentity;

public static class CustomIdentityEndpoints
{
    public static IEndpointRouteBuilder MapCustomIdentityEndpoints(this IEndpointRouteBuilder route)
    {
        // Step 1:  Make a Grouop for Custom Identity Endpoints
        var group = route.MapGroup("/api/auth")
            .WithTags("Admin");

        // Step 2:  Map Endpoints in the group to their handlers
        group.MapPost("/register-admin", RegisterUser)
            .WithName("RegisterAdmin")
            .WithSummary("Register a User")
            .WithDescription("Registerrs a user must have admin role.")
            //.RequireAuthorization("AdminPolicy");
            ;
        // Step 3:  Implement Route Handlers in CustomIdentityHandlers class
        // (see below)

        // Step 4:  Add OpenAPI metadata to each endpoint

        // Step 5:  Return the modified IEndpointRouteBuilder        
        return route;
    }

    
    private static async Task<IResult> RegisterUser(
        RegisterUserRequest dto,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailSender emailSender,
        IConfiguration config
        )
    {
        // see if the user already exists
        if(await userManager.FindByEmailAsync(dto.Email) is not null)
        {
            return Results.BadRequest(new { Error = $"A User with email {dto.Email} already exists"});
        }

        // Make a new ApplicationUser
        ApplicationUser user = new() 
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName            
        };
        string tempPassword = "Abc@123!"; // pwd must meet complexity rules
        var created = await userManager.CreateAsync(user, tempPassword);
        if(!created.Succeeded)
        {
            return Results.BadRequest(new { Error = $"Could not create user {dto.Email} ", Details = created.Errors });
        }

        // Generate a password reset token
        var resetToken =  await userManager.GeneratePasswordResetTokenAsync(user);
        var encodeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        // Put the user in the Researcher role
        if (await roleManager.RoleExistsAsync("Researcher"))
        {
           await userManager.AddToRoleAsync(user, "Researcher");
        }

        // Send the user an email with their temp password to change password
        var baseURl = config["BaseUrl"] ?? "https://localhost:7023";
        await emailSender.SendEmailAsync(
            dto.Email,
            "Your New Account",
            
            $"""

            Your account has been created.  Please change your password by visiting:  {baseURl}/SetPasswrod.html

            {baseURl}/Setpassword.html?email={dto.Email}&resetCode={encodeToken}

            """
            );

        return Results.Ok(new { Message = $"User, {dto.Email}, created. Passsword reset email sent."});
    }
}
