using System.Collections.Generic;

namespace TicTacToe.Server.Model.DTOs;
public class PlayerDTO
{
    public int id { get; set; }
    public required string username { get; set; }
}

public class ListOfPlayers
{
    public List<PlayerDTO> players { get; set; }

    public ListOfPlayers()
    {
        players = new List<PlayerDTO>();
    }
}