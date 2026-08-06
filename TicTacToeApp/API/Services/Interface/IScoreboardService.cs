using API.Models;

namespace API.Services.Interface
{
    public interface IScoreboardService
    {
        Task<Scoreboard> GetScoreboardAsync(Guid sessionId);
        Task ResetScoreboardAsync();
    }

}
