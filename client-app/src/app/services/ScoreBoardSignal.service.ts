import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class ScoreboardSignalRService {
  private hubConnection!: signalR.HubConnection;

  startConnection(onUpdate: (scoreboard: any) => void) {
    this.hubConnection = new signalR.HubConnectionBuilder()
  .withUrl('https://localhost:7199/scoreboardHub')
  .build();

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected');
    });

    this.hubConnection.on('ScoreboardUpdated', (scoreboard) => {
      console.log('Update received', scoreboard);
      onUpdate(scoreboard);
    });
  }
}
