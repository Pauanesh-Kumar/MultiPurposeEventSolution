using AutoMapper;
using App.Application. DTOs.Request;
using App.Application.DTOs.Response;
using App.Domain.Entities;

namespace App.Application.Mappers
{
    public class UserTokenMappingExtension : Profile
    {
        public UserTokenMappingExtension()
        {
            CreateMap<UserTokenRequestDto, UserTokenDomain>();

            CreateMap<UserTokenDomain, RefreshTokenResponseDto>();
        }
    }
}
