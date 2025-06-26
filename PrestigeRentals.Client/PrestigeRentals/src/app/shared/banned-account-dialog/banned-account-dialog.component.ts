import { Component } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-banned-account-dialog',
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './banned-account-dialog.component.html',
  styleUrl: './banned-account-dialog.component.scss'
})
export class BannedAccountDialogComponent {

}
