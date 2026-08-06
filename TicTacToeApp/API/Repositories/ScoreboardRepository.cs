using API.Data;
using API.Models;
using API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ScoreboardRepository : IScoreboardRepository
    {
        private readonly GameDbContext _context;

        public ScoreboardRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Scoreboard> GetScoreboardAsync(Guid sessionId)
        {
            return await _context.Scoreboards.FirstAsync();
        }

        public async Task ResetScoreboardAsync()
        {
            _context.Scoreboards.RemoveRange(_context.Scoreboards);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
