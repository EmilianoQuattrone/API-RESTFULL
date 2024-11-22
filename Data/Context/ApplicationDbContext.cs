using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Crear los modelos o las entidades, El nombre en plural, que se vera en sql server.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}