
using AeonRegistryAPI.Endpoints.CustomIdentity;
using AeonRegistryAPI.Endpoints.Home;
using AeonRegistryAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCustomSwagger();

// Get a connection string to the database from appsettings.json
var connectionString = DataUtility.GetConnectionString(builder.Configuration);

// Configure the database context to use PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Identity endpoints
builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>();

// Add an Admin policy
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

// Add email sender service
builder.Services.AddTransient<IEmailSender, ConsoleEmailService>();

// Enable validation for minimal APIs (This is a .NET 10 feature)
builder.Services.AddValidation();  // Enforces [Required] and other data annotations

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // This allows serving static files like images from wwwroot folder
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BlockIdentityEndpoints>();

// Map Endpoints
var authRouteGroup = app.MapGroup("/api/auth")
    .WithTags("Admin");
authRouteGroup.MapIdentityApi<ApplicationUser>();

app.MapCustomIdentityEndpoints();
app.MapHomeEndpoints();

app.Run();

// TODO:  Create a Role Manager
// TODO:  Allow users to change their e-mail address