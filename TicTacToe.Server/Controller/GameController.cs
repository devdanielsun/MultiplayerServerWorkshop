using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Server.Logic;
using TicTacToe.Server.Model;
using TicTacToe.Server.Model.DTOs;

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
    [ProducesResponseType(typeof(ListOfGames), 200)]
    public IActionResult GetGames()
    {

        var games = _context.GamesPlayers
            .Include(gp => gp.Game)
            .Include(gp => gp.PlayerOne)
            .Include(gp => gp.PlayerTwo)
            .Select(g => g)
            .ToList();

        return Ok(Mapper.MapGame(games));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GameDTO), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetGame(int id)
    {
        var game = _context.GamesPlayers
            .Include(gp => gp.Game)
            .Include(gp => gp.PlayerOne)
            .Include(gp => gp.PlayerTwo)
            .Where(g => g.Game.Id == id)
            .Select(g => g)
            .FirstOrDefault();

        if (game == null)
        {
            return NotFound($"Game with ID {id} not found");
        }

        return Ok(Mapper.MapGame(game));
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(GameDTO), 201)]
    [ProducesResponseType(400)]
    public IActionResult CreateGame(int playerId)
    {
        if (!int.TryParse(playerId.ToString(), out _))
        {
            return BadRequest("PlayerId is null");
        }

        // 1. Create and save the GameModel first to get its Id
        var gameModel = new GameModel
        {
            GameName = "New Game",
            IsActive = true,
            Board = new string('_', 9) // Initialize the board with underscores
        };

        _context.Games.Add(gameModel);
        _context.SaveChanges();

        // 2. Create the GamesPlayersModel with the generated GameId
        var gamePlayerModel = new GamesPlayersModel
        {
            GameId = gameModel.Id,
            PlayerOneId = playerId,
            PlayerTurn = playerId // Player who created the game starts
        };

        _context.GamesPlayers.Add(gamePlayerModel);
        _context.SaveChanges();

        // 3. Optionally update the GameName now that Id is known
        gameModel.GameName = $"Game {gameModel.Id}";
        _context.SaveChanges();

        GameDTO gameDTO = new GameDTO()
        {
            id = gameModel.Id,
            gameName = gameModel.GameName,
            isActive = gameModel.IsActive,
            board = gameModel.Board,
            playerOneId = gamePlayerModel.PlayerOneId,
            playerOneName = _context.Players.FirstOrDefault(p => p.Id == gamePlayerModel.PlayerOneId)?.Username ?? "Unknown",
            playerTwoId = -1, // PlayerTwoId will be set when the second player joins
            playerTwoName = "", // PlayerTwoName will be set when the second player joins
            winnerId = -1 // No winner at the start
        };

        return CreatedAtAction(nameof(GetGame), new { id = gameDTO.id }, gameDTO);
    }

    [HttpPost("{id}/join")]
    [ProducesResponseType(typeof(GameDTO), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult JoinGame(int id, int playerId)
    {
        var playerTwo = _context.Players
            .Where(p => p.Id == playerId)
            .First();
        if (playerId == null || playerTwo == null)
        {
            return BadRequest("PlayerId is null or invalid");
        }

        var gamesPlayers = _context.GamesPlayers
            .Include(g => g.Game)
            .Include(gp => gp.PlayerOne)
            .Include(gp => gp.PlayerTwo)
            .Where(g => g.Game.Id == id)
            .First();

        if (gamesPlayers == null || gamesPlayers.Game == null)
        {
            return NotFound($"Game with ID {id} not found");
        }

        if (gamesPlayers.PlayerTwoId != null)
        {
            return BadRequest("Game is already full or invalid");
        }

        if (gamesPlayers.PlayerOneId == playerId)
        {
            return BadRequest("Player cannot join their own game");
        }

        gamesPlayers.PlayerTwoId = playerId;
        _context.SaveChanges();

        return Ok(Mapper.MapGame(gamesPlayers));
    }
}