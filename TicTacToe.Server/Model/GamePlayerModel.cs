using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Server.Model;

[Table("game_players")]
public class GamePlayerModel
{
    [Column("game_player_id")]
    public int Id { get; set; }

    [Column("game_id")]
    public required int GameId { get; set; }

    [Column("player_one_id")]
    public required int PlayerOneId { get; set; }

    [Column("player_two_id")]
    public int? PlayerTwoId { get; set; }

    [Column("player_turn")]
    public int? PlayerTurn { get; set; }

    public GameModel? Game { get; set; }
    public PlayerModel? PlayerOne { get; set; }
    public PlayerModel? PlayerTwo { get; set; }
    public PlayerModel? CurrentTurnPlayer { get; set; }
}