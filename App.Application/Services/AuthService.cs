// MyApp.Application/Services/AuthService.cs
using App.Application.DTOs.Request;
using App.Application.DTOs.Response;
using AutoMapper;
using App.Application.Interfaces;
using App.Domain.UseCases.User;
using App.Domain.UseCases.UserToken;
//using System.Threading.Tasks;

namespace App.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly GenerateTokenUseCase _generateTokenUseCase;
        private readonly GenerateRefreshTokenUseCase _generateRefreshTokenUseCase;
        private readonly IMapper _mapper;

        public AuthService(
            GenerateTokenUseCase generateTokenUseCase,
            GenerateRefreshTokenUseCase generateRefreshTokenUseCase,
            IMapper mapper)
        {
            _generateTokenUseCase = generateTokenUseCase;
            _generateRefreshTokenUseCase = generateRefreshTokenUseCase;
            _mapper = mapper;
        }

        public async Task<TokenResponseDto> GenerateTokenAsync(string email, string password)
        {
            var (accessToken, refreshToken) = await _generateTokenUseCase.ExecuteAsync(email, password);
            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponseDto> GenerateRefreshTokenAsync(RefreshTokenRequestDto refreshToken)
        {
            var (accessToken, newRefreshToken) = await _generateRefreshTokenUseCase.ExecuteAsync(
                refreshToken.AccessToken, refreshToken.RefreshToken);
            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}