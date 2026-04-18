using System.Security.Claims;
using App.Domain.Entities;
using System.Threading.Tasks;

namespace App.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(UserDomain user);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateAccessToken(string accessToken);
    }
}
