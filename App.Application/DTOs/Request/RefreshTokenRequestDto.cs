namespace App.Application.DTOs.Request
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken {  set; get; } = default!;

        public string RefreshToken { set; get; } = default!;
    }
}
