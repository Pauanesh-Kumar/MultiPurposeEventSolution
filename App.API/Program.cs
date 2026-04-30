using App.API.Middlewares;
using App.Application;
using App.Application.Interfaces;
using App.Application.Mappings;
using App.Domain.Interfaces.Services;
using App.Infrastructure;
using App.Infrastructure.ExternalServices;
using App.Infrastructure.Mappers;
using App.Infrastructure.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Exceptions;
using System.Configuration;
using System.Text;



namespace App.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                      .Enrich.WithEnvironmentName()
                   .Enrich.WithExceptionDetails()
                 .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddMemoryCache();

            builder.Services.AddCors(options => 
            { 
                options.AddPolicy("AllowAll", builder => 
                { 
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); 
                }); 
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };

                });

            // Register dependencies
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddSingleton<ILoggerFactory, NullLoggerFactory>(); // For AutoMapper's internal needs
            builder.Services.AddTransient<RefreshTokenExpiryResolverInfra>(); // Explicitly register resolver
            //builder.Services.AddTransient<ITimeProvider, TimeProvider>(); // Register TimeProvider
           
            // Register AutoMapper profiles for both Application and Infrastructure layers
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly); // Scan App.Application assembly
                cfg.AddMaps(typeof(InfraAssemblyMarker).Assembly); // Scan App.Infrastructure assembly
            });

            builder.Services.AddApplication();

            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DatabaseConnection")!);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //var serviceProvider = builder.Services.BuildServiceProvider();
            //using (var scope = serviceProvider.CreateScope())
            //{
            //    var authService = scope.ServiceProvider.GetService<IAuthService>();
            //    var userService = scope.ServiceProvider.GetService<IUserService>();
            //}

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();

            app.UseAuthorization();
            //app.UseCors();


            app.UseMiddleware<CommonResponseMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //app.MapControllers();
            app.MapControllers();
            app.Run();
        }
    }
}
