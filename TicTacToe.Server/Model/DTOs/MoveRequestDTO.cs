namespace TicTacToe.Server.Model.DTOs;

public class MoveRequestDTO
{
    public int playerId { get; set; }
    public int tileIndex { get; set; }
}
