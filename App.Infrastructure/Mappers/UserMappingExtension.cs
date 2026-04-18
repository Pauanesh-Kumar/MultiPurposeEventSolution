using AutoMapper;
using App.Domain.Entities;
using App.Infrastructure.Data.Entities;

namespace App.Infrastructure.Mappers;

public class UserMappingExtension : Profile
{
    public UserMappingExtension()
    {
        CreateMap<UserDomain, User>().ReverseMap()
            .ForMember(dest => dest.UserRoles,
                opt =>
                    opt.MapFrom(src => src.Roles.Select(x => x.Name))); // mapping role name
    }
}