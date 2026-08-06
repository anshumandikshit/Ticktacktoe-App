using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Scoreboard> Scoreboards { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Non-clustered index on GameId + Timestamp
            modelBuilder.Entity<Move>()
                .HasIndex(m => new { m.GameId })
                .HasDatabaseName("IX_Move_GameId")
                .IsClustered(false); // ✅ explicitly non-clustered
        }
    }
}
