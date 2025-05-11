using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Server.Model;

namespace TicTacToe.Server.Controller;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly DatabaseContext _context;

    public GameController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("games")]
    [ProducesResponseType(typeof(IEnumerable<GameModel>), 200)]
    public IActionResult GetGames()
    {
        var games = _context.Games.ToList();
        return Ok(games);
    }

    [HttpGet("games/{id}")]
    [ProducesResponseType(typeof(GameModel), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetGame(int id, int playerId)
    {
        var gameModel = _context.Games.FirstOrDefault(g => g.Id == id);
        if (gameModel == null)
        {
            return NotFound($"Game with ID {id} not found");
        }

        var gamePlayer = _context.GamePlayers.FirstOrDefault(gp => gp.GameId == id);
        if (gamePlayer == null)
        {
            return BadRequest("Game is invalid");
        }

        if (gamePlayer.PlayerTwoId != null)
        {
            return BadRequest("Game is already full");
        }

        // Automatically assign the player to the game
        gamePlayer.PlayerTwoId = playerId;
        _context.GamePlayers.Update(gamePlayer);
        _context.SaveChanges();

        return Ok(new
        {
            gameModel.Id,
            gameModel.GameName,
            gameModel.IsActive,
            gameModel.Board,
            gameModel.WinnerId,
            gamePlayer.PlayerOneId,
            gamePlayer.PlayerTwoId
        });
    }

    [HttpPost("games/create")]
    [ProducesResponseType(typeof(GameModel), 201)]
    [ProducesResponseType(400)]
    public IActionResult CreateGame(int playerId)
    {
        if (!int.TryParse(playerId.ToString(), out _))
        {
            return BadRequest("PlayerId is null");
        }

        var gameModel = new GameModel
        {
            GameName = "New Game",
            IsActive = true,
            Board = new string(' ', 9) // Initialize the board with empty spaces
        };

        _context.Games.Add(gameModel);
        _context.SaveChanges();

        var gamePlayerModel = new GamePlayerModel
        {
            GameId = gameModel.Id,
            PlayerOneId = playerId,
            PlayerTurn = playerId // Player who created the game starts
        };

        _context.GamePlayers.Add(gamePlayerModel);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetGame), new { id = gameModel.Id }, gameModel);
    }

    [HttpPost("games/{id}/join")]
    [ProducesResponseType(typeof(GameModel), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult JoinGame(int id, int playerId)
    {
        var gameModel = _context.Games.FirstOrDefault(g => g.Id == id);
        if (gameModel == null)
        {
            return NotFound($"Game with ID {id} not found");
        }

        var gamePlayer = _context.GamePlayers.FirstOrDefault(gp => gp.GameId == id);
        if (gamePlayer == null || gamePlayer.PlayerTwoId != null)
        {
            return BadRequest("Game is already full or invalid");
        }

        gamePlayer.PlayerTwoId = playerId;
        _context.SaveChanges();

        return Ok(gameModel);
    }
}