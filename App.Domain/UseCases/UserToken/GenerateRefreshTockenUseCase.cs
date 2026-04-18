// App.Domain/UseCases/User/GenerateRefreshTokenUseCase.cs
using App.Domain.Interfaces.Repositories;
using App.Domain.Interfaces.Services;
using App.Domain.Entities;
using App.Domain.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Domain.UseCases.UserToken;

namespace App.Domain.UseCases.User
{
    public class GenerateRefreshTokenUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserTokenRepository _userTokenRepository;
 
        public GenerateRefreshTokenUseCase(
            IUserRepository userRepository,
            ITokenService tokenService,
            IUserTokenRepository userTokenRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<(string AccessToken, string RefreshToken)> ExecuteAsync(string accessToken, string refreshToken)
        {
            var principal = _tokenService.ValidateAccessToken(accessToken);
            if (principal == null)
                throw new InvalidOperationException("Invalid Access Token");

            var userId = principal.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            if (userId == null)
                throw new InvalidOperationException("Invalid token claims");

            var previousToken = await _userTokenRepository.GetTokenByUserIdAsync(Convert.ToInt32(userId));
            if (previousToken == null ||
                previousToken.RefreshToken != refreshToken ||
                previousToken.AccessToken != accessToken ||
                previousToken.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new InvalidOperationException("Invalid Refresh Token");

            var emailAddress
                = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;

            var user = await _userRepository.GetUserByEmailAsync(emailAddress);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newUserToken = new UserTokenDomain
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };
            await _userTokenRepository.AddUserTokenAsync(newUserToken);

            return (newAccessToken, newRefreshToken);
        }
    }
}