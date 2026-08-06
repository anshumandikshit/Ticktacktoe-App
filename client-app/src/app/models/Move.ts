export interface Move {
  id?: number;
  gameId?: number;
  player: string;   // "X" or "O"
  action: string;   // required by backend
  timestamp?: string;
}
