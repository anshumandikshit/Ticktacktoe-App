import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Move } from '../../models/Move';

@Component({
  selector: 'app-move-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './move-history.html',
  styleUrls: ['./move-history.scss']
})
export class MoveHistoryComponent {
  @Input() moveHistory: Move[] = [];

  formatAction(action: string): string {
    const coords = action.match(/\((\d+),(\d+)\)/);
    if (coords) {
      const row = parseInt(coords[1], 10);
      const col = parseInt(coords[2], 10);
      return `Row ${row}, Column ${col}`;
    }
    return action;
  }
}
