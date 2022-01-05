import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Stats } from 'src/model/Stats';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent {

  constructor(
    public dialogRef: MatDialogRef<StatsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Stats
  ) { }
}
