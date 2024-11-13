using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Categoría nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo de caracteres es 100.")]
        public string NameCategory { get; set; }

        public DateTime CreationDate { get; set; }
    }
}