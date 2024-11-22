using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NameMovie { get; set; }

        public string Description { get; set; }

        public string RouteImage { get; set; }

        public int Duration { get; set; }

        public enum TypeClassification { Siete, Trece, Dieciocho }

        public TypeClassification Clasification { get; set; }

        public DateTime CreationDate { get; set; }

        // Relacion con tabla Category
        public int categoryId { get; set; }

        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
}