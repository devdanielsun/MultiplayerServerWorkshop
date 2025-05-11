using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Server.Model;

[Table("players")]
public class PlayerModel
{
    [Column("player_id")]
    public int Id { get; set; }
    [Column("username")]
    public required string Username { get; set; }

    public ICollection<GamePlayerModel>? GamePlayers { get; set; }
}
