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

        public async Task<List<Scoreboard>> GetScoreboardAsync()
        {
            return await _repo.GetScoreboardAsync();
        }

        public async Task ResetScoreboardAsync()
        {
            await _repo.ResetScoreboardAsync();
        }
    }


}
