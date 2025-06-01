import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthDialogComponent } from '../components/auth-dialog/auth-dialog.component';

@Injectable({ providedIn: 'root' })
export class AuthDialogService {
  constructor(private dialog: MatDialog) {}

  open(message: string = 'You are not authenticated. Please log in.', redirect = '/login') {
    this.dialog.open(AuthDialogComponent, {
      width: '400px',
      data: { message, redirect }
    });
  }
}
