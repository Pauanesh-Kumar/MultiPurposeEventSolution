namespace App.Application.DTOs.Response
{
    public class UserResponseDto
    {
        public UserResponseDto()
        {
            Roles = [];
        }

        public string Name { get; set; } = default!;
         
        public string Email { get; set; } = default!;

        public int UserId { get; set; }

        public List<string> Roles { get; set; }
    }
}
