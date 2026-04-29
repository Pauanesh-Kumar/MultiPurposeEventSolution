using App.Domain.Entities;
using System.Threading.Tasks;
using App.Domain.Interfaces.Repositories;
using App.Domain.Interfaces.Services;
using System.Security.Claims;

namespace App.Domain.UseCases.UserTocken
{
    namespace MyApp.Domain.UseCases.User
    {
        public class ValidateTokenUseCase
        {
            private readonly ITokenService _tokenService;

            public ValidateTokenUseCase(ITokenService tokenService)
            {
                _tokenService = tokenService;
            }

            public ClaimsPrincipal Execute(string accessToken)
            {
                var principal = _tokenService.ValidateAccessToken(accessToken);
                if (principal == null)
                    throw new InvalidOperationException("Invalid Access Token");
                return principal;
            }
        }
    }
}
