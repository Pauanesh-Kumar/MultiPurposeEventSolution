//using App.Application.Contracts;
//using App.Application.Implementation;
using App.Application.DTOs.Request;
using App.Application.Interfaces;
using App.Application.Mappings;
using App.Application.Services;
using App.Domain.Entities;
using App.Domain.UseCases.User;
using App.Domain.UseCases.UserTocken.MyApp.Domain.UseCases.User;
using App.Domain.UseCases.UserToken;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


namespace App.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<GenerateTokenUseCase>();
            services.AddScoped<GenerateRefreshTokenUseCase>();
            services.AddScoped<RegisterUserUseCase>();
            services.AddScoped<GetUserByEmailUseCase>();
            services.AddScoped<ValidateTokenUseCase>();
            services.AddScoped<IValueResolver<RegisterUserDto, UserDomain, string>, PasswordHashResolver>();
            services.AddScoped<PasswordHashResolver>();
            return services;
        }
    }
}
