using System.Text.RegularExpressions;
using API.Data;
using API.Migrations;
using API.Models;
using API.Repositories.Interface;
using API.WebSocket;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static API.Enums.ApplicationEnumscs;

namespace API.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;
        private readonly IHubContext<ScoreboardHub> _hubContext;

        public GameRepository(GameDbContext context, IHubContext<ScoreboardHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<Game> AddGameAsync(Guid sessionId, Game game)
        {
            game.SessionId = sessionId;
            if(game.GameType== GameType.PvP.ToString())
            {
                game.Player1 = PlayerCategory.X.ToString();
                game.Player2 = PlayerCategory.O.ToString();
            }
            else
            {
                game.Player1 = PlayerCategory.X.ToString();
                game.Player2 = PlayerCategory.O.ToString();
            }
                _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> GetGameAsync(int id)
        {
            return await _context.Games.Include(g => g.Moves)
                                       .FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task<Game> UpdateCurrentTurnGameAssync(Game game)
        {
            var gameContext = await _context.Games.Where(g => g.Id == game.Id).FirstOrDefaultAsync();
            if (gameContext.GameType == GameType.PvC.ToString())
            {
                gameContext.CurrentTurn = PlayerCategory.X.ToString();
            }
            else
            {
                gameContext.CurrentTurn = game.CurrentTurn;
            }
            await _context.SaveChangesAsync();
            return gameContext;
        }

        public async Task<Move> AddMoveAsync(Move move)
        {
            var game = await _context.Games
                .Include(g => g.Moves)
                .FirstOrDefaultAsync(g => g.Id == move.GameId);

            if (game == null) return move;

            // Add human move
            _context.Moves.Add(move);
            await _context.SaveChangesAsync();
            await _context.Entry(game).Collection(g => g.Moves).LoadAsync();

            var board = BuildBoard(game.Moves);

            // If PvC and human just played, let Computer respond
            if (game.GameType == GameType.PvC.ToString()
                && move.Player == "X"
                && !HasWinner(board)
                && game.Moves.Count < 9)
            {
                var computerMove = GenerateComputerMove(game, board);
                _context.Moves.Add(computerMove);
                await _context.SaveChangesAsync();

                await _context.Entry(game).Collection(g => g.Moves).LoadAsync();
                board = BuildBoard(game.Moves);
            }

            var scoreboard = await _context.Scoreboards
                .FirstOrDefaultAsync(s => s.SessionId == game.SessionId);

            if (scoreboard == null)
            {
                scoreboard = new Scoreboard { SessionId = game.SessionId };
                _context.Scoreboards.Add(scoreboard);
            }

            // Determine winner based on board
            if (HasWinner(board))
            {
                game.Status = "Completed";

                // Find last move (winner)
                var lastMove = game.Moves.OrderByDescending(m => m.Timestamp).First();
                var winner = lastMove.Player;
                game.CurrentTurn = winner; // store winner

                switch (winner)
                {
                    case "X":
                        scoreboard.XWins++;
                        break;
                    case "O":
                        if (game.GameType == GameType.PvC.ToString())
                            scoreboard.OWins++;
                        else
                            scoreboard.OWins++;
                        break;
                }
            }
            else if (game.Moves.Count == 9)
            {
                game.Status = "Completed";
                game.CurrentTurn = "Draw";
                scoreboard.Draws++;
            }
            else
            {
                game.Status = "Active";

                // Next turn should be based on last move
                var lastMove = game.Moves.OrderByDescending(m => m.Timestamp).First();
                game.CurrentTurn = lastMove.Player == "X" ? "O" : "X";
            }

            await _context.SaveChangesAsync();
            await NotifyScoreboardUpdate(game.SessionId, scoreboard);
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


        private string[,] BuildBoard(IEnumerable<Move> moves)
        {
            var board = new string[3, 3];
            foreach (var m in moves)
            {
                var coords = Regex.Match(m.Action, @"\((\d+),(\d+)\)");
                if (coords.Success)
                {
                    int row = int.Parse(coords.Groups[1].Value);
                    int col = int.Parse(coords.Groups[2].Value);
                    board[row, col] = m.Player;
                }
            }
            return board;
        }

        private Move GenerateComputerMove(Game game, string[,] board)
        {
            // Simple strategy: first empty cell
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(board[row, col]))
                    {
                        return new Move
                        {
                            GameId = game.Id,
                            Player = "O",
                            Action = $"O at ({row},{col})",
                            Timestamp = DateTime.UtcNow
                        };
                    }
                }
            }
            throw new InvalidOperationException("No empty cells left");
        }

        private async Task NotifyScoreboardUpdate(Guid sessionId, Scoreboard scoreboard)
        {
            await _hubContext.Clients.All.SendAsync("ScoreboardUpdated", scoreboard);
        }
    }

}
