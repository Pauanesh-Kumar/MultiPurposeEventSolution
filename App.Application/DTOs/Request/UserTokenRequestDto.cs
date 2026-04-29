namespace App.Application.DTOs.Request
{
    public class UserTokenRequestDto
    {
        public string AccessToken { get; set; } = default!; 
        public string RefreshToken { get; set; } = default!;
        public int UserId {  get; set; }    
    }
}
