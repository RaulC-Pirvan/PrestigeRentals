import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { TitleComponent } from '../../shared/title/title.component';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { BannedAccountDialogComponent } from '../../shared/banned-account-dialog/banned-account-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-login-form',
  imports: [
    ButtonComponent,
    TitleComponent,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
  ],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss',
})
export class LoginFormComponent {
  loginForm: FormGroup;

  rememberMe = false;
  notificationMessage: string | null = null;
  notificationType: 'success' | 'error' = 'success';

  showForgotPassword = false;
  forgotEmail: string = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService,
    private dialog: MatDialog
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', Validators.required],
      rememberMe: [false],
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    if (this.loginForm.valid) {
      const { email, password, rememberMe } = this.loginForm.value;

      this.http
        .post<{
          token: string;
          firstName: string;
          lastName: string;
          imageData: string;
        }>('/api/login', { email, password })
        .subscribe({
          next: (response) => {
            const token = response.token;

            this.authService.login(token, rememberMe);
            this.notificationService.show('Login successful!', 'success');

            setTimeout(() => {
              this.authService.loggedInSubject.next(true);
              this.authService.loadUserProfile();
              this.router.navigate(['/']);
            }, 1000);
          },
          error: (err) => {
            if (err.status === 403)
            {
              this.dialog.open(BannedAccountDialogComponent);
            } else if(err.status === 401 || err.status === 404) {
              this.notificationService.show('Invalid credentials', 'error');
            } else {
              this.notificationService.show('Unexpected error occured', 'error');
            }
          }
        });
    }
  }

  redirectTo(path: string): void {
    this.router.navigate([path]);
  }

  openForgotPasswordModal() {
    this.showForgotPassword = true;
  }

  closeForgotPasswordModal() {
    this.showForgotPassword = false;
    this.forgotEmail = '';
  }

  sendResetLink() {
    if (!this.forgotEmail || !this.forgotEmail.includes('@')) {
      this.notificationService.show(
        'Please enter a valid email address.',
        'error'
      );
      return;
    }

    this.http
      .post('https://localhost:7093/forgot-password', {
        email: this.forgotEmail,
      })
      .subscribe({
        next: () => {
          this.notificationService.show(
            'A new password has been sent to your email.',
            'success'
          );
          this.closeForgotPasswordModal();
        },
        error: () => {
          this.notificationService.show(
            'Something went wrong. Please try again.',
            'error'
          );
        },
      });
  }
}
