using App.Application.DTOs.Request;
using App.Application.DTOs.Response;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.API.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(
            IAuthService authService,  
            ILogger<TokenController> logger
            )
        {
            _authService = authService;
            _logger = logger;

        }
        [HttpGet("generate-token")]
        public async Task<ActionResult<TokenResponseDto>> GenerateToken([FromQuery] string email, [FromQuery] string password)
        {
                _logger.LogInformation($"GetToken method invoked inside TokenController. The username and password supplied is {email} and password is {password}");

                var tokenResponse = await _authService.GenerateTokenAsync(email, password);
                return Ok(tokenResponse);
        }
        [HttpPost("token-refresh")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
                _logger.LogInformation("GenerateRefreshToken method invoked inside TokenController.");
                var tokenResponse = await _authService.GenerateRefreshTokenAsync(refreshTokenRequest);
                return Ok(tokenResponse);
        }
    }
}
