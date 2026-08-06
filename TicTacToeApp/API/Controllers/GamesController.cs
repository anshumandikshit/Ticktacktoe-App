using API.Models;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("{sessionId}")]
        public async Task<IActionResult> CreateGame(Guid sessionId, [FromBody] Game game)
        {
            var created = await _gameService.CreateGameAsync(sessionId,game);
            return Ok(created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _gameService.GetGameAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost("{id}/moves")]
        public async Task<IActionResult> SubmitMove(int id, [FromBody] Move move)
        {
            var submitted = await _gameService.SubmitMoveAsync(id, move);
            return Ok(submitted);
        }

        [HttpPost("{id}/undo")]
        public async Task<IActionResult> UndoMove(int id)
        {
            var success = await _gameService.UndoLastMoveAsync(id);
            return success ? Ok(success) : NotFound();
        }

        [HttpPost("{id}/reset")]
        public async Task<IActionResult> ResetGame(int id)
        {
            var success = await _gameService.ResetGameAsync(id);
            return success ? Ok(success) : NotFound();
        }
    }

}
