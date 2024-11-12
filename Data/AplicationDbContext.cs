using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
            : base (options) { }

        // Crear los modelos o las entidades.
        public DbSet<Category> Categories { get; set; }
    }
}