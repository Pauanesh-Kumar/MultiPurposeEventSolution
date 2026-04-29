using App.Domain.Entities;
using App.Infrastructure.Data.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Mappings
{
    public class UserTokenMappingExtensionInfra : Profile
    {
        public UserTokenMappingExtensionInfra()
        {
            CreateMap<UserTokenDomain, UserToken>()
                .ForMember(dest => dest.RefreshTokenExpiryTime,
                    opt => opt.MapFrom<RefreshTokenExpiryResolverInfra>());

            CreateMap<UserToken, UserTokenDomain>();
        }
    }

    public class RefreshTokenExpiryResolverInfra : IValueResolver<UserTokenDomain, UserToken, DateTime?>
    {
        private readonly IConfiguration _configuration;

        public RefreshTokenExpiryResolverInfra(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DateTime? Resolve(UserTokenDomain source, UserToken destination, DateTime? destMember, ResolutionContext context)
        {
            var expiryDaysString = _configuration["Jwt:RefreshTokenExpiryInDays"];
            if (!int.TryParse(expiryDaysString, out var expiryDays))
            {
                throw new InvalidOperationException("Invalid or missing Jwt:RefreshTokenExpiryInDays configuration.");
            }
            return DateTime.UtcNow.AddDays(expiryDays);
        }
    }
}