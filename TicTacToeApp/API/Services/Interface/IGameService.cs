using API.Models;

namespace API.Services.Interface
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(Guid sessionId,Game game);
        Task<Game?> GetGameAsync(int id);
        Task<Move> SubmitMoveAsync(int gameId, Move move);
        Task<bool> UndoLastMoveAsync(int gameId);
        Task<bool> ResetGameAsync(int gameId);
    }
}
