using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Categoría nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo de caracteres es 100.")]
        public string NameCategory { get; set; }
    }
}