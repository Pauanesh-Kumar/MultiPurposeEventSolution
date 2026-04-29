using App.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper profiles
builder.Services.AddAutoMapper(typeof(UserTokenMappingExtension));

// Register other dependencies (example: IConfiguration is already registered by default)
builder.Services.AddScoped<RefreshTokenExpiryResolver>();

// Add controllers and other services as needed
builder.Services.AddControllers();

// Build and run the app
var app = builder.Build();

app.MapControllers();

app.Run();
