using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Entities;

namespace MovieLibrary.Core
{
    public class MovieLibraryContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<MovieCategory> MovieCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MovieLibrary.db");
        }
    }
}
