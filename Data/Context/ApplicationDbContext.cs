using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Crear los modelos o las entidades.
        public DbSet<Category> Category { get; set; }
    }
}