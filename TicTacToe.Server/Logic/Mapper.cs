using System.Collections.Generic;
using TicTacToe.Server.Model;
using TicTacToe.Server.Model.DTOs;

namespace TicTacToe.Server.Logic;
public static class Mapper
{
    public static ListOfGames MapGame(List<GamesPlayersModel> gp)
    {
        List<GameDTO> gameDTOs = new List<GameDTO>();
        foreach (var game in gp)
        {
            gameDTOs.Add(Mapper.MapGame(game));
        }
        return new ListOfGames() { games = gameDTOs };
    }

    public static GameDTO MapGame(GamesPlayersModel gp) {
        return new GameDTO()
        {
            id = gp.Game.Id,
            gameName = gp.Game.GameName,
            isActive = gp.Game.IsActive,
            board = gp.Game.Board,
            playerOneId = gp.PlayerOneId,
            playerOneName = gp.PlayerOne.Username,
            playerTwoId = gp.PlayerTwoId ?? -1, // Use -1 to represent "no player two" when null
            playerTwoName = gp.PlayerTwo != null ? gp.PlayerTwo.Username : "", // Safely handle null
            winnerId = gp.Game.WinnerId ?? -1, // Use -1 to represent "no winner" when null
            playerTurn = gp.PlayerTurn
        };
    }
}
