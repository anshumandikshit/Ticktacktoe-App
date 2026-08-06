using API.Models;

namespace API.Repositories.Interface
{
    public interface IGameRepository
    {
        Task<Game> AddGameAsync(Guid sessionId, Game game);
        Task<Game?> GetGameAsync(int id);
        Task<Move> AddMoveAsync(Move move);
        Task<Move?> GetLastMoveAsync(int gameId);
        Task RemoveMoveAsync(Move move);

        Task<Game> UpdateCurrentTurnGameAssync(Game game);
        Task SaveChangesAsync();
    }
}
