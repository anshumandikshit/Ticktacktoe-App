using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [ApiController]
    [Route("api/scoreboard")]
    public class ScoreboardController : ControllerBase
    {
        private readonly IScoreboardService _scoreboardService;

        public ScoreboardController(IScoreboardService scoreboardService)
        {
            _scoreboardService = scoreboardService;
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetScoreboard(Guid sessionId)
        {
            var scores = await _scoreboardService.GetScoreboardAsync(sessionId);
            return Ok(scores);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetScoreboard()
        {
            await _scoreboardService.ResetScoreboardAsync();
            return Ok("Scoreboard reset");
        }
    }

}
