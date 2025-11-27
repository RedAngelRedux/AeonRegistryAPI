
using AeonRegistryAPI.Endpoints.Home;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // This allows serving static files like images from wwwroot folder

// Map Endpoints
app.MapHomeEndpoints();

app.Run();