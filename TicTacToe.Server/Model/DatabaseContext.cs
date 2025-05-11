using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Server.Model;

public class DatabaseContext : DbContext
{
    public DbSet<GameModel> Games { get; set; }
    public DbSet<PlayerModel> Players { get; set; }
    public DbSet<GamePlayerModel> GamePlayers { get; set; } // Added DbSet for game_players

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships for game_players
        // modelBuilder.Entity<GamePlayerModel>()
        //     .HasOne(gp => gp.Game)
        //     .WithMany(g => g.GamePlayers)
        //     .HasForeignKey(gp => gp.GameId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<GamePlayerModel>()
        //     .HasOne(gp => gp.PlayerOne)
        //     .WithMany(p => p.GamePlayers)
        //     .HasForeignKey(gp => gp.PlayerOneId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<GamePlayerModel>()
        //     .HasOne(gp => gp.PlayerTwo)
        //     .WithMany()
        //     .HasForeignKey(gp => gp.PlayerTwoId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<GamePlayerModel>()
        //     .HasOne(gp => gp.CurrentTurnPlayer)
        //     .WithMany()
        //     .HasForeignKey(gp => gp.PlayerTurn)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
