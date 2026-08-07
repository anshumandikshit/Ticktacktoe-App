using API.Migrations;
using API.Models;
using API.Repositories.Interface;
using API.Services.Interface;
using static API.Enums.ApplicationEnumscs;

namespace API.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;

        public GameService(IGameRepository repo)
        {
            _repo = repo;
        }

        public async Task<Game> CreateGameAsync(Guid sessionId, Game game) => await _repo.AddGameAsync(sessionId, game);

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
            var game = await _repo.GetGameAsync(gameId);
            if (lastMove == null) return false;
            if (game == null) return false;
            game.CurrentTurn = lastMove.Player;
            await _repo.RemoveMoveAsync(lastMove);
            if (game.GameType == GameType.PvC.ToString())
            {
                lastMove = await _repo.GetLastMoveAsync(gameId);
                await _repo.RemoveMoveAsync(lastMove);

            }
            await _repo.UpdateCurrentTurnGameAssync(game);
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
