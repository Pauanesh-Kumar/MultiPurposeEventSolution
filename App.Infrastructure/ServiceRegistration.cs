using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Domain.Interfaces.Repositories;
using App.Domain.Interfaces.Services;
using App.Infrastructure.Data.Entities;
using App.Infrastructure.ExternalServices;
using App.Infrastructure.Mappers;
using App.Infrastructure.Repositories;


//using App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace App.Infrastructure
{
    public static class ServiceRegistration
    {
        // Remove or fix the following line:
        // services.AddSingleton<IConfiguration>(Configuration); // Assuming Configuration is your IConfiguration instance

        // If you want to register IConfiguration, you should pass an instance of IConfiguration to this method.
        // For example, change your method signature to accept IConfiguration:

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
           

            return services;
        }
    }
}
