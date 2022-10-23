using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RandomJokes.Data
{
    public class JokesDataContext : DbContext
    {
        private readonly string _connectionString;
        public JokesDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<UserLikedJoke>()
                .HasKey(uj => new { uj.UserId, uj.JokeId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<UserLikedJoke> UserLikedJokes { get; set; }
    }
}
