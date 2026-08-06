import { Component } from '@angular/core';
import { GameService } from '../../services/game';
import { CommonModule } from '@angular/common';
import { Game } from '../../models/Game';
import { Move } from '../../models/Move';
import { ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-tic-tac-toe',
  standalone: true,
  templateUrl: './tic-tac-toe.html',
  styleUrls: ['./tic-tac-toe.scss'],
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.Default 
})
export class TicTacToeComponent {
  board: string[] = Array(9).fill('');
  currentPlayer: 'X' | 'O' = 'X';
  winner: string | null = null;
  gameId: number | null = null;
  scoreboard: any[] = [];
  winningCells: number[] = []; //  track winning indices
  moveHistory: Move[] = [];

  constructor(private gameService: GameService, private cd: ChangeDetectorRef) { }

  public startGame() {
    const newGame: Game = {
      player1: 'X',
      player2: 'O',
      currentTurn: 'X',
      status: 'Active',
      moves: []
    };

    this.gameService.createGame(newGame).subscribe(game => {
      this.gameId = game.id;
      this.board = [...Array(9).fill('')];
      this.currentPlayer = 'X';
      this.winner = null;
      this.winningCells = [];
      this.loadScoreboard();
      this.cd.detectChanges(); 
    });
  }

  public makeMove(index: number) {
    if (!this.gameId || this.board[index] || this.winner) return;

    const move: Move = {
      gameId: this.gameId,
      player: this.currentPlayer,
      action: `${this.currentPlayer} at (${Math.floor(index / 3)},${index % 3})`
    };

    this.gameService.submitMove(this.gameId, move).subscribe(() => {
      this.reloadGame();
      this.togglePlayer();
      this.checkWinner(); //  detect winner locally
      this.cd.detectChanges(); 
    });
  }

  public reloadGame() {
    if (this.gameId) {
      this.gameService.getGame(this.gameId).subscribe(game => {
        const newBoard = [...Array(9).fill('')];
        if (game.moves) {
          game.moves.forEach((m: Move) => {
            const coords = m.action.match(/\((\d+),(\d+)\)/);
            if (coords) {
              const row = parseInt(coords[1], 10);
              const col = parseInt(coords[2], 10);
              const idx = row * 3 + col;
              newBoard[idx] = m.player;
            }
          });
        };
        this.moveHistory = game.moves || [];
        this.board = newBoard;
        this.checkWinner(); //  run winner detection after reload
        this.cd.detectChanges(); 
      });
    }
  }

  public resetGame() {
    if (this.gameId) {
      this.gameService.resetGame(this.gameId).subscribe(() => {
        this.board = [...Array(9).fill('')];
        this.currentPlayer = 'X';
        this.winner = null;
        this.winningCells = [];
        this.reloadGame();
        this.cd.detectChanges(); 
      });
    }
  }

  public undoMove() {
    if (this.winner) {
    return; //  do nothing if winner exists
    }
    if (this.gameId) {
      this.gameService.undoMove(this.gameId).subscribe(() => {
        this.reloadGame();
        this.cd.detectChanges(); 
      });
    }
  }

  public loadScoreboard() {
    this.gameService.getScoreboard().subscribe(scores => {
      this.scoreboard = scores;
      this.cd.detectChanges(); 
    });
  }

  public resetScoreboard() {
    this.gameService.resetScoreboard().subscribe(() => {
      this.scoreboard = [];
      this.cd.detectChanges(); 
    });
  }

  public togglePlayer() {
    this.currentPlayer = this.currentPlayer === 'X' ? 'O' : 'X';
  }

  private checkWinner() {
    const combos = [
      [0,1,2],[3,4,5],[6,7,8], // rows
      [0,3,6],[1,4,7],[2,5,8], // cols
      [0,4,8],[2,4,6]          // diagonals
    ];

    for (const [a,b,c] of combos) {
      if (this.board[a] && this.board[a] === this.board[b] && this.board[a] === this.board[c]) {
        this.winner = this.board[a];
        this.winningCells = [a,b,c]; //  highlight winning cells
        return;
      }
    }

    if (!this.board.includes('') && !this.winner) {
      this.winner = 'Draw';
      this.winningCells = [];
    }
  }

  public formatAction(action: string): string {
  const coords = action.match(/\((\d+),(\d+)\)/);
  if (coords) {
    const row = parseInt(coords[1], 10);
    const col = parseInt(coords[2], 10);
    return `Row ${row}, Column ${col}`;
  }
  return action; // fallback
}
}
