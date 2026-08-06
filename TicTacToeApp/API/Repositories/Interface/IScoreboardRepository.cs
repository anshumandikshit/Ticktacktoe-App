using API.Models;

namespace API.Repositories.Interface
{
    public interface IScoreboardRepository
    {
        Task<Scoreboard> GetScoreboardAsync(Guid sessionId);
        Task ResetScoreboardAsync();
        Task SaveChangesAsync();
    }
}
