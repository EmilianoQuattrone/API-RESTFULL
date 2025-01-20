namespace Models.DTOs.User
{
    public class UserResponseDto
    {
        public UserDataDto User { get; set; }

        public string Rol { get; set; }

        public string Token { get; set; }
    }
}