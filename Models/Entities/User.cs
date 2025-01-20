using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string NameUser { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Rol { get; set; }
    }
}