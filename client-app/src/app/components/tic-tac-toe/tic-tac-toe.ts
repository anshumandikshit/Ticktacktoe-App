import { Component } from '@angular/core';
import { GameService } from '../../services/game';
import { CommonModule } from '@angular/common';
import { Game } from '../../models/Game';
import { Move } from '../../models/Move';
import { NgZone } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { ChangeDetectionStrategy } from '@angular/core';

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
  winner: string | null | undefined= null;
  gameId: number | null | undefined= null;
  scoreboard: any[] = [];

  constructor(private gameService: GameService,private cd: ChangeDetectorRef) { }

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
        this.loadScoreboard();
        this.cd.detectChanges(); 
    });
  }

  public makeMove(index: number) {
    if (!this.gameId || this.board[index] || this.winner) return;

    const move: Move = {
      gameId: this.gameId, // ✅ required by backend
      player: this.currentPlayer,
      action: `${this.currentPlayer} at (${Math.floor(index / 3)},${index % 3})`
    };

    this.gameService.submitMove(this.gameId, move).subscribe(() => {
      this.reloadGame();
      this.togglePlayer();
      this.cd.detectChanges(); 
    });
  }

  public reloadGame() {
  if (this.gameId) {
    this.gameService.getGame(this.gameId).subscribe(game => {
      const newBoard = Array(9).fill('');
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
      }
      this.board = newBoard; // ✅ new reference triggers Angular update
      this.winner = game.status === 'Completed' ? game.currentTurn : null;
      this.cd.detectChanges(); 
    });
  }
}



  public resetGame() {
    if (this.gameId) {
      this.gameService.resetGame(this.gameId).subscribe(() => {
        this.board = Array(9).fill('');
        this.currentPlayer = 'X';
        this.winner = null;
        this.cd.detectChanges(); 
      });
    }
  }

  public undoMove() {
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
}
