using AeonRegistryAPI.Endpoints.CustomIdentity.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Principal;
using System.Text;

namespace AeonRegistryAPI.Endpoints.CustomIdentity;

public static class CustomIdentityHandlers
{
    public static async Task<IResult> RegisterUser(
        RegisterUserRequest request,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailSender emailSender,
        IConfiguration config)
    {
        // see if the user already exists
        if (await userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Results.BadRequest(new { Error = $"A User with email {request.Email} already exists" });
        }

        // Make a new ApplicationUser
        ApplicationUser user = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        string tempPassword = "Abc@123!"; // pwd must meet complexity rules
        var created = await userManager.CreateAsync(user, tempPassword);
        if (!created.Succeeded)
        {
            return Results.BadRequest(new { Error = $"Could not create user {request.Email} ", Details = created.Errors });
        }

        // Generate a password reset token
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        // Put the user in the Researcher role
        if (await roleManager.RoleExistsAsync("Researcher"))
        {
            await userManager.AddToRoleAsync(user, "Researcher");
        }

        // Send the user an email with their temp password to change password
        var baseURl = config["BaseUrl"] ?? "https://localhost:7023";
        var setPwdLink = $"{baseURl}/Setpassword.html?email={request.Email}&resetCode={encodeToken}";

        await EmailLink(emailSender, request.Email,"Your New Account",
            "Your account has been created. Please change your password by visiting...",
            setPwdLink);

        //await emailSender.SendEmailAsync(
        //    request.Email,
        //    "Your New Account",

        //    $"""

        //    Your account has been created.  Please change your password by visiting:  {baseURl}/SetPasswrod.html

        //    {baseURl}/Setpassword.html?email={request.Email}&resetCode={encodeToken}

        //    """
        //    );

        return Results.Ok(new { Message = $"User, {request.Email}, created. Passsword reset email sent." });
    }

    public static async Task<IResult> ResetPassword(
        ResetPasswordRequest request,
        UserManager<ApplicationUser> userManager)
    {
        // Validate the request
        if (string.IsNullOrEmpty(request.Email)
         || string.IsNullOrEmpty(request.ResetCode)
         || string.IsNullOrEmpty(request.NewPassword))
        {
            return Results.BadRequest(new { Message = "All fields are required" });
        }

        // Find the User
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Results.BadRequest(new { Message = "user not found." });
        }

        // Attempt to reset the password
        try
        {
            var decodeToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(request.ResetCode));

            var result = await userManager.ResetPasswordAsync(user, decodeToken, request.NewPassword);
            if (!result.Succeeded)
                return Results.BadRequest(new { Message = "Unable to reset password." });

            return Results.Ok(new { Message = "Password successfully reset." });
        }
        catch (FormatException)
        {
            return Results.BadRequest(new { Message = "Invalid Token" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = $"Error: {ex.Message}" });
        }
    }

    public static async Task<IResult> ForgotPassword(
        ForgotPasswordRequest request,
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        IConfiguration config)
    {
        if(string.IsNullOrEmpty(request.Email))
            return Results.BadRequest(new { Message = "Email is required." });

        var user = await userManager.FindByEmailAsync(request.Email);
        if(user is null)
            return Results.Ok(new { Message = "If the user existes a forgot password link will be sent." });

        // Generate a password reset token
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        // Send the user an email with their temp password to change password
        var baseURl = config["BaseUrl"] ?? "https://localhost:7023";
        var resetLink = $"{baseURl}/reset-password.html?email={user.Email}&resetCode={encodeToken}";

        await EmailLink(emailSender,request.Email,
            "Forgot Password",
            "To reset your password, click on this link...",
            resetLink);
        
        return Results.Ok(new { Message = "If the user existes a forgot password link will be sent." });
    }

    private static async Task EmailLink(IEmailSender emailSender, [EmailAddress] string email, string subject, string body, string resetLink)
    {
        await emailSender.SendEmailAsync(
            email,
            $"{subject}",
            $"""

            {body}

            {resetLink}

            """
            );
    }

}
