// app.Domain/UseCases/User/GenerateTokenUseCase.cs
using App.Domain.Entities;
using System.Threading.Tasks;
using App.Domain.Interfaces.Repositories;
using App.Domain.Interfaces.Services;

namespace App.Domain.UseCases.UserToken
{
    public class GenerateTokenUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IPasswordService _passwordService;

        public GenerateTokenUseCase(
            IUserRepository userRepository,
            ITokenService tokenService,
            IUserTokenRepository userTokenRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userTokenRepository = userTokenRepository;
            _passwordService = passwordService;
        }

        public async Task<(string AccessToken, string RefreshToken)> ExecuteAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new InvalidOperationException($"No user with {email} exists in database.");

            bool isValidPassword = _passwordService.VerifyPassword(password, user.Password);
            if (!isValidPassword)
                throw new InvalidOperationException($"Invalid credentials for user {email}");

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var userToken = new UserTokenDomain
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7), // Example expiry
                UserId = user.Id
            };
            await _userTokenRepository.AddUserTokenAsync(userToken);

            return (accessToken, refreshToken);
        }
    }
}
