using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.User
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Usuario es obligatorio.")]
        public string NameUser { get; set; }

        [Required(ErrorMessage = "Password es obligatorio.")]
        public string Password { get; set; }
    }
}