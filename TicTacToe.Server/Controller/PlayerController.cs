using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Server.Model;

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
    [ProducesResponseType(typeof(IEnumerable<PlayerModel>), 200)]
    public IActionResult GetPlayers()
    {
        var players = _context.Players.ToList();
        return Ok(players);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlayerModel), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetPlayer(int id)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return NotFound($"Player with ID {id} not found");
        }

        return Ok(player);
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
    [ProducesResponseType(typeof(PlayerModel), 201)]
    [ProducesResponseType(400)]
    public IActionResult CreatePlayer([FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Username is required");
        }

        var existingPlayer = _context.Players.FirstOrDefault(p => p.Username == name);
        if (existingPlayer != null)
        {
            return BadRequest($"Player with username {name} already exists");
        }

        var player = new PlayerModel
        {
            Username = name,
        };

        _context.Players.Add(player);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }
}