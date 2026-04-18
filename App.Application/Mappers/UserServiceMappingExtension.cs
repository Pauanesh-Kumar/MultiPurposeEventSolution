// App.Application/Mappings/UserServiceMappingProfile.cs
using App.Application.DTOs.Request;
using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Domain.Interfaces.Services;
using AutoMapper;

namespace App.Application.Mappings
{
    public class UserServiceMappingProfile : Profile
    {
        public UserServiceMappingProfile()
        {
            CreateMap<RegisterUserDto, UserDomain>()
                .ForMember(dest => dest.Password,
                    opt => opt.MapFrom<PasswordHashResolver>());
        }
    }

    public class PasswordHashResolver : IValueResolver<RegisterUserDto, UserDomain, string>
    {
        private readonly IPasswordService _passwordService;

        public PasswordHashResolver(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public string Resolve(
            RegisterUserDto source,
            UserDomain destination,
            string destMember,
            ResolutionContext context)
        {
            return _passwordService.HashPassword(source.Password);
        }
    }
}