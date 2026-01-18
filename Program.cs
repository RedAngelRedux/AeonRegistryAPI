
using AeonRegistryAPI.Endpoints.Artifacts;
using AeonRegistryAPI.Endpoints.CustomIdentity;
using AeonRegistryAPI.Endpoints.Home;
using AeonRegistryAPI.Endpoints.Sites;
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

// Custom Services
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IArtifactService, ArtifactService>();

// Enable validation for minimal APIs (This is a .NET 10 feature)
builder.Services.AddValidation();  // Enforces [Required] and other data annotations

var app = builder.Build();

// Configure the HTTP request pipeline.
// By NOT wrapping in app.Environment.IsDevelopment, we enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

// Register custom global exception handling middleware FIRST
app.UseMiddleware<BlockIdentityEndpoints>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles(); // This allows serving static files like images from wwwroot folder
app.UseAuthentication();
app.UseAuthorization();


// Map Endpoints
var authRouteGroup = app.MapGroup("/api/auth")
    .WithTags("Admin");
authRouteGroup.MapIdentityApi<ApplicationUser>();

app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapCustomIdentityEndpoints();
app.MapHomeEndpoints();
app.MapSiteEndpoints();
app.MapArtifactMediaFileEndpoints();
app.MapArtifactEndpoints();

using (var scope = app.Services.CreateScope())
{
    await DataSeed.ManageDataAsync(scope.ServiceProvider);
}

app.Run();

// TODO:  Create a Role Manager
// TODO:  Allow users to change their e-mail address