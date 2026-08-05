using API.Models;
using API.Repositories.Interface;
using API.Services.Interface;

namespace API.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;

        public GameService(IGameRepository repo)
        {
            _repo = repo;
        }

        public async Task<Game> CreateGameAsync(Game game) => await _repo.AddGameAsync(game);

        public async Task<Game?> GetGameAsync(int id) => await _repo.GetGameAsync(id);

        public async Task<Move> SubmitMoveAsync(int gameId, Move move)
        {
            move.GameId = gameId;
            move.Timestamp = DateTime.UtcNow;
            return await _repo.AddMoveAsync(move);
        }

        public async Task<bool> UndoLastMoveAsync(int gameId)
        {
            var lastMove = await _repo.GetLastMoveAsync(gameId);
            if (lastMove == null) return false;
            await _repo.RemoveMoveAsync(lastMove);
            return true;
        }

        public async Task<bool> ResetGameAsync(int gameId)
        {
            var game = await _repo.GetGameAsync(gameId);
            if (game == null) return false;

            foreach (var move in game.Moves.ToList())
                await _repo.RemoveMoveAsync(move);

            game.Status = "Reset";
            await _repo.SaveChangesAsync();
            return true;
        }
    }


}
