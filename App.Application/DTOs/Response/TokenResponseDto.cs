namespace App.Application.DTOs.Response
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = default!;

        public string RefreshToken {  get; set; } = default!;   
    }
}
