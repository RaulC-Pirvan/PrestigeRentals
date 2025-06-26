import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-auth-dialog',
  imports: [],
  templateUrl: './auth-dialog.component.html',
  styleUrl: './auth-dialog.component.scss',
})
export class AuthDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { message: string; redirect: string },
    private router: Router,
    private dialogRef: MatDialogRef<AuthDialogComponent>
  ) {}

  goToLogin() {
    this.dialogRef.close();
    this.router.navigate([this.data.redirect]);
  }
}
