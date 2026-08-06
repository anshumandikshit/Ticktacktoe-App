import { Move } from "./Move";
export interface Game {
  id?: number;
  player1: string;
  player2: string;
  currentTurn: string; // "Player1" | "Player2" | "Computer"
  status: string;      // "Active" | "Completed" | "Reset"
  moves?: Move[];
}