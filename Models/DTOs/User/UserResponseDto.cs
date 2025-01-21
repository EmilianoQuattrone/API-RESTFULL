namespace Models.DTOs.User
{
    public class UserResponseDto
    {
        public Models.Entities.User User { get; set; }

        public string Rol { get; set; }

        public string Token { get; set; }
    }
}