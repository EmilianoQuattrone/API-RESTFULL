using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.User
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Usuario es obligatorio.")]
        public string NameUser { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password es obligatorio.")]
        public string Password { get; set; }
    }
}