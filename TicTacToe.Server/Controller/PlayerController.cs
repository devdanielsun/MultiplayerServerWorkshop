using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Server.Model;
using TicTacToe.Server.Model.DTOs;

namespace TicTacToe.Server.Controller;

[ApiController]
[Route("api/players/")]
public class PlayerController : ControllerBase
{
    private readonly DatabaseContext _context;

    public PlayerController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(ListOfPlayers), 200)]
    public IActionResult GetPlayers()
    {
        var players = _context.Players.ToList();

        ListOfPlayers playersDTOs = new ListOfPlayers();

        foreach (var player in players)
        {
            PlayerDTO playerDTO = new PlayerDTO()
            {
                id = player.Id,
                username = player.Username,
            };
            playersDTOs.players.Add(playerDTO);
        }

        return Ok(playersDTOs);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlayerDTO), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetPlayer(int id)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return NotFound($"Player with ID {id} not found");
        }

        PlayerDTO playerDTO = new PlayerDTO()
        {
            id = player.Id,
            username = player.Username,
        };

        return Ok(playerDTO);
    }

    [HttpGet("{id}/name")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetPlayerName(int id)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return NotFound($"Player with ID {id} not found");
        }

        return Ok(player.Username);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(PlayerDTO), 201)]
    [ProducesResponseType(400)]
    public IActionResult CreatePlayer([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Username is required");
        }

        var existingPlayer = _context.Players.Where(p => p.Username == username).FirstOrDefault();
        if (existingPlayer != null)
        {
            return Created($"api/players/{existingPlayer.Id}", existingPlayer);
        }

        var player = new PlayerModel
        {
            Username = username,
        };

        _context.Players.Add(player);
        _context.SaveChanges();

        PlayerDTO playerDTO = new PlayerDTO()
        {
            id = player.Id,
            username = player.Username,
        };

        return CreatedAtAction(nameof(GetPlayer), new { id = playerDTO.id }, playerDTO);
    }
}