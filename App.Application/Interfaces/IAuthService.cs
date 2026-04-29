// App.Application/Interfaces/IAuthService.cs
using App.Application.DTOs.Request;
using App.Application.DTOs.Response;

namespace App.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> GenerateTokenAsync(string email, string password);
        Task<TokenResponseDto> GenerateRefreshTokenAsync(RefreshTokenRequestDto refreshToken);
    }
}