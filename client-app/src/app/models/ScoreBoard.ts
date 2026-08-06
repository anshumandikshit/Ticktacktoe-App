export interface ScoreBoard {
  id: number;          // Primary key
  sessionId: string;   // Unique per session
  xWins: number;       // Total wins by X
  oWins: number;       // Total wins by O
  draws: number;       // Total draws
}