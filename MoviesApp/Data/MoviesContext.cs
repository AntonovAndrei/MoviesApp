using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext (DbContextOptions<MoviesContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Acter> Acters { get; set; }
        public DbSet<ActerMovie> ActerMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActerMovie>(entity =>
                entity.HasKey(sc => new {sc.ActerId, sc.MovieId}));

            modelBuilder.Entity<Acter>().Property(n => n.Name).IsRequired();
            modelBuilder.Entity<Acter>().Property(n => n.LastName).IsRequired();
        }
    }
}