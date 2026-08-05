using API.Data;
using API.Models;
using API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Game> AddGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> GetGameAsync(int id)
        {
            return await _context.Games.Include(g => g.Moves)
                                       .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Move> AddMoveAsync(Move move)
        {
            _context.Moves.Add(move);
            await _context.SaveChangesAsync();
            return move;
        }

        public async Task<Move?> GetLastMoveAsync(int gameId)
        {
            return await _context.Moves
                .Where(m => m.GameId == gameId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveMoveAsync(Move move)
        {
            _context.Moves.Remove(move);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }

    

}
