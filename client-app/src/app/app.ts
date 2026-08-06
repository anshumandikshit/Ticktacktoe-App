import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TicTacToeComponent } from "./components/tic-tac-toe/tic-tac-toe";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TicTacToeComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('client-app');
}
