using API.Models;
using API.Repositories.Interface;
using API.Services.Interface;

namespace API.Services
{
    public class ScoreboardService : IScoreboardService
    {
        private readonly IScoreboardRepository _repo;

        public ScoreboardService(IScoreboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<Scoreboard> GetScoreboardAsync(Guid sessionId)
        {
            return await _repo.GetScoreboardAsync(sessionId);
        }

        public async Task ResetScoreboardAsync()
        {
            await _repo.ResetScoreboardAsync();
        }
    }


}
