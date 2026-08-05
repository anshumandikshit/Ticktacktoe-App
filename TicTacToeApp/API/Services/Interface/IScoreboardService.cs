using API.Models;

namespace API.Services.Interface
{
    public interface IScoreboardService
    {
        Task<List<Scoreboard>> GetScoreboardAsync();
        Task ResetScoreboardAsync();
    }

}
