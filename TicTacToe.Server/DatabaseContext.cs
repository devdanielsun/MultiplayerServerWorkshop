using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TicTacToe.Server.Model;

namespace TicTacToe.Server;

public class DatabaseContext : DbContext
{
    public DbSet<GameModel> Games { get; set; }
    public DbSet<PlayerModel> Players { get; set; }
    public DbSet<GamesPlayersModel> GamesPlayers { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Players table
        modelBuilder.Entity<PlayerModel>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Username).IsRequired().HasMaxLength(50);
            entity.HasIndex(p => p.Username).IsUnique();
        });

        // Games table
        modelBuilder.Entity<GameModel>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.GameName).IsRequired().HasMaxLength(50);
            entity.Property(g => g.Board).IsRequired().HasMaxLength(9);
            entity.Property(g => g.IsActive).HasDefaultValue(true);
            entity.Property(g => g.WinnerId).IsRequired(false);

            //entity.HasOne(g => g.GamePlayerModel)
            //      .WithOne()
            //      .HasForeignKey<GameModel>(g => g.Id)
            //      .OnDelete(DeleteBehavior.Cascade);
        });

        // GamesPlayers table
        modelBuilder.Entity<GamesPlayersModel>(entity =>
        {
            entity.HasKey(gp => gp.Id);
            entity.Property(gp => gp.PlayerTurn).IsRequired();
            entity.Property(gp => gp.PlayerOneId).IsRequired();
            entity.Property(gp => gp.PlayerTwoId).IsRequired(false);
            entity.Property(gp => gp.GameId).IsRequired();

            entity.HasOne(gp => gp.PlayerOne)
                  .WithMany()
                  .HasForeignKey(gp => gp.PlayerOneId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(gp => gp.PlayerTwo)
                  .WithMany()
                  .HasForeignKey(gp => gp.PlayerTwoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(gp => gp.Game)
                  .WithMany()
                  .HasForeignKey(gp => gp.GameId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
