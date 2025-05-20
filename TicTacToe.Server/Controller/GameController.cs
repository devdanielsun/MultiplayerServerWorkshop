using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Server.Model;

namespace TicTacToe.Server.Controller;

[ApiController]
[Route("api/games/")]
public class GameController : ControllerBase
{
    private readonly DatabaseContext _context;

    public GameController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GameModel>), 200)]
    public IActionResult GetGames()
    {
        var games = _context.Games
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.PlayerOne)
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.PlayerTwo)
            .Select(g => new
            {
                g.Id,
                g.GameName,
                g.IsActive,
                g.Board,
                Players = g.GamePlayers.Select(gp => new
                {
                    PlayerOne = gp.PlayerOne.Username,
                    PlayerTwo = gp.PlayerTwo.Username
                })
            })
            .ToList();
        return Ok(games);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GameModel), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetGame(int id)
    {
        var game = _context.Games
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.PlayerOne)
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.PlayerTwo)
            .Where(g => g.Id == id)
            .Select(g => new
            {
                g.Id,
                g.GameName,
                g.IsActive,
                g.Board,
                Players = g.GamePlayers.Select(gp => new
                {
                    PlayerOne = gp.PlayerOne.Username,
                    PlayerTwo = gp.PlayerTwo.Username
                })
            })
            .FirstOrDefault();

        if (game == null)
        {
            return NotFound($"Game with ID {id} not found");
        }

        return Ok(game);
    }

    [HttpPost("create")]
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

        GameModel newGame = _context.Games.Add(gameModel).Entity;
        _context.SaveChanges();

        var gamePlayerModel = new GamePlayerModel
        {
            GameId = gameModel.Id,
            PlayerOneId = playerId,
            PlayerTurn = playerId // Player who created the game starts
        };

        _context.GamePlayers.Add(gamePlayerModel);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetGame), new { id = newGame.Id }, newGame);
    }

    [HttpPost("{id}/join")]
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