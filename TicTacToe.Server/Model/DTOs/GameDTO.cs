using System.Collections.Generic;

namespace TicTacToe.Server.Model.DTOs;

public class GameDTO
{
    public int id { get; set; }
    public required string gameName { get; set; }
    public required bool isActive { get; set; }
    public required string board { get; set; } = new string(' ', 9); // Default to empty board
    public int? winnerId { get; set; } = null;
    public required int playerOneId { get; set; }
    public required string playerOneName { get; set; }
    public int? playerTwoId { get; set; } = null;
    public string? playerTwoName { get; set; } = null;
    public int? playerTurn { get; set; } = null;
}


public class ListOfGames
{
    public List<GameDTO> games { get; set; }

    public ListOfGames()
    {
        games = new List<GameDTO>();
    }
}