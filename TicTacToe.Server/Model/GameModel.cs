using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Server.Model;

[Table("games")]
public class GameModel
{
    [Column("game_id")]
    public int Id { get; set; }
    [Column("game_name")]
    public string GameName { get; set; }
    [Column("is_active")]
    public bool IsActive { get; set; }
    [Column("board")]
    public string Board { get; set; } = new string(' ', 9); // Default to empty board
    [Column("winner_id")]
    public int? WinnerId { get; set; }

    public PlayerModel? Winner { get; set; }

    public ICollection<GamePlayerModel> GamePlayers { get; set; }
}
