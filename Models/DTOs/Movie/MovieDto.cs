namespace Models.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string NameMovie { get; set; }

        public string Description { get; set; }

        public string RouteImage { get; set; }

        public int Duration { get; set; }

        public enum TypeClassification { Siete, Trece, Dieciocho }

        public TypeClassification Clasification { get; set; }

        public DateTime CreationDate { get; set; }

        public int categoryId { get; set; }
    }
}