import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TicTacToeComponent } from "./components/tic-tac-toe/tic-tac-toe";
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TicTacToeComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('client-app');

  ngOnInit() {
    let sessionId = localStorage.getItem('sessionId');
    if (!sessionId) {
      sessionId = uuidv4(); // generate new session ID
      localStorage.setItem('sessionId', sessionId);
    }
    console.log("Session ID:", sessionId);
  }
}
