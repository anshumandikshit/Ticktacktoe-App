import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Game } from '../models/Game';
import { Move } from '../models/Move';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private baseUrl = 'https://localhost:7199/api'; 

  constructor(private http: HttpClient) {}

  // Create new game
  createGame(newGame : Game): Observable<any> {
    return this.http.post(`${this.baseUrl}/games`, newGame);
  }

  // Get game state
  getGame(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/games/${id}`);
  }

  // Submit move
  submitMove(id: number, move : Move): Observable<any> {
    return this.http.post(`${this.baseUrl}/games/${id}/moves`, move);
  }

  // Reset game
  resetGame(id: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/games/${id}/reset`, {});
  }

  // Undo last move
  undoMove(id: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/games/${id}/undo`, {});
  }

  // Scoreboard
  getScoreboard(): Observable<any> {
    return this.http.get(`${this.baseUrl}/scoreboard`);
  }

  resetScoreboard(): Observable<any> {
    return this.http.post(`${this.baseUrl}/scoreboard/reset`, {});
  }
}
