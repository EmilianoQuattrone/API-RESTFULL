using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category
    {
        // Este decorador indica al campo que va hacer una PK y autoincremental.
        [Key] 
        public int Id { get; set; }

        [Required]
        public string NameCategory { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }
}