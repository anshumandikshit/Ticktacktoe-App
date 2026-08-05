using API.Models;

namespace API.Repositories.Interface
{
    public interface IScoreboardRepository
    {
        Task<List<Scoreboard>> GetScoreboardAsync();
        Task ResetScoreboardAsync();
        Task SaveChangesAsync();
    }
}
