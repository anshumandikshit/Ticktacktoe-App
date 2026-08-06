using System.Text.RegularExpressions;
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

            // Load the game with all moves
            var game = await _context.Games
                .Include(g => g.Moves)
                .FirstOrDefaultAsync(g => g.Id == move.GameId);

            if (game != null)
            {
                // Build board
                var board = new string[3, 3];
                foreach (var m in game.Moves)
                {
                    var coords = Regex.Match(m.Action, @"\((\d+),(\d+)\)");
                    if (coords.Success)
                    {
                        int row = int.Parse(coords.Groups[1].Value);
                        int col = int.Parse(coords.Groups[2].Value);
                        board[row, col] = m.Player;
                    }
                }

                // Update game status
                if (HasWinner(board))
                {
                    game.Status = "Completed";
                    game.CurrentTurn = move.Player; // winner
                }
                else if (game.Moves.Count == 9)
                {
                    game.Status = "Completed";
                    game.CurrentTurn = "Draw";
                }
                else
                {
                    game.Status = "Active";
                    game.CurrentTurn = move.Player == "X" ? "O" : "X";
                }

                await _context.SaveChangesAsync();
            }
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

        private bool HasWinner(string[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(board[i, 0]) &&
                    board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;

                if (!string.IsNullOrEmpty(board[0, i]) &&
                    board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return true;
            }

            if (!string.IsNullOrEmpty(board[0, 0]) &&
                board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;

            if (!string.IsNullOrEmpty(board[0, 2]) &&
                board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;

            return false;
        }
    }

}
